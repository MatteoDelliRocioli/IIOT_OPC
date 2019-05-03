namespace IIOT_OPC.Shared.Models
{
    using System;
    public class PlantStates : DbObject
    {
        public DateTime TimeStamp { get; set; }
        public int PlantState { get; set; }
    }
}
