using SQLiteDemo.MVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace SQLiteDemo.DAO
{
    internal class ClassDB
    {
        private static DatabaseConnection dtc = new DatabaseConnection();

        public ObservableCollection<Class> GetAllClass()
        {
            ObservableCollection<Class> temp = new ObservableCollection<Class>();
            try
            {
                dtc.createConection();

                string querry = "SELECT * FROM Class";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string sclass = reader.GetString(0);
                        string fac = reader.GetString(1);
                        Faculty sfaculty = new Faculty(fac);
                        Class item = new Class(sclass, sfaculty);
                        temp.Add(item);
                    }
                }
                dtc.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Get_All_Class :" + ex.Message);
            }
            return temp;
        }

        public ObservableCollection<Class> SearchClass (string searchText)
        {
            ObservableCollection< Class> temp = new ObservableCollection<Class>();
            if (searchText != null)
            {
                try
                {
                    dtc.createConection();

                    string querry = "SELECT * FROM Class WHERE SClass LIKE @searchText OR Faculty LIKE @searchText";
                    SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                    cmd.CommandText = querry;
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string sclass = reader.GetString(0);
                            string fac = reader.GetString(1);
                            Faculty sfaculty = new Faculty(fac);
                            Class item = new Class(sclass,sfaculty);
                            temp.Add(item);
                        }
                    }
                    dtc.closeConnection();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error_Search_Class :" + ex.Message);
                }
                return temp;
            }
            else
            {
                return GetAllClass();
            }
        }

        public bool CheckExist(Class item)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "SELECT * FROM Class WHERE SClass = @sclass AND Faculty = @sfaculty";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@sclass", item.SClass);
                cmd.Parameters.AddWithValue("@faculty", item.SFaculty.Fac);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string temp = reader.GetString(0);
                        rs = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Check_Exist :" + ex.Message);
            }
            finally { dtc.closeConnection(); }
            return rs;
        }

        public bool CreateClass(Class item)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "INSERT INTO Class VALUES ( @sclass , @sfaculty )";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@sclass", item.SClass);
                cmd.Parameters.AddWithValue("@sfaculty", item.SFaculty.Fac);
                cmd.ExecuteNonQuery();
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Create_Class :" + ex.Message);
            }
            finally { dtc.closeConnection(); }
            return rs;
        }

        public bool DeleteClass(Class item)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "DELETE FROM Class WHERE SClass = @sclass";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@sclass", item.SClass);
                cmd.ExecuteNonQuery();
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Delete_Class :" + ex.Message);
            }
            finally { dtc.closeConnection(); }
            return rs;
        }

        public int GetNoStudent(string SClass)
        {
            int rs = 0;
            try
            {
                dtc.createConection();

                string querry = "SELECT COUNT(*) FROM Student WHERE Class = @sclass";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@sclass", SClass);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rs = reader.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_GetNoStudent :" + ex.Message);
            }
            finally { dtc.closeConnection(); }
            return rs;
        }
    }
}
