using System.Data.SqlClient;

namespace JanLehner_Backend_Prototyp_1706.Models
{
    public class Database
    {
        public static readonly string DATABASE_NAME = @"ParkGuardDB";
        public static readonly string SERVER_NAME = "DESKTOP-DKL8S3C";
        public static readonly string MASTER_CONNECTION_STRING = $@"Server={SERVER_NAME}\SQLEXPRESS; Integrated Security=True;";
        public static readonly string CONNECTION_STRING = $@"Server={SERVER_NAME}\SQLEXPRESS; Database={DATABASE_NAME}; Integrated Security = True;";

        public static void Initialize()
        {
            CreateDatabaseIfNotExists();
            CreateTablesIfNotExists();
        }

        private static void CreateDatabaseIfNotExists()
        {
            string createDbQuery = $@"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{DATABASE_NAME}') CREATE DATABASE {DATABASE_NAME}";
            using (var masterConnection = new SqlConnection(MASTER_CONNECTION_STRING))
            {
                var command = new SqlCommand(createDbQuery, masterConnection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static void CreateTablesIfNotExists()
        {
            const string createCarsDbQuery = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Cars' and xtype='U')
                BEGIN
                    CREATE TABLE Cars (
                        carID INT PRIMARY KEY IDENTITY (1, 1),
                        numberPlate VARCHAR(10) NOT NULL,
                        isParked BIT NOT NULL,
                        paidUntil DATETIME NOT NULL
                    )
                END";
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                var command = new SqlCommand(createCarsDbQuery, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
