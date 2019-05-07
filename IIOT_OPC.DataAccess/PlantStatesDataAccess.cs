namespace IIOT_OPC.DataAccess
{
    using IIOT_OPC.Shared.Extensions;
    using IIOT_OPC.Shared.Models;
    public class PlantStatesDataAccess : AbstractDataAccess<PlantStateRowData>
    {
        public override int Delete(PlantStateRowData item)
        {
            throw new System.NotImplementedException();
        }

        public override PlantStateRowData GetItem(object filter)
        {
            string query = @"
                SELECT Id
                        ,TimeStamp
                        ,PlantState
                    FROM dbo.PlantStates
                {filter}";
            return GetItem(query.ApplyFilters("{filter}", filter), filter);
        }

        public override int Insert(PlantStateRowData item)
        {
            string query = @"
                INSERT INTO dbo.PlantStates
                           (TimeStamp
                           ,PlantState)
                     VALUES
                           (@TimeStamp
                           ,@PlantState);
                SELECT @@Identity";
            return Insert(query, item);
        }

        public override int Update(PlantStateRowData item)
        {
            throw new System.NotImplementedException();
        }
    }
}
