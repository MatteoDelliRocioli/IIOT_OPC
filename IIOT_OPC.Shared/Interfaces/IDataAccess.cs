namespace IIOT_OPC.Shared.Interfaces
{
    public interface IDataAccess<TItem> 
    {
        int Insert(TItem item);

        TItem GetItem(object filter);

        /// <returns>The number of rows affected</returns>
        int Update(TItem item);

        int Delete(TItem item);
    }
}