using SQLiteDemo.MVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;

namespace SQLiteDemo.DAO
{
    internal class StudentDB
    {
        private static DatabaseConnection dtc = new DatabaseConnection();

        public ObservableCollection<Student> GetAllStudent()
        {
            ObservableCollection<Student> temp = new ObservableCollection<Student>();

            try
            {
                if(dtc._con.State == System.Data.ConnectionState.Closed)
                {
                    dtc.createConection();
                }
                string querry = "SELECT * FROM Student INNER JOIN Class ON Student.SClass = Class.SClass";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string SID = reader.GetString(0);
                        string SName = reader.GetString(1);
                        string SDOB = reader.GetString(3);
                        string SPhone = reader.GetString(4);
                        string SAddress = reader.GetString(5);
                        string cla = reader.GetString(6);
                        string fac = reader.GetString(7);
                        Faculty SFaculty = new Faculty(fac);
                        Class SClass = new Class(cla, SFaculty);
                        temp.Add(new Student(SID, SName, SClass, SDOB, SPhone, SAddress));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Get_All_Student :" + ex.Message);
            }
            finally {dtc.closeConnection(); }
            return temp;
        }

        
        public ObservableCollection<Student> SearchStudent(string searchText,Faculty searchFaculty , Class searchClass)
        {
            ObservableCollection<Student> temp = new ObservableCollection<Student>();
            if (searchText != null)
            {
                try
                {
                    dtc.createConection();

                    string tempFac;
                    string tempClass;

                    if (searchFaculty == null) tempFac = "";
                    else tempFac = searchFaculty.Fac;

                    if (searchClass == null) tempClass = "";
                    else tempClass = searchClass.SClass;


                    string querry = "SELECT * FROM Student " +
                                    "INNER JOIN Class ON Student.SClass = Class.SClass " +
                                    "WHERE SID LIKE @searchText " +
                                    "AND SName LIKE @searchText " +
                                    "AND Class.Faculty LIKE @searchFaculty " +
                                    "AND Student.SClass LIKE @searchClass ";
                    SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                    cmd.CommandText = querry;
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                    cmd.Parameters.AddWithValue("@searchClass", "%" + tempClass + "%");
                    cmd.Parameters.AddWithValue("@searchFaculty", "%" + tempFac + "%");

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string SID = reader.GetString(0);
                            string SName = reader.GetString(1);
                            string SDOB = reader.GetString(3);
                            string SPhone = reader.GetString(4);
                            string SAddress = reader.GetString(5);
                            string cla = reader.GetString(6);
                            string fac = reader.GetString(7);
                            Faculty SFaculty = new Faculty(fac);
                            Class SClass = new Class(cla, SFaculty);
                            temp.Add(new Student(SID, SName, SClass, SDOB, SPhone, SAddress));
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
                return GetAllStudent();
            }
        }

        public Student GetStudent(string sID)
        {
            Student student = null;
            try
            {
                dtc.createConection();

                string querry = "SELECT * FROM Student " +
                                "INNER JOIN Class ON Student.SClass = Class.SClass " +
                                "WHERE SID = @sid";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@sid", sID);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string SID = reader.GetString(0);
                        string SName = reader.GetString(1);
                        string SDOB = reader.GetString(3);
                        string SPhone = reader.GetString(4);
                        string SAddress = reader.GetString(5);
                        string cla = reader.GetString(6);
                        string fac = reader.GetString(7);
                        Faculty SFaculty = new Faculty(fac);
                        Class SClass = new Class(cla, SFaculty);

                        student = new Student(SID, SName, SClass, SDOB, SPhone, SAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Search_Student :" + ex.Message);
            }
            finally { dtc.closeConnection(); }
            return student;
        }

        public bool CheckExist(string sid)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "SELECT * FROM Student WHERE SID = @sid";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@sid", sid);

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

        public bool CreateStudent(Student item)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "INSERT INTO Student VALUES ( @sid , @sname, @class , @sdob , @sphone , @saddress )";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@sid", item.SID);
                cmd.Parameters.AddWithValue("@sname", item.SName);
                cmd.Parameters.AddWithValue("@class", item.SClass.SClass);
                cmd.Parameters.AddWithValue("@sdob", item.SDOB);
                cmd.Parameters.AddWithValue("@sphone", item.SPhone);
                cmd.Parameters.AddWithValue("@saddress", item.SAddress);
                cmd.ExecuteNonQuery();
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Create_Student :" + ex.Message);
            }
            finally { dtc.closeConnection(); }
            return rs;
        }

        public bool UpdateStudent(Student item)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "UPDATE Student SET SName = @sname, SClass = @sclass, SDOB = @sdob, SPhone = @sphone, SAddress = @saddress WHERE SID = @sid ";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@sid", item.SID);
                cmd.Parameters.AddWithValue("@sname", item.SName);
                cmd.Parameters.AddWithValue("@sclass", item.SClass.SClass);
                cmd.Parameters.AddWithValue("@sdob", item.SDOB);
                cmd.Parameters.AddWithValue("@sphone", item.SPhone);
                cmd.Parameters.AddWithValue("@saddress", item.SAddress);
                cmd.ExecuteNonQuery();
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Update_Student :" + ex.Message);
            }
            finally { dtc.closeConnection(); }
            return rs;
        }

        public bool DeleteStudent(Student item)
        {
            bool rs = false;
            try
            {
                dtc.createConection();

                string querry = "DELETE FROM Student WHERE SID = @sid";
                SQLiteCommand cmd = new SQLiteCommand(querry, dtc._con);
                cmd.CommandText = querry;
                cmd.Parameters.AddWithValue("@sid", item.SID);
                cmd.ExecuteNonQuery();
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error_Delete_Student :" + ex.Message);
            }
            finally { dtc.closeConnection(); }
            return rs;
        }
    }
}
