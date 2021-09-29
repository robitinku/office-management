using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Office_Dll
{
    
    public class Dll_Component
    {
        private Office_ManagementEntities dbContext = new Office_ManagementEntities();
        db_Work_Category _db_Work_Category = new db_Work_Category();
        db_calendar _db_calendar = new db_calendar();
        db_benifit _db_benifit = new db_benifit();
        public List<db_Work_Category> Get_All_WorkCategory()
        {
            List<db_Work_Category> _listWorkCategory = new List<db_Work_Category>();
            _listWorkCategory = (from list in dbContext.db_Work_Category
                                 orderby list.Name ascending
                                 select list).ToList();

            return _listWorkCategory;
        }
        public bool Create(db_Work_Category _db_Work_Category)
        {
            using (TransactionScope tran = new TransactionScope())
            {
                try {
                dbContext.db_Work_Category.Add(_db_Work_Category);

            db_work_text _db_work_text = new db_work_text();
            _db_work_text.Work_Id = _db_Work_Category.Work_Id;
            dbContext.db_work_text.Add(_db_work_text);
            dbContext.SaveChanges();
                    tran.Complete();
                }
                catch(Exception ex)
                {
                    tran.Dispose();
                }
            }
            return true;
        }

        public db_Work_Category Get_Work_Cat_ID(long id)
        {

            _db_Work_Category = (from work in dbContext.db_Work_Category
                            where work.Work_Id == id
                            select work).SingleOrDefault();

            return _db_Work_Category;
        }
        
             public db_work_text Work_Edit_text_join(long id)
        {

            db_work_text _db_work_text = (from work in dbContext.db_work_text
                                          where work.Work_Id == id
                                          select work).SingleOrDefault();

            return _db_work_text;
        }
        public db_work_text Work_Edit_text(long id)
        {

            db_work_text _db_work_text = (from work in dbContext.db_work_text
                                          where work.Work_Id == id
                                 select work).SingleOrDefault();

            return _db_work_text;
        }
        
        public bool Edit_Work_Category(db_Work_Category _db_Work_Category)
        {


            db_Work_Category _db_Work_Category_local;

            _db_Work_Category_local = dbContext.db_Work_Category.Single(x => x.Work_Id == _db_Work_Category.Work_Id);

            _db_Work_Category_local.Name = _db_Work_Category.Name;
            _db_Work_Category_local.Start_Time = _db_Work_Category.Start_Time;
            _db_Work_Category_local.End_Time = _db_Work_Category.End_Time;
            _db_Work_Category_local.Status= _db_Work_Category.Status;
            _db_Work_Category_local.Entertainment = _db_Work_Category.Entertainment;
            _db_Work_Category_local.Travel = _db_Work_Category.Travel;
            _db_Work_Category_local.work_type = _db_Work_Category.work_type;
            _db_Work_Category_local.Cell_ID = _db_Work_Category.Cell_ID;
            _db_Work_Category_local.Detail = _db_Work_Category.Detail;
            dbContext.SaveChanges();

            return true;
        }

        public bool EditText(db_work_text _db_work_text)
        {


            db_work_text _db_work_text_local;

            _db_work_text_local = dbContext.db_work_text.Single(x => x.WorkTextId == _db_work_text.WorkTextId);

            _db_work_text_local.Subject = _db_work_text.Subject;
            _db_work_text_local.DetailHeader = _db_work_text.DetailHeader;
            _db_work_text_local.DetailFottor = _db_work_text.DetailFottor;
            
            dbContext.SaveChanges();

            return true;
        }
        public List<db_calendar> Get_All_list_calendar()
        {
            List<db_calendar> _listdb_calendar = new List<db_calendar>();
            _listdb_calendar = (from list in dbContext.db_calendar
                                 orderby list.HoliDay_Date
                                 select list).ToList();

            return _listdb_calendar;
        }
        public bool Create(db_calendar _db_calendar)
        {

            dbContext.db_calendar.Add(_db_calendar);

            dbContext.SaveChanges();

            return true;
        }
        public bool Delete_Calender(db_calendar data)
        {
           db_calendar _db_calendar_local = dbContext.db_calendar.Single(x => x.Id == data.Id);
            dbContext.db_calendar.Remove(_db_calendar_local);

            dbContext.SaveChanges();

            return true;
        }
        
        public db_calendar Get_Calender_ID(long id)
        {

            _db_calendar = (from Calender in dbContext.db_calendar
                                 where Calender.Id == id
                                 select Calender).SingleOrDefault();

            return _db_calendar;
        }

        public bool Edit_Calender(db_calendar _db_calendar)
        {


            db_calendar _db_calendar_local;

            _db_calendar_local = dbContext.db_calendar.Single(x => x.Id == _db_calendar.Id);

            _db_calendar_local.Discription = _db_calendar.Discription;
            _db_calendar_local.HoliDay_Date = _db_calendar.HoliDay_Date;
            _db_calendar_local.Status = _db_calendar.Status;
            dbContext.SaveChanges();

            return true;
        }

        /// <summary>
        public List<db_benifit> Get_All_list_benifit()
        {
            List<db_benifit> _listdb_benifit = new List<db_benifit>();
            _listdb_benifit = (from list in dbContext.db_benifit
                               orderby list.id
                                select list).ToList();

            return _listdb_benifit;
        }
        public bool Create(db_benifit _db_benifit)
        {

            dbContext.db_benifit.Add(_db_benifit);

            dbContext.SaveChanges();

            return true;
        }
        public bool Delete_Benifit(db_benifit data)
        {
            db_benifit _db_benifit_local = dbContext.db_benifit.Single(x => x.id == data.id);
            dbContext.db_benifit.Remove(_db_benifit_local);

            dbContext.SaveChanges();

            return true;
        }

        public db_benifit Get_Benifit_ID(long id)
        {

            _db_benifit = (from Benifit in dbContext.db_benifit
                            where Benifit.id == id
                            select Benifit).SingleOrDefault();

            return _db_benifit;
        }

        public bool Edit_Benifit(db_benifit _db_benifit)
        {
            db_benifit _db_benifit_local;
            _db_benifit_local = dbContext.db_benifit.Single(x => x.id == _db_benifit.id);
            _db_benifit_local.User_id = _db_benifit.User_id;
            _db_benifit_local.amount = _db_benifit.amount;
            _db_benifit_local.date = _db_benifit.date;
            _db_benifit_local.comments = _db_benifit.comments;
            _db_benifit_local.type = _db_benifit.type;
            dbContext.SaveChanges();

            return true;
        }
        /// </summary>





        public List<db_Cell> Get_All_list_Cell()
        {
            List<db_Cell> _listdb_Cell = new List<db_Cell>();
            _listdb_Cell = (from list in dbContext.db_Cell
                            orderby list.Name ascending
                            select list).ToList();

            return _listdb_Cell;
        }
        public bool Create(db_Cell db_Cell)
        {

            dbContext.db_Cell.Add(db_Cell);

            dbContext.SaveChanges();

            return true;
        }

        public db_Cell Get_Cell_ID(long id)
        {

            db_Cell _db_Cell = (from item in dbContext.db_Cell
                            where item.Cell_Id == id
                            select item).SingleOrDefault();

            return _db_Cell;
        }

        public bool Edit_db_Cell(db_Cell _db_Cell)
        {


            db_Cell _db_db_Cell;

            _db_db_Cell = dbContext.db_Cell.Single(x => x.Cell_Id == _db_Cell.Cell_Id);

            _db_db_Cell.Name = _db_Cell.Name;
            
            _db_db_Cell.Status = _db_Cell.Status;
            _db_db_Cell.Short_name = _db_Cell.Short_name;
            dbContext.SaveChanges();

            return true;
        }


        public List<db_Department> Get_All_list_Dep()
        {
            List<db_Department> _listdb_Department = new List<db_Department>();
            _listdb_Department = (from list in dbContext.db_Department
                                  where list.Priority==2
                            orderby list.Name ascending 
                            select list).ToList();

            return _listdb_Department;
        }
        public bool Create(db_Department _db_Department)
        {

            dbContext.db_Department.Add(_db_Department);

            dbContext.SaveChanges();

            return true;
        }

        public db_Department Get_Dep_ID(long id)
        {

            db_Department _db_Department = (from item in dbContext.db_Department
                                where item.Dep_id == id
                                select item).SingleOrDefault();

            return _db_Department;
        }

        public bool Edit_db_Dep(db_Department _db_Department)
        {


            db_Department _db_Departmentl;

            _db_Departmentl = dbContext.db_Department.Single(x => x.Dep_id == _db_Department.Dep_id);

            _db_Departmentl.Name = _db_Department.Name;

            _db_Departmentl.Status = _db_Department.Status;
            dbContext.SaveChanges();

            return true;
        }
        public List<db_Designation> Get_All_list_Designation()
        {

            List<db_Designation> _listdb_Designation = (from list in dbContext.db_Designation
                                                        orderby list.Name ascending
                                                        select list).ToList();

            return _listdb_Designation;
        }
        public bool Create(db_Designation db_Designation)
        {

            dbContext.db_Designation.Add(db_Designation);

            dbContext.SaveChanges();

            return true;
        }

        public db_Designation Get_Designation_ID(long id)
        {

            db_Designation _db_Designation = (from item in dbContext.db_Designation
                                where item.Designation_Id == id
                                select item).SingleOrDefault();

            return _db_Designation;
        }

        public bool Edit_db_Designation(db_Designation _db_Designation)
        {


            db_Designation _db_Designation_local;

            _db_Designation_local = dbContext.db_Designation.Single(x => x.Designation_Id == _db_Designation.Designation_Id);

            _db_Designation_local.Name = _db_Designation.Name;
            _db_Designation_local.Bn_Name = _db_Designation.Bn_Name;
            _db_Designation_local.Status = _db_Designation.Status;
            dbContext.SaveChanges();

            return true;
        }

        public List<db_Job_Cate> Get_All_JobCategory()
        {

            List<db_Job_Cate> _listdb_Job_Cate = (from list in dbContext.db_Job_Cate
                                                  orderby list.Name ascending
                                                  select list).ToList();

            return _listdb_Job_Cate;
        }
        public bool Create(db_Job_Cate _db_Job_Cate)
        {
           
            dbContext.db_Job_Cate.Add(_db_Job_Cate);

            dbContext.SaveChanges();

            return true;
        }

        public db_Job_Cate Get_Job_Cate_ID(long id)
        {

            db_Job_Cate _db_Job_Cate = (from item in dbContext.db_Job_Cate
                                              where item.Category_Id == id
                                              select item).SingleOrDefault();

            return _db_Job_Cate;
        }

        public bool Edit_db_Job_Cate(db_Job_Cate _db_Job_Cate)
        {


            db_Job_Cate _db_Job_Cate_local;

            _db_Job_Cate_local = dbContext.db_Job_Cate.Single(x => x.Category_Id == _db_Job_Cate.Category_Id);

            _db_Job_Cate_local.Name = _db_Job_Cate.Name;

            _db_Job_Cate_local.Status = _db_Job_Cate.Status;
            dbContext.SaveChanges();

            return true;
        }

        public List<db_User> Get_All_UserList()
        {

            List<db_User> _listdb_User = (from list in dbContext.db_User
                                          orderby list.User_Name ascending
                                                  select list).ToList();

            return _listdb_User;
        }

        public List<db_Log> Get_All_LogList()
        {

            List<db_Log> _listdb_Log = (from list in dbContext.db_Log
                                          orderby list.Log_Id descending
                                          select list).ToList();

            return _listdb_Log;
        }


        public bool Create(db_User _db_User)
        {

            dbContext.db_User.Add(_db_User);

            dbContext.SaveChanges();

            return true;
        }
        public bool CheckPass(db_User data)
        {
            List<db_User> _listdb_User = (from list in dbContext.db_User
                                       where list.User_Id== data.User_Id && list.Password == data.Password
                                          select list).ToList();

  
            if(_listdb_User.Count>0)
            return true;
            else
                return false;
        }

        public bool ChangePass(db_User data)
        {
            db_User _db_User_local;

            _db_User_local = dbContext.db_User.Single(x => x.User_Id == data.User_Id);

            _db_User_local.Password = data.Password;

            dbContext.SaveChanges();
            return true;
        }
        public db_User Get_User_ID(long id)
        {

            db_User _db_User = (from item in dbContext.db_User
                                where item.User_Id == id
                                        select item).SingleOrDefault();

            return _db_User;
        }

        public bool Edit_User(db_User _db_User, long userid)
        {
            db_User _db_User_local;
            db_User _db_User_log;
            _db_User_local = dbContext.db_User.Single(x => x.User_Id == _db_User.User_Id);
            _db_User_log= _db_User_local;
            //for tracking log
            db_Log _db_Log = new db_Log();
            _db_Log.User_Id = userid;
            _db_Log.User_Name = _db_User_log.User_Name;
            _db_Log.Password = _db_User_log.Password;
            _db_Log.Status = _db_User_log.Status;
            _db_Log.Category = _db_User_log.Category;
            _db_Log.created = DateTime.Now;
            dbContext.db_Log.Add(_db_Log);
            dbContext.SaveChanges();

            _db_User_local.User_Name = _db_User.User_Name;
            if (_db_User.Password != null)
            {
                _db_User_local.Password = _db_User.Password;
            }
            _db_User_local.Category = _db_User.Category;
            _db_User_local.Status = _db_User.Status;
            dbContext.SaveChanges();
            

            return true;
        }
        public bool NameCheck(db_Job_Cate _db_Job_Cate)
        {
            List<db_Job_Cate> _listdb_Job_Cate=null;
            List<db_Job_Cate> _listdb_Job_Catetem = null;
            if (_db_Job_Cate.Category_Id!=0)
            {
                _listdb_Job_Cate = (from item in dbContext.db_Job_Cate
                                    where item.Name == _db_Job_Cate.Name && item.Category_Id == _db_Job_Cate.Category_Id
                                    select item).ToList();
                if(_listdb_Job_Cate.Count==0)
                {
                    _listdb_Job_Catetem = (from item in dbContext.db_Job_Cate
                                        where item.Name == _db_Job_Cate.Name
                                        select item).ToList();

                }


                if (_listdb_Job_Cate.Count > 0)
                    return true;
               else if (_listdb_Job_Catetem.Count > 0)
                    return false;
                else
                    return true;
            }
            else  { 
           _listdb_Job_Cate = (from item in dbContext.db_Job_Cate
                                        where item.Name == _db_Job_Cate.Name
                                                  select item).ToList();

            
            if(_listdb_Job_Cate.Count>0)
                return false;
            else
                    return true;
            }
            
           
        }

        public bool NameCheckCell(db_Cell _db_Cell)
        {
            List<db_Cell> _listdb_Cell = null;
            List<db_Cell> _listdb_Celltem = null;
            if (_db_Cell.Cell_Id != 0)
            {
                _listdb_Cell = (from item in dbContext.db_Cell
                                    where item.Name == _db_Cell.Name && item.Cell_Id == _db_Cell.Cell_Id
                                    select item).ToList();
                if (_listdb_Cell.Count == 0)
                {
                    _listdb_Celltem = (from item in dbContext.db_Cell
                                           where item.Name == _db_Cell.Name
                                           select item).ToList();

                }


                if (_listdb_Cell.Count > 0)
                    return true;
                else if (_listdb_Celltem.Count > 0)
                    return false;
                else
                    return true;
            }
            else {
                _listdb_Cell = (from item in dbContext.db_Cell
                                    where item.Name == _db_Cell.Name
                                    select item).ToList();


                if (_listdb_Cell.Count > 0)
                    return false;
                else
                    return true;
            }


        }

        public bool NameCheckDesignation(db_Designation _db_Designation)
        {
            List<db_Designation> _listdb_Designation = null;
            List<db_Designation> _listdb_Designationtem = null;
            if (_db_Designation.Designation_Id != 0)
            {
                _listdb_Designation = (from item in dbContext.db_Designation
                                where item.Name == _db_Designation.Name && item.Designation_Id == _db_Designation.Designation_Id
                                       select item).ToList();
                if (_listdb_Designation.Count == 0)
                {
                    _listdb_Designationtem = (from item in dbContext.db_Designation
                                       where item.Name == _db_Designation.Name
                                       select item).ToList();

                }


                if (_listdb_Designation.Count > 0)
                    return true;
                else if (_listdb_Designationtem.Count > 0)
                    return false;
                else
                    return true;
            }
            else {
                _listdb_Designation = (from item in dbContext.db_Designation
                                where item.Name == _db_Designation.Name
                                select item).ToList();


                if (_listdb_Designation.Count > 0)
                    return false;
                else
                    return true;
            }


        }

        public bool NameCheckWork(db_Work_Category _db_Work_Category)
        {
            List<db_Work_Category> _listdb_Work_Category = null;
            List<db_Work_Category> _listdb_Work_Categorytem = null;
            if (_db_Work_Category.Work_Id != 0)
            {
                _listdb_Work_Category = (from item in dbContext.db_Work_Category
                                         where item.Name == _db_Work_Category.Name && item.Work_Id == _db_Work_Category.Work_Id
                                         select item).ToList();
                if (_listdb_Work_Category.Count == 0)
                {
                    _listdb_Work_Categorytem = (from item in dbContext.db_Work_Category
                                                where item.Name == _db_Work_Category.Name
                                              select item).ToList();

                }


                if (_listdb_Work_Category.Count > 0)
                    return true;
                else if (_listdb_Work_Categorytem.Count > 0)
                    return false;
                else
                    return true;
            }
            else {
                _listdb_Work_Category = (from item in dbContext.db_Work_Category
                                         where item.Name == _db_Work_Category.Name
                                       select item).ToList();


                if (_listdb_Work_Category.Count > 0)
                    return false;
                else
                    return true;
            }


        }
        public bool NameCheckUser(db_User _db_User)
        {
            List<db_User> _listdb_User = null;
            List<db_User> _listdb_Usertem = null;
            if (_db_User.User_Id != 0)
            {
                _listdb_User = (from item in dbContext.db_User
                                where item.User_Name == _db_User.User_Name && item.User_Id == _db_User.User_Id
                                select item).ToList();
                if (_listdb_User.Count == 0)
                {
                    _listdb_Usertem = (from item in dbContext.db_User
                                       where item.User_Name == _db_User.User_Name
                                                select item).ToList();

                }


                if (_listdb_User.Count > 0)
                    return true;
                else if (_listdb_Usertem.Count > 0)
                    return false;
                else
                    return true;
            }
            else {
                _listdb_User = (from item in dbContext.db_User
                                         where item.User_Name == _db_User.User_Name
                                select item).ToList();


                if (_listdb_User.Count > 0)
                    return false;
                else
                    return true;
            }


        }


        public bool NameCheckDep(db_Department _db_Department)
        {
            List<db_Department> _listdb_Department = null;
            List<db_Department> _listdb_Departmenttem = null;
            if (_db_Department.Dep_id != 0)
            {
                _listdb_Department = (from item in dbContext.db_Department
                                where item.Name == _db_Department.Name && item.Dep_id == _db_Department.Dep_id
                                      select item).ToList();
                if (_listdb_Department.Count == 0)
                {
                    _listdb_Departmenttem = (from item in dbContext.db_Department
                                             where item.Name == _db_Department.Name
                                             select item).ToList();

                }


                if (_listdb_Department.Count > 0)
                    return true;
                else if (_listdb_Departmenttem.Count > 0)
                    return false;
                else
                    return true;
            }
            else {
                _listdb_Department = (from item in dbContext.db_Department
                                      where item.Name == _db_Department.Name
                                      select item).ToList();


                if (_listdb_Department.Count > 0)
                    return false;
                else
                    return true;
            }


        }


    }
}
