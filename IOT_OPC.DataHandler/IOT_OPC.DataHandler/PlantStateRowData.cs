using System;

namespace IOT_OPC.DataHandler
{
    public class PlantStateRowData
    {
        public PlantStateRowData()
        {
        }

        public PlantState CurrentState { get; set; }
        public DateTime StartTime { get; set; }
    }
}