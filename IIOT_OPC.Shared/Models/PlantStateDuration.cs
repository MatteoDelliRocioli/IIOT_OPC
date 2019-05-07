namespace IIOT_OPC.Shared.Models
{
    using System;

    public class PlantStateDuration : DbObject
    {
        public DateTime TimeStamp { get; set; }
        // https://stackoverflow.com/questions/8503825/what-is-the-correct-sql-type-to-store-a-net-timespan-with-values-240000
        public TimeSpan OffDuration { get; set; }
        public TimeSpan OnRunningDuration { get; set; }
        public TimeSpan OnStoppedfDuration { get; set; }
    }
}