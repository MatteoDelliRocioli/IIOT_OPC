namespace IIOT_OPC.DataAccess.Test
{
    using IIOT_OPC.Shared.Configuration;
    using IIOT_OPC.Shared.Extensions;
    using IIOT_OPC.Shared.Models;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            ConfigBuilder<AppConfig> _appsettings = new ConfigBuilder<AppConfig>(Utils.FileToProjectDirectory("..\\IIOT_OPCconfig.json"));
            //AbstractDataAccess<DailyProduction> dailyProductionDataAccess = new DailyProductionDataAccess() { ConnectionString = _appsettings.Config.ConnectionString };

            //// insert new items
            //dailyProductionDataAccess.Insert(
            //    new DailyProduction
            //            {
            //                TimeStamp = new DateTime(1999,2,3)
            //                , NumPieces = 12
            //                , NumPiecesRejected = 0
            //            }
            //    );
            //dailyProductionDataAccess.Insert(new DailyProduction { TimeStamp = new DateTime(1999,3,3), NumPieces = 24, NumPiecesRejected = 0 });
            //dailyProductionDataAccess.Insert(new DailyProduction { TimeStamp = new DateTime(1999, 3,4), NumPieces = 35, NumPiecesRejected = 5 });

            //// retrieve items by filters
            //var itemByDate = dailyProductionDataAccess.GetItem(
            //    new
            //            {
            //                TimeStamp = new DateTime(1999, 3, 3)
            //            }
            //    );
            //var itemById = dailyProductionDataAccess.GetItem(new { Id = 2 });
            //var itemByPieces = dailyProductionDataAccess.GetItem(new { NumPieces = 2 });

            //AbstractDataAccess<PlantStateRowData> plantStatesDataAccess = new PlantStatesDataAccess() { ConnectionString = _appsettings.Config.ConnectionString };

            //plantStatesDataAccess.Insert(
            //    new PlantStateRowData
            //    {
            //                TimeStamp = new DateTime(1999, 2, 3, 6, 36, 11)
            //                , PlantState = (PlantState)1
            //            }
            //    );
            //plantStatesDataAccess.Insert(new PlantStateRowData { TimeStamp = new DateTime(1999, 2, 3, 7, 00, 45), PlantState = (PlantState)2 });
            //plantStatesDataAccess.Insert(new PlantStateRowData { TimeStamp = new DateTime(1999, 2, 3, 12, 24, 13), PlantState = (PlantState)3 });
            //plantStatesDataAccess.Insert(new PlantStateRowData { TimeStamp = new DateTime(1999, 2, 3, 12, 25, 54), PlantState = (PlantState)2 });
            //plantStatesDataAccess.Insert(new PlantStateRowData { TimeStamp = new DateTime(1999, 2, 3, 18, 16, 9), PlantState = (PlantState)1 });

            //var stateByDate = plantStatesDataAccess.GetItem(new { TimeStamp = new DateTime(1999, 2, 3, 7, 00, 45) });



            AbstractDataAccess<PlantStateDuration> plantStateDurationDataAccess = new PlantStateDurationDataAccess() { ConnectionString = _appsettings.Config.ConnectionString };

            // insert new items
            plantStateDurationDataAccess.Insert(
                new PlantStateDuration
                {
                    TimeStamp = DateTime.Now,
                    OffDuration = new TimeSpan(8,0,0),
                    OnRunningDuration = new TimeSpan(15, 30, 0),
                    OnStoppedfDuration = new TimeSpan(0, 30, 0)
                }
                );

            // retrieve items by filters
            var itemByDate = plantStateDurationDataAccess.GetItem(
                new
                {
                    TimeStamp = new DateTime(1999, 3, 3)
                }
                );
            var itemById = plantStateDurationDataAccess.GetItem(new { Id = 2 });
            var itemByPieces = plantStateDurationDataAccess.GetItem(new { OnRunningDuration = new TimeSpan(15, 30, 0) });






        }
    }
}
