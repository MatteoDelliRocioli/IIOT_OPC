namespace IOT_OPC.DataHandler
{
    using System;

    public class PlantStateDuration
    {
        public TimeSpan OffDuration { get; set; }
        public TimeSpan OnRunningDuration { get; set; }
        public TimeSpan OnStoppedfDuration { get; set; }
    }
}