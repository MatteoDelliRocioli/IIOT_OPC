namespace IIOT_OPC.DataAccess
{
    using Dapper;
    using IIOT_OPC.Shared.Interfaces;
    using IIOT_OPC.Shared.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Transactions;

    public abstract class AbstractDataAccess<T> : IDataConnect, IDataAccess<T> where T : DbObject, new()
    {
        public AbstractDataAccess()
        {
        }

        public AbstractDataAccess(string config)
        {
            ConnectionString = config;
        }

        public string ConnectionString { get; set; }

        public abstract int Delete(T item);
        public abstract T GetItem(object filter);
        public abstract int Insert(T item);
        public abstract int Update(T item);

        /// <returns>The number of rows affected</returns>
        protected int Delete(string query, T item)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var result = connection.Execute(query, item);
                return result;
            }
        }

        protected T GetItem(string query, object filter = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var result = connection.QueryFirstOrDefault<T>(
                                query,
                                filter);
                return result;
            }
        }

        //protected List<T> GetList(string query, object filters = null)
        //{
        //    using (var connection = new SqlConnection(ConnectionString))
        //    {
        //        var result = connection.Query<T>(query, filters);
        //        return result.AsList();
        //    }
        //}

        /// <summary>
        /// insrt a new record into the db and assigns the new id to the item passed
        /// </summary>
        /// <param name="query"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected int Insert(string query, T item)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var result = connection.ExecuteScalar<int>(query, item);
                item.Id = result;
                return result;
            }
        }
        /// <returns>The number of rows affected</returns>
        protected int Update(string query, T item)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var result = connection.Execute(query, item);
                return result;
            }
        }
    }
}
