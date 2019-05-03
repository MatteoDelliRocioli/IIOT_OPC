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
            AbstractDataAccess<DailyProduction> dailyProductiondataAccess = new DailyProductionDataAccess() { ConnectionString = _appsettings.Config.ConnectionString };

            dailyProductiondataAccess.Insert(new DailyProduction { TimeStamp = new DateTime(1999,2,3), NumPieces = 12, NumPiecesRejected = 0 });
            dailyProductiondataAccess.Insert(new DailyProduction { TimeStamp = new DateTime(1999,3,3), NumPieces = 24, NumPiecesRejected = 0 });
            dailyProductiondataAccess.Insert(new DailyProduction { TimeStamp = new DateTime(1999, 3,4), NumPieces = 35, NumPiecesRejected = 5 });
        }
    }
}
