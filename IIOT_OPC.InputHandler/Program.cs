using IIOT_OPC.DataAccess;
using IIOT_OPC.Shared.Configuration;
using IIOT_OPC.Shared.Extensions;
using IIOT_OPC.Shared.Models;
using System;

namespace IIOT_OPC.InputHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            //dato giornaliero sul polling

            /*check date*/
            /*calculate oee*/
            /**/
            /**/
            ProgramState state = ProgramState.checkingDate;
            DateTime defaultDate = new DateTime();
            DateTime dateToCheck = defaultDate;
            TimeSpan cycleTime = new TimeSpan(0,0,20);// arbitrario
            double availability = 0;
            double quality = 0;
            double performance = 0;
            int productedPieces = 0;
            int defectedPieces = 0;
            TimeSpan plannedProductionTime =new TimeSpan(0);
            TimeSpan productionTime = new TimeSpan(0);

            while (true)
            {
                switch (state)
                {
                    case ProgramState.checkingDate:
                        Console.WriteLine("sono dentro checkingDate");
                        dateToCheck = CheckDateFromUser();
                        // actually this check is unnecessary because CheckDateFromUser returns only
                        // after a valid date was input
                        //if (dateToCheck != defaultDate)
                        //{
                            state = ProgramState.getDataFromDb;
                        //}
                        break;

                        // excecution stratight through the switch is not allowed in c#
                        // https://stackoverflow.com/questions/6696692/control-cannot-fall-through-from-one-case-label
                        /*if (dateToCheck == defaultDate)
                        {
                            break;
                        }
                        state = ProgramState.getDataFromDb;*/

                    case ProgramState.getDataFromDb:
                        Console.WriteLine("sono dentro getDataFromDb");
                        int dbResult = GetDataFromDB(dateToCheck, out productedPieces, out defectedPieces, out plannedProductionTime, out productionTime);

                        // reversed logic:
                        //if (dbResult == 0)// 0: success, -1: error, -2: record not found
                        //{
                        //    state = ProgramState.oeeCalculation;
                        //}
                        //else /*if (dbResult == -2)*/ // don't mind why it failed
                        //{
                        //    state = ProgramState.checkingDate;
                        //}
                        //break;

                        // straight logic
                        // process flow continues on success or exit on fail
                        if (dbResult != 0)// 0: success, -1: error, -2: record not found
                        {
                            state = ProgramState.checkingDate;
                            break;
                        }
                        state = ProgramState.oeeCalculation;
                        break;

                    case ProgramState.oeeCalculation:
                        Console.WriteLine("sono dentro oeeCalculation");

                        availability = GetAvailability(productionTime, plannedProductionTime);
                        quality = GetQuality(productedPieces,defectedPieces);
                        performance = GetPerformances(productedPieces, productionTime, cycleTime);
                        state = ProgramState.writeOutPut;
                        break;
						
                    case ProgramState.writeOutPut:
                        Console.WriteLine("sono dentro writeOutPut");
                        PrintOEE(dateToCheck, availability, quality, performance);
 
                        state = ProgramState.checkingDate;
                        break;
                }
            }
        }

        static int GetDataFromDB(DateTime date, out int productedPieces, out int defectedPieces, out TimeSpan plannedProductionTime, out TimeSpan productionTime)
        {

            ConfigBuilder<AppConfig> _appsettings = new ConfigBuilder<AppConfig>(Utils.FileToProjectDirectory("..\\IIOT_OPCconfig.json"));
            AbstractDataAccess<PlantStateDuration> plantStateDurationDataAccess = new PlantStateDurationDataAccess() { ConnectionString = _appsettings.Config.ConnectionString };
            AbstractDataAccess<DailyProduction> dailyProductionDataAccess = new DailyProductionDataAccess() { ConnectionString = _appsettings.Config.ConnectionString };
            // retrieve items by filters
            PlantStateDuration stateDuration = null;
            DailyProduction productionObject;
            productedPieces = 0;
            defectedPieces = 0;
            plannedProductionTime = new TimeSpan(0);
            productionTime = new TimeSpan(0);

            #region try get duration
            try
            {
                stateDuration = plantStateDurationDataAccess.GetItem(
                    new
                    {
                        TimeStamp = date.AddHours(23).AddMinutes(59).AddSeconds(59)
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore in Program.TryGetDuration() -> {ex}");
                return -1;
            }
            #endregion

            #region try get production pieces
            try
            {
                productionObject = dailyProductionDataAccess.GetItem(
                    new
                    {
                        TimeStamp = date.AddHours(23).AddMinutes(59).AddSeconds(59)
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore in Program.TryGetDuration() -> {ex}");
                return -1;
            }
            #endregion

            if (stateDuration == null || productionObject == null)
            {
                Console.WriteLine("runningDuration o onStoppedDuration not found");
                Console.WriteLine("introduce a new date to check");
                return -2;
            }
            plannedProductionTime = stateDuration.OnRunningDuration + stateDuration.OnStoppedfDuration;
            productionTime = stateDuration.OnRunningDuration;
            productedPieces = productionObject.NumPieces;
            defectedPieces = productionObject.NumPiecesRejected;

            //Console.WriteLine("db get result:");
            //Console.WriteLine($"    OnRunningDuration {string.Format("{0:t}", stateDuration.OnRunningDuration)}");
            //Console.WriteLine($"    OnStoppedfDuration {string.Format("{0:t}", stateDuration.OnStoppedfDuration)}");
            //Console.WriteLine($"    plannedProductionTime {string.Format("{0:t}", plannedProductionTime)}");
            //Console.WriteLine($"    productionTime {string.Format("{0:t}", productionTime)}");
            //Console.WriteLine($"    productedPieces {productedPieces}");
            //Console.WriteLine($"    defectedPieces {defectedPieces}");

            return 0;
        }

        static DateTime CheckDateFromUser()
        {
            DateTime userDateTime = new DateTime();

            Console.WriteLine("Enter a date (ex. MM/dd/YYYY): ");
            if (DateTime.TryParse(Console.ReadLine(), out userDateTime))
            {
                Console.WriteLine("The date typed is: " + userDateTime);
                return userDateTime;
            }
            else
            {
                Console.WriteLine("You have entered an incorrect value.");
                Console.WriteLine("Try again...");
            }
            Console.ReadLine();
            Console.Clear();
            return userDateTime;
        }
        static double GetAvailability(TimeSpan operatingTime, TimeSpan scheduledTime)
        {
            return (operatingTime/scheduledTime);
        }
        static double GetQuality(int numPieces, int numDefectedPieces)
        {
            return (double)(numPieces-numDefectedPieces)/numPieces;
        }
        static double GetPerformances(int numPieces, TimeSpan operatingTime, TimeSpan cycleTime)
        {
            return (double)numPieces*cycleTime/operatingTime;
        }
        static void PrintOEE(DateTime targetDate, double availability, double quality, double performance)
        {
            string da = string.Format("\nThe OEE values in {0:d} are:", targetDate);

            string av = string.Format("  Availability:   {0:0.0}%", availability * 100);
            string qu = string.Format("  Quality:        {0:0.0}%", quality * 100);
            string pe = string.Format("  Performance:    {0:0.0}%\n", performance * 100);

            Console.WriteLine(da); //Data

            Console.WriteLine(av); //Availability
            Console.WriteLine(qu); //Quality
            Console.WriteLine(pe); //Performance
        }

        public enum ProgramState
        {
            checkingDate = 0x10,
            getDataFromDb = 0x20,
            oeeCalculation = 0x30,
            writeOutPut = 0x40
        }
    }
}