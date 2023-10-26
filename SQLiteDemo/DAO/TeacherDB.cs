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
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string TID = reader.GetString(0);
                        MessageBox.Show("Test0" + TID);
                        string TName = reader.GetString(1);
                        MessageBox.Show("Test1" + TName);
                        string fac = reader.GetString(2);
                        Faculty TFaculty = new Faculty(fac);
                        MessageBox.Show("Test2" + TFaculty.Fac);
                        string TDOB = reader.GetString(3);
                        MessageBox.Show("Test3" + TDOB);
                        string TAddress = reader.GetString(4);
                        MessageBox.Show("Test4" + TAddress);
                        string TPhone = reader.GetString(5);
                        MessageBox.Show("Test5" + TPhone);
                        temp.Add(new Teacher(TID,TName,TFaculty,TDOB,TAddress,TPhone));
                        MessageBox.Show(temp.Count.ToString()+"ABC");
                    }
                }
                dtc.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Get_All_Teacher :" + ex.Message);
            }
            //MessageBox.Show(temp.Count.ToString());
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
                    cmd.Parameters.AddWithValue("@searchText", searchText);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Teacher item = new Teacher();
                            item.TID = reader.GetString(0);
                            item.TName = reader.GetString(1);
                            string fac = reader.GetString(2);
                            item.TFaculty = new Faculty(fac);
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

        public Teacher GetTeacher(string tID)
        {
            Teacher teacher = null;
            try
            {
                dtc.createConection();

                string querry = "SELECT * FROM Teacher WHERE TID = @tID";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@tid", tID);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Teacher item = new Teacher();
                        item.TID = reader.GetString(0);
                        item.TName = reader.GetString(1);
                        string fac = reader.GetString(2);
                        item.TFaculty = new Faculty(fac);
                        item.TDOB = reader.GetString(3);
                        item.TAddress = reader.GetString(4);
                        item.TPhone = reader.GetString(5);
                        teacher = item;
                    }
                }
                dtc.closeConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Search_Teacher :" + ex.Message);
            }
            return teacher;
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
                dtc.closeConnection();
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

                string querry = "INSERT INTO Teacher VALUES ( @tid , @tname, @faculty , @tdob , @taddress , @tphone )";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@tid", item.TID);
                cmd.Parameters.AddWithValue("@tname", item.TName);
                cmd.Parameters.AddWithValue("@faculty", item.TFaculty.Fac);
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

        public bool UpdateTeacher(Teacher teacher)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "UPDATE Teacher SET TName = @tname , TFaculty = @faculty , TDOB = @tdob , TAddress = @taddress , TPhone = @tphone  WHERE TID = @tid ";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@tid", teacher.TID);
                cmd.Parameters.AddWithValue("@tname", teacher.TName);
                cmd.Parameters.AddWithValue("@faculty", teacher.TFaculty.Fac);
                cmd.Parameters.AddWithValue("@tdob", teacher.TDOB);
                cmd.Parameters.AddWithValue("@taddress", teacher.TAddress);
                cmd.Parameters.AddWithValue("@tphone", teacher.TPhone);
                cmd.ExecuteNonQuery();
                dtc.closeConnection();
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Update_Teacher :" + ex.Message);
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
