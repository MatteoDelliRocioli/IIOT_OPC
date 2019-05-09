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
            DateTime dateToCheck;
            DateTime defaultDate = new DateTime();
            dateToCheck = defaultDate;
            double availability = 0.573;
            double quality = 1;
            double performance = 0.12;

            while (true)
            {
                switch (state)
                {
                    case ProgramState.checkingDate:
                        dateToCheck = checkDateFromUser();
                        if (dateToCheck != defaultDate)
                        {
                            state = ProgramState.writeOutPut;
                        }
                        break;
                    case ProgramState.getDateFromDb:
                        Console.WriteLine("sono dentro date");
                        Console.ReadLine();
                        break;
                    case ProgramState.oeeCalculation:
                        Console.WriteLine("sono dentro calculation");
                        Console.ReadLine();
                        break;
                    case ProgramState.writeOutPut:
                        PrintOEE(dateToCheck, availability, quality, performance);
 
                        state = ProgramState.checkingDate;
                        break;
                    default:
                        break;
                }
            }
        }

        static DateTime checkDateFromUser()
        {
            DateTime userDateTime = new DateTime();

            Console.WriteLine("Enter a date (ex. dd/MM/YYYY): ");
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

        static void PrintOEE(DateTime pippo, double availability, double quality, double performance)
        {
            string da = String.Format("\n The OEE values in {0:d} are:", pippo);

            string av = String.Format("  Availability: {0:0.0}%", availability);
            string qu = String.Format("  Quality: {0:0.0}%", quality);
            string pe = String.Format("  Performance: {0:0.0}%\n", performance);

            Console.WriteLine(da); //Data

            Console.WriteLine(av); //Availability
            Console.WriteLine(qu); //Quality
            Console.WriteLine(pe); //Performance
        }

        public enum ProgramState
        {
            checkingDate = 0x10,
            getDateFromDb = 0x20,
            oeeCalculation = 0x30,
            writeOutPut = 0x40
        }
    }
}