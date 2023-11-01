using System;
using System.Data.SQLite;

namespace SQLiteDemo.DAO
{
    internal class DatabaseConnection
    {
        public SQLiteConnection _con = new SQLiteConnection();
        public void createConection()
        {
            string _strConnect = @"Data Source=D:\[Study]\.NET\NET_Programming\SQLiteDemo\SQLiteDemo\SQLiteDemoDB.db;Version=3;";
            _con.ConnectionString = _strConnect;
            try
            {
                _con.Open();
                Console.WriteLine("Database connection successful.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error_Open: " + ex.Message);
            }
        }

        public void closeConnection()
        {
            try
            {
                _con.Close();
                Console.WriteLine("Close Connection successfull.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error_close: " + ex.Message);
            }
        }
    }
}
