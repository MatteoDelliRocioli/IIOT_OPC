namespace IIOT_OPC.DataAccess
{
    using IIOT_OPC.Shared.Extensions;
    using IIOT_OPC.Shared.Models;
    public class DailyProductionDataAccess : AbstractDataAccess<DailyProduction>
    {
        public override int Delete(DailyProduction item)
        {
            throw new System.NotImplementedException();
        }

        public override DailyProduction GetItem(object filter)
        {
            string query = @"
                SELECT Id
                        ,TimeStamp
                        ,NumPieces
                        ,NumPiecesRejected
                    FROM dbo.DailyProduction
                {filter}";
            return GetItem(query.ApplyFilters("{filter}", filter), filter);
        }

        public override int Insert(DailyProduction item)
        {
            string query = @"
                INSERT INTO dbo.DailyProduction
                           (TimeStamp
                           ,NumPieces
                           ,NumPiecesRejected)
                     VALUES
                           (@TimeStamp
                           ,@NumPieces
                           ,@NumPiecesRejected);
                SELECT @@Identity";
            return Insert(query, item);
        }

        public override int Update(DailyProduction item)
        {
            throw new System.NotImplementedException();
        }
    }
}
