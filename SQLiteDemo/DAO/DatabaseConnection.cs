using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace SQLiteDemo.DAO
{
    internal class DatabaseConnection
    {
        public SQLiteConnection _con = new SQLiteConnection();
        public void createConection()
        {
            string projectFolderPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string filePath = Path.Combine(projectFolderPath, "SQLiteDemoDB.db");
            string _strConnect = $"Data Source={filePath};Version=3;";
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
