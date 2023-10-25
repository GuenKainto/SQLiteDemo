using SQLiteDemo.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDemo.DAO
{
    internal class TeacherDB
    {
        private static DatabaseConnection dtc = new DatabaseConnection();

        public ObservableCollection<Teacher> GetAllTeacher()
        {
            ObservableCollection<Teacher> temp = new ObservableCollection<Teacher>();

            try
            {
                dtc.createConection();

                string querry = "SELECT * FROM Teacher";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Teacher item = new Teacher();
                        item.TID = reader.GetString(0);
                        item.TName = reader.GetString(1);
                        item.TFaculty = reader.GetString(2);
                        item.TDOB = reader.GetString(3);
                        item.TAddress = reader.GetString(4);
                        item.TPhone = reader.GetString(5);
                        temp.Add(item);
                    }
                }
                dtc.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Get_All_Teacher :" + ex.Message);
            }
            return temp;
        }

        public ObservableCollection<Teacher> SearchTeacher(string searchText)
        {
            ObservableCollection<Teacher> temp = new ObservableCollection<Teacher>();
            if (searchText != null)
            {
                try
                {
                    dtc.createConection();

                    string querry = "SELECT * FROM Teacher WHERE TID = @searchText OR TName LIKE @searchText OR Faculty LIKE @searchText ";
                    SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                    cmd.CommandText = querry;
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Teacher item = new Teacher();
                            item.TID = reader.GetString(0);
                            item.TName = reader.GetString(1);
                            item.TFaculty = reader.GetString(2);
                            item.TDOB = reader.GetString(3);
                            item.TAddress = reader.GetString(4);
                            item.TPhone = reader.GetString(5);
                            temp.Add(item);
                        }
                    }
                    dtc.closeConnection();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error_Search_Teacher :" + ex.Message);
                }
                return temp;
            }
            else
            {
                return GetAllTeacher();
            }
        }

        public bool CheckExist(string tid)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "SELECT * FROM Teacher WHERE TID = @tid";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@tid", tid);

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
            return rs;
        }

        public bool CreateTeacher(Teacher item)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "INSERT INTO Faculty VALUES ( @tid , @tname, @faculty , @tdob , @taddress , @tphone )";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@tid", item.TID);
                cmd.Parameters.AddWithValue("@tname", item.TName);
                cmd.Parameters.AddWithValue("@faculty", item.TFaculty);
                cmd.Parameters.AddWithValue("@tdob", item.TDOB);
                cmd.Parameters.AddWithValue("@taddress", item.TAddress);
                cmd.Parameters.AddWithValue("@tphone", item.TPhone);
                cmd.ExecuteNonQuery();
                dtc.closeConnection();
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Create_Teacher :" + ex.Message);
            }
            return rs;
        }

        public bool DeleteTeacher(Teacher item)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "DELETE FROM Teacher WHERE TID = @tid";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@tid", item.TID);
                cmd.ExecuteNonQuery();
                dtc.closeConnection();
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Delete_Teacher :" + ex.Message);
            }
            return rs;
        }

    }
}
