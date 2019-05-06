namespace IIOT_OPC.Shared.Models
{
    using System;
    public class PlantStateRowData : DbObject
    {
        public DateTime TimeStamp { get; set; }
        public PlantState PlantState { get; set; }
    }
    public enum PlantState : int 
    {                     // plc internal bit state       On/Off  Running/stopped
        Off = 0x0,        // plant is Off                   0         0
        OnRunning = 0x3,  // plant is On  and Running       1         1
        OnStopped = 0x1   // Plant is On  and Stopped       1         0
                          // OffRunning = 0x2  // Plant is Off BUT Running       0         1     Invalid state
    }

}
