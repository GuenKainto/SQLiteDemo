using SQLiteDemo.MVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;

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

        public Class GetClass(string sClass)
        {
            Class rs = new Class();
            try
            {
                if (dtc._con.State == System.Data.ConnectionState.Closed)
                {
                    dtc.createConection();
                }

                string querry = "SELECT * FROM Class WHERE SClass = @sClass";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@sClass", sClass);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        /*string sclass = reader.GetString(0);
                        string fac = reader.GetString(1);
                        Faculty sfaculty = new Faculty(fac);
                        rs = new Class(sclass, sfaculty);*/

                        rs.SClass = reader.GetString(0);
                        string fac = reader.GetString(1);
                        rs.SFaculty = new Faculty(fac);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Search_Class :" + ex.Message);
            }
            finally { dtc.closeConnection(); }
            return rs;
        }

        public ObservableCollection<Class> SearchClass (string sClass, Faculty sFaculty)
        {
            ObservableCollection< Class> temp = new ObservableCollection<Class>();

            try
            {
                string tempFac;
                dtc.createConection();

                if (sFaculty == null)
                {
                    tempFac = "";
                }
                else tempFac = sFaculty.Fac;

                string querry = "SELECT * FROM Class WHERE SClass LIKE @sClass AND Faculty LIKE @sFaculty";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@sClass", "%" + sClass + "%");
                cmd.Parameters.AddWithValue("@sFaculty", "%" + tempFac + "%");
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
                Console.WriteLine("Error_Search_Class :" + ex.Message);
            }
            return temp;
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

                string querry = "SELECT COUNT(*) FROM Student WHERE SClass = @sclass";
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
