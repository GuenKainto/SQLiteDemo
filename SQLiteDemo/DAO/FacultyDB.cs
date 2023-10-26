using SQLiteDemo.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLiteDemo.DAO
{
    internal class FacultyDB // using query
    {
        private static DatabaseConnection dtc = new DatabaseConnection();

        public ObservableCollection<Faculty> GetAllFac()
        {
            ObservableCollection<Faculty> temp = new ObservableCollection<Faculty>();
            try
            {
                dtc.createConection();

                string querry = "SELECT * FROM Faculty";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
               
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string fac = reader.GetString(0);
                        Faculty item = new Faculty(fac);
                        temp.Add(item);
                    }
                }
                dtc.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Get_All_Faculty :" + ex.Message);
            }
            return temp;
        } 

        public bool CheckExist(string fac)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "SELECT * FROM Faculty WHERE Fac = @fac";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@fac", fac);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string temp = reader.GetString(0);
                        rs = true;
                    }
                }
                dtc.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Check_Exist :" + ex.Message);
            }
            return rs;
        }

        public bool CreateFaculty(string fac)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "INSERT INTO Faculty VALUES ( @fac )";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@fac",fac);
                cmd.ExecuteNonQuery();
                dtc.closeConnection();
                rs = true;
                dtc.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Create_Faculty :" + ex.Message);
            }
            return rs;
        }

        public bool DeleteFaculty(string fac)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "DELETE FROM Faculty WHERE Fac = @fac";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@fac", fac);
                cmd.ExecuteNonQuery();
                dtc.closeConnection();
                rs= true;
                dtc.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Delete_Faculty :" + ex.Message);
            }
            return rs;
        }

        public int GetNoClass(string fac)
        {
            int rs = 0;
            try
            {
                dtc.createConection();

                string querry = "SELECT COUNT(*) FROM Class WHERE Faculty = @fac";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@fac", fac);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rs = reader.GetInt32(0);
                    }
                }
                dtc.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_GetNoClass :" + ex.Message);
            }
            return rs;
        }

        public int GetNoTeacher(string fac)
        {
            int rs = 0;
            try
            {
                dtc.createConection();

                string querry = "SELECT COUNT(*) FROM Teacher WHERE Faculty = @fac";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@fac", fac);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rs = reader.GetInt32(0);
                    }
                }
                dtc.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_GetNoTeacher :" + ex.Message);
            }
            return rs;
        }

        public int GetNoStudent(string fac)
        {
            int rs = 0;
            try
            {
                dtc.createConection();

                string querry = "SELECT COUNT(*) " +
                                "FROM Student JOIN Class ON Student.SClass = Class.SClass " +
                                "JOIN Faculty ON Faculty.Fac = Class.Faculty " +
                                "WHERE Class.Faculty = @fac";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@fac", fac);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rs = reader.GetInt32(0);
                    }
                }
                dtc.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_GetNoStudent :" + ex.Message);
            }
            return rs;
        }
    }
}
