﻿using IIOT_OPC.DataAccess;
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
            DateTime dateToCheck = new DateTime();
            DateTime defaultDate = new DateTime();

            while (true)
            {
                switch (state)
                {
                    case ProgramState.checkingDate:
                        dateToCheck = CheckDateFromUser();
                        if (dateToCheck != defaultDate)
                        {
                            state = ProgramState.getDataFromDb;
                        }
                        break;
                    case ProgramState.getDataFromDb:

                        Console.WriteLine("sono dentro getDataFromDb");
                        int dbResult = GetDataFromDB(dateToCheck, out int productedPieces, out int defectedPieces, out TimeSpan plannedProductionTime);
                        if (dbResult == 0)
                        {
                            state = ProgramState.oeeCalculation;
                        }
                        else if (dbResult == -2)
                        {
                            state = ProgramState.checkingDate;
                        }
                        Console.ReadLine();
                        break;
                    case ProgramState.oeeCalculation:
                        Console.WriteLine("sono dentro oeeCalculation");
                        Console.ReadLine();
                        break;
                    case ProgramState.writeOutPut:
                        Console.WriteLine("sono dentro writeOutput");
                        Console.ReadLine();
                        break;
                    default:
                        break;
                }
            }
        }

        static int GetDataFromDB(DateTime date, out int productedPieces, out int defectedPieces, out TimeSpan plannedProductionTime)
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

            #region try get duration
            try
            {
                stateDuration = plantStateDurationDataAccess.GetItem(
                    new
                    {
                        TimeStamp = date
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
                        TimeStamp = date
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore in Program.TryGetDuration() -> {ex}");
                return -1;
            }
            #endregion

            if (stateDuration == null)
            {
                Console.WriteLine("runningDuration o onStoppedDuration not founded");
                Console.WriteLine("introduce a new date to check");
                return -2;
            }

            plannedProductionTime = stateDuration.OnRunningDuration + stateDuration.OnStoppedfDuration;
            productedPieces = productionObject.NumPieces;
            defectedPieces = productionObject.NumPiecesRejected;
            /*if (productedPieces == null || defectedPieces == null || plannedProductionTime == default(TimeSpan))
            {
                Console.WriteLine("Uno dei valori tra pezzi prodotti, difettati o tempo di produzione non sono stati valorizzati da db");
                Console.ReadLine();
                return -1;
            }*/
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

        public enum ProgramState
        {
            checkingDate = 0x10,
            getDataFromDb = 0x20,
            oeeCalculation = 0x30,
            writeOutPut = 0x40
        }
    }
}