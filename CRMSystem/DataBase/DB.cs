using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

namespace CRMSystem.DataBase
{
    public static class DB
    {
        private static string ConnectionString { get; }



        public static readonly CRMSystemEntities Context;



        static DB()
        {
            ConnectionString = new EntityConnectionStringBuilder
            {
                Metadata = "res://*",
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = new SqlConnectionStringBuilder
                {
                    DataSource = "tcp:127.0.0.1,1433",
                    InitialCatalog = "CRMSystem",
                    IntegratedSecurity = false,
                    UserID = "Krem",
                    Password = "VjqLbgkjv",
                    MultipleActiveResultSets = true,
                    ApplicationName = "EntityFramework"
                }.ConnectionString
            }.ConnectionString;

            Context = new CRMSystemEntities(
                ConnectionString);
        }
    }
}
