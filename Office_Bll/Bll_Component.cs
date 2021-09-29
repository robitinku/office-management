using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office_Dll;
namespace Office_Bll
{
    public class Bll_Component
    {
        List<db_Work_Category> _listWorkCategory = new List<db_Work_Category>();
        db_Work_Category _db_Work_Category = new db_Work_Category();
        List<db_calendar> _listCalendar = new List<db_calendar>();
        List<db_benifit> _listBenifit = new List<db_benifit>();
        List<db_Cell> _listCell = new List<db_Cell>();
        db_calendar _db_calendar = new db_calendar();
        db_benifit _db_benifit = new db_benifit();
        Dll_Component _Dll_Component = new Dll_Component();

        public List<db_Work_Category> Get_All_WorkCategory()
        {

            _listWorkCategory = _Dll_Component.Get_All_WorkCategory();

            return _listWorkCategory;
        }

        public bool Add(db_Work_Category _db_Work_Category)
        {
            _Dll_Component.Create(_db_Work_Category);

            return true;
        }
        public db_Work_Category Edit(long Work_Id)
        {
            _db_Work_Category = _Dll_Component.Get_Work_Cat_ID(Work_Id);

            return _db_Work_Category;
        }
        public db_work_text Work_Edit_text(long Work_Id)
        {
            db_work_text _db_work_text = _Dll_Component.Work_Edit_text(Work_Id);

            return _db_work_text;
        }
        public db_work_text Work_Edit_text_join(long Work_Id)
        {
            db_work_text _db_work_text = _Dll_Component.Work_Edit_text_join(Work_Id);

            return _db_work_text;
        }
        public bool EditData(db_Work_Category data)
        {
            _Dll_Component.Edit_Work_Category(data);

            return true;
        }
        public bool EditText(db_work_text data)
        {
            _Dll_Component.EditText(data);

            return true;
        }
        
        public List<db_calendar> Get_All_list_calendar()
        {

            _listCalendar = _Dll_Component.Get_All_list_calendar();

            return _listCalendar;
        }

        public bool Add(db_calendar _db_calendar)
        {
            _Dll_Component.Create(_db_calendar);

            return true;
        }
        public db_calendar Get_Calender_ID(long Id)
        {
            _db_calendar = _Dll_Component.Get_Calender_ID(Id);

            return _db_calendar;
        }

        public bool Delete_Calender(db_calendar data)
        {
            _Dll_Component.Delete_Calender(data);

            return true;
        }

       
        public bool Edit_Calender(db_calendar data)
        {
            _Dll_Component.Edit_Calender(data);

            return true;
        }

        public List<db_benifit> Get_All_list_benifit()
        {

            _listBenifit = _Dll_Component.Get_All_list_benifit();

            return _listBenifit;
        }

        public bool Add(db_benifit _db_benifit)
        {
            _Dll_Component.Create(_db_benifit);

            return true;
        }
        public db_benifit Get_Benifit_ID(long Id)
        {
            _db_benifit = _Dll_Component.Get_Benifit_ID(Id);

            return _db_benifit;
        }

        public bool Delete_Benifit(db_benifit data)
        {
            _Dll_Component.Delete_Benifit(data);

            return true;
        }


        public bool Edit_Benifit(db_benifit data)
        {
            _Dll_Component.Edit_Benifit(data);

            return true;
        }


        public List<db_Cell> Get_All_list_Cell()
        {

            _listCell = _Dll_Component.Get_All_list_Cell();

            return _listCell;
        }

        public bool Add(db_Cell db_Cell)
        {
            _Dll_Component.Create(db_Cell);

            return true;
        }
        public db_Cell Get_Cell_ID(long Id)
        {
            db_Cell _db_Cell = _Dll_Component.Get_Cell_ID(Id);

            return _db_Cell;
        }

        public bool Edit_db_Cell(db_Cell data)
        {
            _Dll_Component.Edit_db_Cell(data);

            return true;
        }

        public List<db_Department> Get_All_list_Dep()
        {

            List<db_Department> _listdb_Department = _Dll_Component.Get_All_list_Dep();

            return _listdb_Department;
        }

        public bool Add(db_Department db_Department)
        {
            _Dll_Component.Create(db_Department);

            return true;
        }
        public db_Department Get_Dep_ID(long Id)
        {
            db_Department _db_Department = _Dll_Component.Get_Dep_ID(Id);

            return _db_Department;
        }

