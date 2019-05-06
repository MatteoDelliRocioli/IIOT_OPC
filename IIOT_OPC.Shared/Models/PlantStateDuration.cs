namespace IIOT_OPC.Shared.Models
{
    using System;

    public class PlantStateDuration
    {
        public TimeSpan OffDuration { get; set; }
        public TimeSpan OnRunningDuration { get; set; }
        public TimeSpan OnStoppedfDuration { get; set; }
    }
}