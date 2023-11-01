using System;
using System.Data.SQLite;

namespace SQLiteDemo.DAO
{
    internal class LoginDB // using query
    {
        private static DatabaseConnection dtc = new DatabaseConnection();
        public bool CheckLogin(string userName, string passWord)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "SELECT * FROM Account WHERE UserName = @userName AND Password = @passWord";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Parameters.AddWithValue("@passWord", passWord);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string username = reader.GetString(0);
                        string password = reader.GetString(1);
                        rs = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_login :" + ex.Message);
            }
            finally { dtc.closeConnection(); }

            return rs;
        }

        //update Validate input 
    }
}
