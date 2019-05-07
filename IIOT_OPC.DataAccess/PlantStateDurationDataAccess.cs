namespace IIOT_OPC.DataAccess
{
    using IIOT_OPC.Shared.Extensions;
    using IIOT_OPC.Shared.Models;
    public class PlantStateDurationDataAccess : AbstractDataAccess<PlantStateDuration>
    {
        public override int Delete(PlantStateDuration item)
        {
            throw new System.NotImplementedException();
        }

        public override PlantStateDuration GetItem(object filter)
        {
            string query = @"
                SELECT Id
                      ,TimeStamp
                      ,OffDuration
                      ,OnRunningDuration
                      ,OnStoppedfDuration
                  FROM dbo.PlantStateDuration
                {filter}";
            return GetItem(query.ApplyFilters("{filter}", filter), filter);
        }

        public override int Insert(PlantStateDuration item)
        {
            string query = @"
                INSERT INTO dbo.PlantStateDuration
                           (TimeStamp
                           ,OffDuration
                           ,OnRunningDuration
                           ,OnStoppedfDuration)
                     VALUES
                           (@TimeStamp
                           ,@OffDuration
                           ,@OnRunningDuration
                           ,@OnStoppedfDuration);
                SELECT @@Identity";
            return Insert(query, item);
        }

        public override int Update(PlantStateDuration item)
        {
            throw new System.NotImplementedException();
        }
    }
}
