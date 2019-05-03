namespace IIOT_OPC.DataAccess
{
    using IIOT_OPC.Shared.Extensions;
    using IIOT_OPC.Shared.Models;
    public class PlantStatesDataAccess : AbstractDataAccess<PlantStates>
    {
        public override int Delete(PlantStates item)
        {
            throw new System.NotImplementedException();
        }

        public override PlantStates GetItem(object filter)
        {
            string query = @"
                SELECT Id
                        ,TimeStamp
                        ,PlantState
                    FROM dbo.PlantStates
                {filter}";
            return GetItem(query.ApplyFilters("{filter}", filter), filter);
        }

        public override int Insert(PlantStates item)
        {
            string query = @"
                INSERT INTO dbo.PlantStates
                           (TimeStamp
                           ,PlantState)
                     VALUES
                           (@TimeStamp
                           ,@PlantState);";
            return Insert(query, item);
        }

        public override int Update(PlantStates item)
        {
            throw new System.NotImplementedException();
        }
    }
}