        public bool Edit_db_Dep(db_Department data)
        {
            _Dll_Component.Edit_db_Dep(data);

            return true;
        }
        public List<db_Designation> Get_All_list_Designation()
        {

            List<db_Designation> _listDesignation = _Dll_Component.Get_All_list_Designation();

            return _listDesignation;
        }

        public bool Add(db_Designation _db_Designation)
        {
            _Dll_Component.Create(_db_Designation);

            return true;
        }
        public db_Designation Get_Designation_ID(long Id)
        {
            db_Designation _db_Designation = _Dll_Component.Get_Designation_ID(Id);

            return _db_Designation;
        }

        public bool Edit_db_Designation(db_Designation data)
        {
            _Dll_Component.Edit_db_Designation(data);

            return true;
        }

        public List<db_Job_Cate> Get_All_JobCategory()
        {

            List<db_Job_Cate> _listdb_Job_Cate = _Dll_Component.Get_All_JobCategory();

            return _listdb_Job_Cate;
        }

        public bool Add(db_Job_Cate _db_Job_Cate)
        {
            _Dll_Component.Create(_db_Job_Cate);

            return true;
        }
        public db_Job_Cate Get_Job_Cate_ID(long Id)
        {
            
               db_Job_Cate _db_Job_Cate = _Dll_Component.Get_Job_Cate_ID(Id);

            return _db_Job_Cate;
        }
        
        public bool Edit_db_Job_Cate(db_Job_Cate data)
        {
            _Dll_Component.Edit_db_Job_Cate(data);

            return true;
        }
        public List<db_User> Get_All_UserList()
        {

            List<db_User> _listdb_User = _Dll_Component.Get_All_UserList();

            return _listdb_User;
        }

        public List<db_Log> Get_All_LogList()
        {

            List<db_Log> _listdb_Log = _Dll_Component.Get_All_LogList();

            return _listdb_Log;
        }


        public bool CheckPass(db_User data)
        {
            bool result;
            result=_Dll_Component.CheckPass(data);

            return result;
        }
        public bool ChangePass(db_User data)
        {
            bool result;
            result = _Dll_Component.ChangePass(data);

            return result;
        }
        public bool Add(db_User _db_User)
        {
            _Dll_Component.Create(_db_User);

            return true;
        }
        public db_User Get_User_ID(long Id)
        {

            db_User _db_User = _Dll_Component.Get_User_ID(Id);

            return _db_User;
        }

        public bool Edit_User(db_User data, long userid)
        {
            _Dll_Component.Edit_User(data, userid);

            return true;
        }
        public bool NameCheckUser(db_User _db_User)
        {
            bool result = _Dll_Component.NameCheckUser(_db_User);

            return result;
        }
        public bool NameCheck(db_Job_Cate _db_Job_Cate)
        {
          bool result=  _Dll_Component.NameCheck(_db_Job_Cate);

            return result;
        }

        public bool NameCheckCell(db_Cell _db_Cell)
        {
            bool result = _Dll_Component.NameCheckCell(_db_Cell);

            return result;
        }
        
             public bool NameCheckDesignation(db_Designation _db_Designation)
        {
            bool result = _Dll_Component.NameCheckDesignation(_db_Designation);

            return result;
        }
        

          public bool NameCheckWork(db_Work_Category _db_Work_Category)
        {
            bool result = _Dll_Component.NameCheckWork(_db_Work_Category);

            return result;
        }

        public bool NameCheckDep(db_Department _db_Department)
        {
            bool result = _Dll_Component.NameCheckDep(_db_Department);

            return result;
        }

        public string converttobangla(string number)
        {
            var chars = number.ToCharArray();
            string result = "";
            foreach (char ch in chars)
            {
                if (ch == '1')
                {
                    result += '১';
                }
                else if (ch == '2')
                {
                    result += '২';
                }
                else if (ch == '3')
                {
                    result += '৩';
                }
                else if (ch == '4')
                {
                    result += '৪';
                }
                else if (ch == '5')
                {
                    result += '৫';
                }
                else if (ch == '6')
                {
                    result += '৬';
                }
                else if (ch == '7')
                {
                    result += '৭';
                }
                else if (ch == '8')
                {
                    result += '৮';
                }
                else if (ch == '9')
                {
                    result += '৯';
                }
                else if (ch == '0')
                {
                    result += '০';
                }
                else
                {
                    result += ch;
                }

            }
            return result;
        }

    }
}
