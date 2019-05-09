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

            while (true)
            {
                switch (state)
                {
                    case ProgramState.checkingDate:
                        dateToCheck = checkDateFromUser();
                        if (dateToCheck != defaultDate)
                        {
                            state = ProgramState.oeeCalculation;
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
                        Console.WriteLine("sono dentro writeOutput");
                        Console.ReadLine();
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

        public enum ProgramState
        {
            checkingDate = 0x10,
            getDateFromDb = 0x20,
            oeeCalculation = 0x30,
            writeOutPut = 0x40
        }
    }
}