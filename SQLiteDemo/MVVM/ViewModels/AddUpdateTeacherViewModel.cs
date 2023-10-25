using SQLiteDemo.DAO;
using SQLiteDemo.MVVM.Command;
using SQLiteDemo.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLiteDemo.MVVM.ViewModels
{
    internal class AddUpdateTeacherViewModel : BindableBase
    {
        #region properties
        private TeacherDB teacherDBConnecter;


        #endregion

        #region command
        public VfxCommand SaveCommand { get; set; }
        private void OnSave(object obj)
        {
            if(obj is Views.AddUpdateTeacherView)
            {
                /*if (teacherDBConnecter.CheckExist())
                {
                    MessageBox.Show(Faculty_tb + " is already available on the database", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (facDB.CreateFaculty(Faculty_tb))
                    {
                        Faculty_tb = null;
                        loadData();
                    }
                    else
                    {
                        MessageBox.Show("Can't Create Faculty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }*/
            }
        }
        private bool CanSave()
        {
            return true;
        }

        public VfxCommand CloseWindowCommand { get; set; }
        private void OnCloseWindow(object obj)
        {
            if (obj is Views.AddUpdateTeacherView wd)
            {
                wd.Close();
            }
        }

        #endregion

        public AddUpdateTeacherViewModel()
        {
            Init_Model();
            Init_Command();
        }

        private void Init_Command()
        {
            SaveCommand = new VfxCommand(OnSave, CanSave);
            CloseWindowCommand = new VfxCommand(OnCloseWindow, () => true);
        }

        private void Init_Model()
        {
            teacherDBConnecter = new TeacherDB();
        }
    }
}
