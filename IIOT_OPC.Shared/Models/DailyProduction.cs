namespace IIOT_OPC.Shared.Models
{
    using System;
    public class DailyProduction : DbObject
    {
        public DateTime TimeStamp { get; set; }
        public int NumPieces { get; set; }
        public int NumPiecesRejected { get; set; }
    }
}
