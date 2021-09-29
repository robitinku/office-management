using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Office_Dll
{

    public class Dll_Employee
    {
        private Office_ManagementEntities dbContext = new Office_ManagementEntities();
        db_Employee _db_Employee = new db_Employee();
        db_Order _db_Order = new db_Order();
        public object _listEmpAct { get; set; }
        public object Employeeinfo { get; private set; }
        public List<db_Department> Get_Dep_all()
        {
            List<db_Department> _listdb_Department = new List<db_Department>();
            _listdb_Department = (from list in dbContext.db_Department
                                  where list.Status == true
                                  orderby list.Name ascending
                                  select list).ToList();

            return _listdb_Department;
        }

        public List<db_Employee> GetEmp()
        {
            List<db_Employee> _listEmp = new List<db_Employee>();
            _listEmp = (from list in dbContext.db_Employee
                        orderby list.Designation_Id ascending
                        select list).ToList();
            return _listEmp;
        }

        public bool Create(db_Employee _db_Employee)
        {
           
            dbContext.db_Employee.Add(_db_Employee);

            if (_db_Employee.Imagetext != null)
            { 
                db_image _db_image = new db_image();
                _db_image.image = System.Text.Encoding.ASCII.GetBytes(_db_Employee.Imagetext);
                _db_image.Emp_Id = _db_Employee.Emp_Id;
                _db_image.User_Id = _db_Employee.User_Id;
                _db_image.Date = _db_Employee.Date;
                dbContext.db_image.Add(_db_image);
            }
            if(_db_Employee.list_db_Emp_Cell!=null)
            {
            foreach (var item in _db_Employee.list_db_Emp_Cell)
            {
                db_Emp_Cell _db_Emp_Cell = new db_Emp_Cell();

                _db_Emp_Cell.Cell_Id = item;
                _db_Emp_Cell.Emp_Id = _db_Employee.Emp_Id;
                _db_Emp_Cell.User_Id = _db_Employee.User_Id;
                _db_Emp_Cell.Date = _db_Employee.Date;
                dbContext.db_Emp_Cell.Add(_db_Emp_Cell);
            }
           }
            dbContext.SaveChanges();
               
            return true;
        }
        public bool EditData(db_Employee _db_Employee)
        {
            db_image _db_img = dbContext.db_image.Where(x => x.Emp_Id == _db_Employee.Emp_Id).FirstOrDefault();
            if (_db_img == null && _db_Employee.Imagetext != null)
            {
                db_image _db_image = new db_image();
                _db_image.image = System.Text.Encoding.ASCII.GetBytes(_db_Employee.Imagetext);
                _db_image.Emp_Id = _db_Employee.Emp_Id;
                _db_image.User_Id = _db_Employee.User_Id;
                _db_image.Date = _db_Employee.Date;
                dbContext.db_image.Add(_db_image);
            }
              else  if (_db_Employee.Imagetext != null)
            {
                db_image _db_image = dbContext.db_image.Single(x => x.Emp_Id == _db_Employee.Emp_Id);
                _db_image.image = System.Text.Encoding.ASCII.GetBytes(_db_Employee.Imagetext);
                _db_image.User_Id = _db_Employee.User_Id;
                _db_image.Date = _db_Employee.Date;

            }
                

               db_Employee _db_Employee_local;

            _db_Employee_local = dbContext.db_Employee.Single(x => x.Emp_Id == _db_Employee.Emp_Id);

            _db_Employee_local.Emp_Name = _db_Employee.Emp_Name;
            _db_Employee_local.Bn_Emp_Name = _db_Employee.Bn_Emp_Name;
            _db_Employee_local.Designation_Id = _db_Employee.Designation_Id;
            _db_Employee_local.Bank_Id = _db_Employee.Bank_Id;
            _db_Employee_local.promotion_Date = _db_Employee.promotion_Date;
            _db_Employee_local.Email = _db_Employee.Email;
            _db_Employee_local.Mobile = _db_Employee.Mobile;
            _db_Employee_local.Present_Posting = _db_Employee.Present_Posting;
            
            _db_Employee_local.Department = _db_Employee.Department;
            _db_Employee_local.Present_Address = _db_Employee.Present_Address;
            _db_Employee_local.Category_Id = _db_Employee.Category_Id;
            _db_Employee_local.Status = _db_Employee.Status;
            _db_Employee_local.Work_Id = _db_Employee.Work_Id;
            _db_Employee_local.Cell_Id = _db_Employee.Cell_Id;

            List<db_Emp_Cell> _list_db_Emp_Cell = dbContext.db_Emp_Cell.Where(x => x.Emp_Id == _db_Employee.Emp_Id).ToList();
            foreach (var _db_Emp_Cell in _list_db_Emp_Cell)
            {
                dbContext.db_Emp_Cell.Remove(_db_Emp_Cell);
                
            }
            if (_db_Employee.list_db_Emp_Cell != null)
            {
                foreach (var item in _db_Employee.list_db_Emp_Cell)
                {
                    db_Emp_Cell _db_Emp_Cell = new db_Emp_Cell();

                    _db_Emp_Cell.Cell_Id = item;
                    _db_Emp_Cell.Emp_Id = _db_Employee.Emp_Id;
                    _db_Emp_Cell.User_Id = _db_Employee.User_Id;
                    _db_Emp_Cell.Date = _db_Employee.Date;
                    dbContext.db_Emp_Cell.Add(_db_Emp_Cell);
                }
            }
            dbContext.SaveChanges();

            return true;
        }
        
          public object Get_Emp_list()
         {
         
            _listEmpAct = (from list in dbContext.db_Employee
                         where list.Status == true
                         orderby list.Emp_Name ascending
                         select new { id = list.Emp_Id, value = list.Emp_Name }).ToList();

            return _listEmpAct;
        }

        
             public List<db_Designation> Get_Designation_list_Active()
        {
            List<db_Designation> _listdb_Designation = new List<db_Designation>();
            _listdb_Designation = (from list in dbContext.db_Designation
                         where list.Status == true
                                   orderby list.Name ascending
                                   select list).ToList();

            return _listdb_Designation;
        }
        public List<db_Cell> Get_Cell_list()
        {
            List<db_Cell> _listCell = new List<db_Cell>();
            _listCell = (from list in dbContext.db_Cell
                         where list.Status==true
                         orderby list.Name ascending
                         select list).ToList();

            return _listCell;
        }
        
            public List<db_Work_Category> Get_Work_list()
        {
            List<db_Work_Category> _list_work_category = new List<db_Work_Category>();
            _list_work_category = (from list in dbContext.db_Work_Category
                              where list.Status == true
                                   orderby list.Name ascending
                                   select list).ToList();

            return _list_work_category;
        }
        public List<db_Work_Category> Get_Work_list_Order(long Id)
        {
            List<db_Work_Category> _list_work_category = new List<db_Work_Category>();
          

          var  list_work_category = (from list in dbContext.db_Order_Detail
                         where list.Order_Id == Id
                         group list by new { list.db_Work_Category.Work_Id, list.db_Work_Category.Name }
             into grp
                         select new 
                         {
                             Work_Id=grp.Key.Work_Id,
                             Name = grp.Key.Name,

                         }).ToList();
            foreach(var item in list_work_category)
            {
                db_Work_Category db_Work_Category_tem = new db_Work_Category();
                db_Work_Category_tem.Name = item.Name;
                db_Work_Category_tem.Work_Id = item.Work_Id;
                _list_work_category.Add(db_Work_Category_tem);
            }

            return _list_work_category;
        }
        public List<db_Job_Cate> Get_Job_Cate()
        {
            List<db_Job_Cate> _list_Job_Cate = new List<db_Job_Cate>();
            _list_Job_Cate = (from list in dbContext.db_Job_Cate
                         where list.Status == true
                              orderby list.Name ascending
                              select list).ToList();

            return _list_Job_Cate;
        }

        public db_Employee Get_Emp_By_ID(long id)
        {

            _db_Employee = (from Emp in dbContext.db_Employee
                            where Emp.Emp_Id == id
                            select Emp).SingleOrDefault();

            return _db_Employee;
        }

        public db_Order Get_Order_By_ID(long id)
        {

            _db_Order = (from Ord in dbContext.db_Order
                            where Ord.Order_Id == id
                            select Ord).SingleOrDefault();

            return _db_Order;
        }


        public object Get_Emp_By_ID_Json(long empid)
        {

            Employeeinfo = (from list in dbContext.db_Employee
             where list.Emp_Id == empid
             select new
             {
                 Bank_Id = list.Bank_Id,
                 Designation = list.db_Designation.Name
             }).SingleOrDefault();

            return Employeeinfo;
        }
       
    public List<db_Employee> GetEmpAll(int page, string searchString)
        {
            List<db_Employee> _listEmp = new List<db_Employee>();
            if (!String.IsNullOrEmpty(searchString))
            {
                
                _listEmp = (from list in dbContext.db_Employee
                            orderby list.Emp_Name ascending
                            where list.Department == 5 && list.Emp_Name.Contains(searchString)
                            select list).ToList();
            }
            else {
                //Skip(20 * page).Take(20)
                /* _listEmp = (from list in dbContext.db_Employee
                         where list.Department == 5
                             orderby list.Emp_Name ascending
                             select list).ToList();
                 _listEmp = (from list in dbContext.db_Employee
                             join desg in dbContext.db_Designation on list.Designation_Id equals desg.Designation_Id
                             where list.Department == 5 & list.Status == true
                             orderby desg.Serial ascending
                             select list).ToList();*/
                _listEmp = (from list in dbContext.db_Employee
                            join desg in dbContext.db_Designation on list.Designation_Id equals desg.Designation_Id
                            where list.Department == 5
                            orderby desg.Serial,list.promotion_Date  ascending
                            select list).ToList();


                

            }
            return _listEmp;
        }

        public List<db_Employee> GetEmpAll2(int page, string searchString)
        {
            List<db_Employee> _listEmp = new List<db_Employee>();
            if (!String.IsNullOrEmpty(searchString))
            {

                _listEmp = (from list in dbContext.db_Employee
                            orderby list.Emp_Name ascending
                            where list.Department == 5 && list.Emp_Name.Contains(searchString)
                            select list).ToList();
            }
            else {
                //Skip(20 * page).Take(20)
                _listEmp = (from list in dbContext.db_Employee
                            where list.Department == 5 & list.Status==true
                            orderby list.Designation_Id ascending
                            select list).ToList();
            }
            return _listEmp;
        }

        public List<db_Employee> GetEmpAllpdf()
        {
            /**
            from item in dbContext.db_Order
                        join detail in dbContext.db_Order_Detail on item.Order_Id equals detail.Order_Id
                        where item.Order_Id == Id
                        orderby detail.From_Date
            _listEmp = (from list in dbContext.db_Employee
                        join desg in dbContext.db_Designation on list.Designation_Id equals desg.Designation_Id
                        where list.Department == 5
                        orderby desg.Serial, list.promotion_Date ascending
                        select list).ToList();*/
            List<db_Employee> _listEmp = new List<db_Employee>();
                _listEmp = (from list in dbContext.db_Employee
                            join desg in dbContext.db_Designation on list.Designation_Id equals desg.Designation_Id
                            where list.Department == 5 & list.Status == true
                            orderby desg.Serial, list.promotion_Date ascending
                            select list).ToList();
            return _listEmp;
        }

        public int GetEmpAll( string searchString)
        {
           int  count;
            if (!String.IsNullOrEmpty(searchString))
            {

                count = (from list in dbContext.db_Employee
                            where list.Department == 5 && list.Emp_Name.Contains(searchString)
                         orderby list.Emp_Name ascending
                         select list).Count();
            }
            else {

                count = (from list in dbContext.db_Employee
                            where list.Department == 5
                         orderby list.Emp_Name ascending
                         select list).Count();
            }
            return count;
        }

        public string Save_order_detail(List<db_Order_Detail> data,long userid)
        {
            string result = "Save Successfully"; ;
            int check=0;
            using (TransactionScope tran = new TransactionScope())
            {
                db_Order _db_Order = new db_Order();
                _db_Order.EnDate = DateTime.Now;
                _db_Order.User_Id = userid;
                _db_Order.Date = data[0].Date;
                _db_Order.Refno = data[0].Refno;
                _db_Order.Detail = data[0].Detail;
                _db_Order.Order_No = DateTime.Now.ToString("yyMMddHHmmssff");
                dbContext.db_Order.Add(_db_Order);
                dbContext.SaveChanges();

                foreach (db_Order_Detail _db_Order_Detail in data)
                {
                    
                    _db_Order_Detail.Order_Id = _db_Order.Order_Id;

                    _db_Order_Detail.Date = DateTime.Now;
                    _db_Order_Detail.User_Id = userid;
              
                    if (_db_Order_Detail.Work_Id == 0)
                    {
                        tran.Dispose();
                        result = "Work Type Missing";
                        return result;
                    }

                    else
                    {
                        check = WorkCheck(_db_Order_Detail);
                    }
                    if (check != 0)
                    {
                        if (check == 1)
                        {
                            tran.Dispose();
                            db_Employee _db_Employee = dbContext.db_Employee.Where(x => x.Emp_Id == _db_Order_Detail.Emp_Id).SingleOrDefault();
                            result = _db_Employee.Emp_Name + " is already another task (Order ID:" + _db_Order.Order_Id + ")";
                        }
                        if (check == 2)
                        {
                            tran.Dispose();
                            result =  " Late Setting is not allow holiday";
                        }
                        if (check == 3)
                        {
                            tran.Dispose();
                            result = " Late Setting and another work is not  allow same day";
                        }
                        if (check == 4)
                        {
                            tran.Dispose();
                            result = " Same time multiple work is not allow";
                        }
                        return result;
                    }
                    string[] From_Datealllist = _db_Order_Detail.From_Dateall.Split(',');

                    foreach (string datea in From_Datealllist)
                    {

                        Dll_Component _Dll_Component = new Dll_Component();
                        _db_Order_Detail.From_Date = DateTime.Parse(datea);
                        _db_Order_Detail.To_Date = DateTime.Parse(datea);
                        db_Work_Category _db_Work_Category = _Dll_Component.Get_Work_Cat_ID(_db_Order_Detail.Work_Id);
                        _db_Order_Detail.Start_time = _db_Work_Category.Start_Time;
                        _db_Order_Detail.End_time = _db_Work_Category.End_Time;
                        dbContext.db_Order_Detail.Add(_db_Order_Detail);
                        dbContext.SaveChanges();
                    }

                }

                
                tran.Complete();
            }
            return result;
        }




        public List<EmpInfo> Get_EmpInfoStatus(db_Order_Detail _db_Order_Detail)
        {
            List<db_Order_Detail> _list_Emp_Status = new List<db_Order_Detail>();
            List<EmpInfo> _list_Emp_Result = new List<EmpInfo>();
            _list_Emp_Status = (from list in dbContext.db_Order_Detail
                                   where list.Emp_Id == _db_Order_Detail.Emp_Id 
                                   & list.From_Date>= _db_Order_Detail.From_Date & list.To_Date <= _db_Order_Detail.To_Date
                                select list).ToList();
            db_Employee _db_Employee = (from emp in dbContext.db_Employee
                                        where emp.Emp_Id == _db_Order_Detail.Emp_Id
                                select emp).FirstOrDefault();
            int i = 0;
        

            for (DateTime date = _db_Order_Detail.From_Date; date.Date <= _db_Order_Detail.To_Date; date = date.AddDays(1))
            {
                bool workstatus = false;
                for(i=0;i< _list_Emp_Status.Count;i++)
                { 
                for (DateTime listdate = _list_Emp_Status[i].From_Date; listdate.Date <= _list_Emp_Status[i].To_Date; listdate = listdate.AddDays(1))
                {
                    if (date == listdate)
                    {
                            EmpInfo _EmpInfo = new EmpInfo();
                            workstatus = true;
                            _EmpInfo.DateReport = listdate; //listdate.ToString("dd-MM-yy");
                        _EmpInfo.Empname = _db_Employee.Emp_Name;
                        _EmpInfo.Bank_Id = _db_Employee.Bank_Id;
                            _EmpInfo.Designation = _db_Employee.db_Designation.Name;
                        _EmpInfo.Cell = _list_Emp_Status[i].db_Cell.Name;
                        _EmpInfo.Work = _list_Emp_Status[i].db_Work_Category.Name;
                        _EmpInfo.StartTime = _list_Emp_Status[i].Start_time;
                        _EmpInfo.EndTime = _list_Emp_Status[i].End_time;
                            if(_EmpInfo.EndTime< _EmpInfo.StartTime)

                            {
                                TimeSpan timeex = new TimeSpan(24, 0, 0);
                                _list_Emp_Status[i].End_time= _list_Emp_Status[i].End_time + timeex;
                                _EmpInfo.Total = (_list_Emp_Status[i].Start_time - _list_Emp_Status[i].End_time).Duration();
                            }
                            else
                                _EmpInfo.Total = (_list_Emp_Status[i].Start_time - _list_Emp_Status[i].End_time).Duration();
                          
                        _list_Emp_Result.Add(_EmpInfo);


                    };
                    

                  }
                }

                if (workstatus != true)
                {
                    List<db_calendar> _list_Calender = (from item in dbContext.db_calendar
                                                        where item.HoliDay_Date == date && item.Status==true

                                                        select item).ToList();
                    if (_list_Calender.Count == 0 && date.DayOfWeek.ToString()!="Friday" && date.DayOfWeek.ToString() != "Saturday")
                    {
                        EmpInfo _EmpInfo = new EmpInfo();
                        _EmpInfo.DateReport = date;
                        _EmpInfo.Date = date.ToString("dd-MM-yy");
                        _EmpInfo.Empname = _db_Employee.Emp_Name;
                        _EmpInfo.Bank_Id = _db_Employee.Bank_Id;
                        _EmpInfo.Designation = _db_Employee.db_Designation.Name;
                        db_Employee _db_Employee_name = dbContext.db_Employee.Where(x => x.Emp_Id == _db_Order_Detail.Emp_Id).SingleOrDefault();
                        _EmpInfo.Cell = _db_Employee_name.db_Cell.Name;
                        db_Work_Category _db_Work_Category = dbContext.db_Work_Category.Where(x => x.Name == "Regular").SingleOrDefault();
                        _EmpInfo.Work = _db_Work_Category.Name;
                        _EmpInfo.StartTime = _db_Work_Category.Start_Time;
                        _EmpInfo.EndTime = _db_Work_Category.End_Time;
                        if (_EmpInfo.EndTime < _EmpInfo.StartTime)

                        {
                            TimeSpan timeex = new TimeSpan(24, 0, 0);
                            _EmpInfo.EndTime = _EmpInfo.EndTime + timeex;
                            _EmpInfo.Total = (_EmpInfo.StartTime - _EmpInfo.EndTime).Duration();
                        }
                        else
                            _EmpInfo.Total = (_EmpInfo.StartTime - _EmpInfo.EndTime).Duration();
                        _list_Emp_Result.Add(_EmpInfo);

                    }

                }
            }


            
            return _list_Emp_Result;
        }



        public List<EmpInfo> Get_CellInfoStatus(db_Order_Detail _db_Order_Detail)
        {

            List<EmpInfo> _list_Emp_Result = new List<EmpInfo>();
            List<db_Employee> _list_db_Employee = new List<db_Employee>();
            _list_db_Employee = (from emp in dbContext.db_Employee
                                 where emp.Cell_Id == _db_Order_Detail.Cell_Id
                                 select emp).ToList();
            int i = 0;

            for (DateTime date = _db_Order_Detail.From_Date; date.Date <= _db_Order_Detail.To_Date; date = date.AddDays(1))
            {
                bool workstatus = false;
                foreach (db_Employee _db_Employee in _list_db_Employee)
                {
                    db_Order_Detail _list_Emp_Status = (from list in dbContext.db_Order_Detail
                                                        where list.Emp_Id == _db_Employee.Emp_Id
                                                        & list.From_Date <= date & list.To_Date >= date
                                                        select list).SingleOrDefault();
                    if (_list_Emp_Status != null)
                    {
                        EmpInfo _EmpInfo = new EmpInfo();
                        workstatus = true;
                        _EmpInfo.DateReport = date;
                        _EmpInfo.Date = date.ToString("dd-MM-yy");
                        _EmpInfo.Empname = _db_Employee.Bn_Emp_Name;
                        _EmpInfo.Bank_Id = _db_Employee.Bank_Id;
                        _EmpInfo.Designation = _db_Employee.db_Designation.Bn_Name;
                        //_EmpInfo.Designation = "test";
                        _EmpInfo.Cell = _db_Employee.db_Cell.Name;
                        _EmpInfo.Work = _db_Employee.db_Work_Category.Name;
                        _EmpInfo.StartTime = _list_Emp_Status.Start_time;
                        _EmpInfo.EndTime = _list_Emp_Status.End_time;
                        if (_EmpInfo.EndTime < _EmpInfo.StartTime)
                        {
                            TimeSpan timeex = new TimeSpan(24, 0, 0);
                            _list_Emp_Status.End_time = _list_Emp_Status.End_time + timeex;
                            _EmpInfo.Total = (_list_Emp_Status.Start_time - _list_Emp_Status.End_time).Duration();
                        }
                        else
                            _EmpInfo.Total = (_list_Emp_Status.Start_time - _list_Emp_Status.End_time).Duration();
                        _list_Emp_Result.Add(_EmpInfo);
                    }
                    else
                    {

                        List<db_calendar> _list_Calender = (from item in dbContext.db_calendar
                                                            where item.HoliDay_Date == date && item.Status == true
                                                            select item).ToList();
                        if (_list_Calender.Count == 0 && date.DayOfWeek.ToString() != "Friday" && date.DayOfWeek.ToString() != "Saturday")
                        {
                            EmpInfo _EmpInfo = new EmpInfo();
                            _EmpInfo.DateReport = date;
                            _EmpInfo.Date = date.ToString("dd-MM-yy");
                            _EmpInfo.Empname = _db_Employee.Bn_Emp_Name;
                            _EmpInfo.Bank_Id = _db_Employee.Bank_Id;
                            _EmpInfo.Designation = _db_Employee.db_Designation.Bn_Name;
                            _EmpInfo.Cell = _db_Employee.db_Cell.Name;
                            db_Work_Category _db_Work_Category = dbContext.db_Work_Category.Where(x => x.Name == "Regular").SingleOrDefault();
                            _EmpInfo.Work = _db_Work_Category.Name;
                            _EmpInfo.StartTime = _db_Work_Category.Start_Time;
                            _EmpInfo.EndTime = _db_Work_Category.End_Time;
                            if (_EmpInfo.EndTime < _EmpInfo.StartTime)

                            {
                                TimeSpan timeex = new TimeSpan(24, 0, 0);
                                _EmpInfo.EndTime = _EmpInfo.EndTime + timeex;
                                _EmpInfo.Total = (_EmpInfo.StartTime - _EmpInfo.EndTime).Duration();
                            }
                            else
                                _EmpInfo.Total = (_EmpInfo.StartTime - _EmpInfo.EndTime).Duration();
                            _list_Emp_Result.Add(_EmpInfo);

                        }
                    }
                }
            }
            return _list_Emp_Result;
        }

        public List<db_Order_Detail> Get_EmpInfoCell(db_Order_Detail _db_Order_Detail)
        {
            List<db_Order_Detail> _list_db_Order_Detail = new List<db_Order_Detail>();

            _list_db_Order_Detail = (from list in dbContext.db_Order_Detail

                                     where list.Cell_Id == _db_Order_Detail.Cell_Id
                                     & list.From_Date >= _db_Order_Detail.From_Date & list.To_Date <= _db_Order_Detail.To_Date
                                     orderby list.From_Date ascending
                                     select list).ToList();


            return _list_db_Order_Detail;

        }
        public List<db_Order_Detail> Get_EmpInfo_Cell_Cal(long cell_id, DateTime date_time)
        {
            List<db_Order_Detail> _list_db_Order_Detail = new List<db_Order_Detail>();

            _list_db_Order_Detail = (from list in dbContext.db_Order_Detail

                                     where list.Cell_Id == cell_id
                                     & list.From_Date >= date_time & list.To_Date <= date_time
                                     orderby list.From_Date ascending
                                     select list).ToList();


            return _list_db_Order_Detail;

        }

        public List<db_Employee> Get_EmpCellWise(db_Employee _db_Employee)
        {
            List<db_Employee> _list_db_Employee = new List<db_Employee>();

            _list_db_Employee = (from list in dbContext.db_Employee
                                 where list.Cell_Id == _db_Employee.Cell_Id
                                 select list).ToList();


            return _list_db_Employee;

        }

        public List<db_User> GetUser_Info(string username,string  pass)

        {

            List<db_User> list = (from user in dbContext.db_User
                                  orderby user.User_Name ascending
                                  where user.User_Name == username && user.Password== pass && user.Status ==true
                                  select user).ToList();

            return list;
        }
        public List<db_Order> Get_Order_list(db_Order_Detail _db_Order)
        {

            List<db_Order> list = (from order in dbContext.db_Order
                                   
                                  where order.Date >= _db_Order.From_Date & order.Date <= _db_Order.To_Date
                                  orderby order.Date descending
                                   select order).ToList();

            return list;
        }
        public List<db_Order_Detail> Get_Detail_id(long Id, long Order_Id, long Work_Id)
        {
            List<db_Order_Detail> list = (from item in dbContext.db_Order_Detail
                                          where item.Emp_Id == Id && item.Order_Id == Order_Id && item.Work_Id == Work_Id
                                          orderby item.From_Date
                                          select item).ToList();
            return list;
        }
        public bool Order_Status_Up(db_Order_Detail _db_Order_detail,string Status)
        {
            bool result=true;
           
                    List<db_Order_Detail> list = (from item in dbContext.db_Order_Detail
                                                  where item.Emp_Id == _db_Order_detail.Emp_Id && item.Order_Id == _db_Order_detail.Order_Id && item.Work_Id == _db_Order_detail.Work_Id
                                                  select item).ToList();
                    foreach (db_Order_Detail db_Order_Detail_temp in list)
                    {
                      db_Order_Detail_temp.Status = Status;

                    }
               dbContext.SaveChanges();

            return result;
        }


        public List<orderinfo> Get_all_order(int Id, int year)
        {
            List<orderinfo> list = (from item in dbContext.db_Order_Detail
                                          join detail in dbContext.db_Work_Category on item.Work_Id equals detail.Work_Id
                                          where (item.Emp_Id == Id && (item.From_Date.Year == year || item.To_Date.Year == year))
                                          orderby detail.work_type
                                          select new orderinfo {
                                              EmpId =item.Emp_Id,
                                              From_Date = item.From_Date,
                                              To_Date = item.To_Date,
                                              Work_Id = item.Work_Id,
                                              NTravel = detail.Travel.Value,
                                              NEntertainment = detail.Entertainment.Value,
                                              work_type = detail.work_type.Value
                                          }).ToList();
            return list;
        }




        public List<orderinfo> Get_OrderList(long Id)
        {

            var list = (from item in dbContext.db_Order
                        join detail in dbContext.db_Order_Detail on item.Order_Id equals detail.Order_Id
                        where item.Order_Id == Id
                        orderby detail.From_Date
                        select new orderinfo
                        {
                            Empname = detail.db_Employee.Emp_Name,
                            EmpId = detail.db_Employee.Emp_Id,
                            Designation = detail.db_Employee.db_Designation.Name,
                            DesignationId = detail.db_Employee.db_Designation.Designation_Id,
                            Serial = detail.db_Employee.db_Designation.Serial.Value,
                            promotion_Date = detail.db_Employee.promotion_Date.Value,
                            Bank_Id = detail.db_Employee.Bank_Id,
                            Cellname = detail.db_Cell.Name,
                            Cellid = detail.db_Cell.Cell_Id,
                            Workname = detail.db_Work_Category.Name,
                            Work_Id = detail.db_Work_Category.Work_Id,
                            Travel = detail.db_Work_Category.Travel.Value,
                            Entertainment = detail.db_Work_Category.Entertainment.Value,
                            detailid = detail.Order_Detail_Id,
                            From_Date = detail.From_Date,
                            To_Date = detail.To_Date,
                            Start_time = detail.Start_time,
                            End_time = detail.End_time,
                            Order_No = item.Order_No,
                            Order_Id = item.Order_Id,
                            OrderDate = item.Date,
                            Detail = item.Detail,
                            Refno = item.Refno,
                            Status = detail.Status,
                            Details = detail.Details


                        }).ToList();

            return list;
        }
        public List<orderinfo> Get_OrderList(long Id,long userId)
        {

            var list = (from item in dbContext.db_Order
                                  join detail in dbContext.db_Order_Detail on item.Order_Id equals detail.Order_Id
                                  where item.Order_Id == Id && item.User_Id== userId
                        select new orderinfo{
                                      Empname= detail.db_Employee.Emp_Name,
                                      EmpId = detail.db_Employee.Emp_Id,
                                      Designation = detail.db_Employee.db_Designation.Name,
                                      Bank_Id = detail.db_Employee.Bank_Id,
                                      Cellname = detail.db_Cell.Name,
                                      Cellid = detail.db_Cell.Cell_Id,
                                      Workname = detail.db_Work_Category.Name,
                                      Work_Id = detail.db_Work_Category.Work_Id,
                                      Travel = detail.db_Work_Category.Travel.Value,
                                      Entertainment = detail.db_Work_Category.Entertainment.Value,
                                      detailid = detail.Order_Detail_Id,
                                      From_Date = detail.From_Date,
                                      To_Date = detail.To_Date,
                                      Start_time = detail.Start_time,
                                      End_time = detail.End_time,
                                      Order_No = item.Order_No,
                                      Order_Id= item.Order_Id,
                                      OrderDate = item.Date,
                                      Detail = item.Detail,
                                      Refno = item.Refno,
                                      Status= detail.Status,
                                      Details = detail.Details


                        }).ToList();

            return list;
        }

        public string SaveEdit(List<orderinfo> data, long userid)
        {
            string result = "Save Successfully";
            int check;
            long Order_Id=data[0].Order_Id;
            //;
            using (TransactionScope tran = new TransactionScope())
            {
                foreach(var item in data)
                {
                    if (item.From_Dateall == null)
                    {
                        List<db_Order_Detail> listdel = new List<db_Order_Detail>();
                        
                        listdel = dbContext.db_Order_Detail.Where(x => x.Order_Id == item.Order_Id && x.Emp_Id == item.EmpId).ToList();
                        foreach(db_Order_Detail _db_Order_Detail in listdel)
                        {
                            dbContext.db_Order_Detail.Remove(_db_Order_Detail);
                            dbContext.SaveChanges();
                        }
                        
                    }

                    else if (item.detailid == 0)
                    {
                        db_Order_Detail _db_Order_Detail = new db_Order_Detail();
                        _db_Order_Detail.Order_Id = item.Order_Id;
                        _db_Order_Detail.Emp_Id = item.EmpId;
                        _db_Order_Detail.Work_Id = item.Work_Id;
                        _db_Order_Detail.Cell_Id = item.Cellid;
                        _db_Order_Detail.From_Date = item.From_Date;
                        _db_Order_Detail.To_Date = item.To_Date;
                        _db_Order_Detail.User_Id = userid;
                        _db_Order_Detail.Date = DateTime.Now;
                        _db_Order_Detail.Start_time = item.Start_time;
                        _db_Order_Detail.End_time = item.End_time;
                        _db_Order_Detail.From_Dateall = item.From_Dateall;
                        _db_Order_Detail.Details = item.Details;
                        check =WorkCheck(_db_Order_Detail);
                        if(check!=0)
                        {
                            tran.Dispose();
                            db_Employee _db_Employee = dbContext.db_Employee.Where(x => x.Emp_Id == item.EmpId).SingleOrDefault() ;
                            result=_db_Employee.Emp_Name + " is already another task (Order ID:" + item.Order_Id + ")";
                            return result;
                        }
                       string[] From_Datealllist = _db_Order_Detail.From_Dateall.Split(',');

                        foreach (string datea in From_Datealllist)
                        {
                            _db_Order_Detail.From_Date = DateTime.Parse(datea);
                            _db_Order_Detail.To_Date = DateTime.Parse(datea);
                            dbContext.db_Order_Detail.Add(_db_Order_Detail);
                            dbContext.SaveChanges();
                        }


                    }
                    else if (item.EmpId != 0)
                    {
                        List<db_Order_Detail> listdel = new List<db_Order_Detail>();
                        List<db_Order_Detail> listdel_REM = new List<db_Order_Detail>();
                        string[] From_Datealllist = item.From_Dateall.Split(',');
                        listdel_REM = dbContext.db_Order_Detail.Where(x => x.Order_Id == item.Order_Id && x.Emp_Id == item.EmpId ).ToList();
                        foreach(db_Order_Detail datarem in listdel_REM)
                        {
                            
                            bool dtchk = false;
                            foreach (string datea in From_Datealllist)
                            {
                                if (DateTime.Parse(datea) == datarem.From_Date)
                                {
                                    dtchk = true;
                                    break;
                                }
                                

                            }

                            if (dtchk == false)
                            {
                                dbContext.db_Order_Detail.Remove(datarem);
                                dbContext.SaveChanges();
                            }

                        }

                        foreach (string datea in From_Datealllist)
                        {
                            DateTime date = DateTime.Parse(datea);
                            listdel = dbContext.db_Order_Detail.Where(x => x.Order_Id == item.Order_Id && x.Emp_Id == item.EmpId && x.From_Date== date).ToList();

                            db_Order_Detail _db_Order_Detail = null;
                                Dll_Component _Dll_Component = new Dll_Component();
                            if (listdel.Count > 0)
                                 _db_Order_Detail = listdel.FirstOrDefault();
                            else
                               _db_Order_Detail = new db_Order_Detail();

                               _db_Order_Detail.Order_Id = item.Order_Id;
                                _db_Order_Detail.Emp_Id = item.EmpId;
                                _db_Order_Detail.Work_Id = item.Work_Id;
                                _db_Order_Detail.Cell_Id = item.Cellid;
                                _db_Order_Detail.From_Date = DateTime.Parse(datea);
                                _db_Order_Detail.To_Date = DateTime.Parse(datea);
                                _db_Order_Detail.User_Id = userid;
                                _db_Order_Detail.Date = DateTime.Now;
                                db_Work_Category _db_Work_Category = _Dll_Component.Get_Work_Cat_ID(_db_Order_Detail.Work_Id);
                                _db_Order_Detail.Start_time = _db_Work_Category.Start_Time;
                                _db_Order_Detail.End_time = _db_Work_Category.End_Time;
                                _db_Order_Detail.From_Dateall = datea;
                                 _db_Order_Detail.Details = item.Details;
                            if (listdel.Count > 0)
                                _db_Order_Detail.Order_Detail_Id = listdel[0].Order_Detail_Id;
                            else
                                dbContext.db_Order_Detail.Add(_db_Order_Detail);
                            check = WorkCheckEdit(_db_Order_Detail);
                                if (check != 0)
                                {
                                    tran.Dispose();
                                    db_Employee _db_Employee = dbContext.db_Employee.Where(x => x.Emp_Id == item.EmpId).SingleOrDefault();
                                    result = _db_Employee.Emp_Name + " is already another task (Order ID:" + item.Order_Id + ")";
                                    return result;
                                }
                               

                            dbContext.SaveChanges();
                            
                            
                        }

                        ///////////////////////////////
                        
                    }

                }

                List<db_Order_Detail> temlist =(from itemor in dbContext.db_Order_Detail
                                                where itemor.Order_Id== Order_Id
                                                select itemor).ToList();
                db_Order _db_Order = dbContext.db_Order.Where(x => x.Order_Id == Order_Id).SingleOrDefault();
                if (temlist.Count>0)
                {
                    _db_Order.Detail = data[0].Detail;
                    dbContext.SaveChanges();
                }

                else
                {
                    dbContext.db_Order.Remove(_db_Order);
                    dbContext.SaveChanges();
                  
                }


                tran.Complete();
            }

            return result;
        }


        public int WorkCheck(db_Order_Detail _db_Order_Detail)
        {
            int workstatus = 0;
            string[] From_Datealllist = null;
            List<db_Order_Detail> _listdb_Order_Detail = null;
            {
                _listdb_Order_Detail = (from item in dbContext.db_Order_Detail
                                      where  item.Emp_Id== _db_Order_Detail.Emp_Id
                                        select item).ToList();
                db_Work_Category _db_Work_Category = dbContext.db_Work_Category.Where(x => x.Work_Id == _db_Order_Detail.Work_Id).SingleOrDefault();
                int i = 0;
                if (_listdb_Order_Detail.Count > 0)
                {
                    From_Datealllist = _db_Order_Detail.From_Dateall.Split(',');

                    foreach (string datea in From_Datealllist)
                    {


                        DateTime date = DateTime.Parse(datea);
                        if (_db_Work_Category.work_type == 2)
                        {
                            if (date.DayOfWeek.ToString() == "Friday" || date.DayOfWeek.ToString() == "Saturday")
                                workstatus = 2;
                            db_calendar _db_calendar = dbContext.db_calendar.Where(x => x.HoliDay_Date == date).SingleOrDefault();
                            if (_db_calendar != null)
                                workstatus = 2;

                        }

                        for (i = 0; i < _listdb_Order_Detail.Count; i++)
                        {

                            for (DateTime listdate = _listdb_Order_Detail[i].From_Date; listdate.Date <= _listdb_Order_Detail[i].To_Date; listdate = listdate.AddDays(1))
                            {

                                if (date == listdate)
                                {
                                    long Work_Id_l = _listdb_Order_Detail[i].Work_Id;
                                    db_Work_Category _db_Work_Category_list = dbContext.db_Work_Category.Where(x => x.Work_Id == Work_Id_l).SingleOrDefault();
                                    if (_db_Order_Detail.Work_Id == _listdb_Order_Detail[i].Work_Id)
                                        workstatus = 1;
                                    else if (_db_Work_Category.work_type == 2 && (_db_Work_Category_list.work_type == 1 || _db_Work_Category_list.work_type == 3 || _db_Work_Category_list.work_type == 4))
                                        workstatus = 3;
                                    else if (_db_Work_Category_list.work_type == 2 && (_db_Work_Category.work_type == 1 || _db_Work_Category.work_type == 3 || _db_Work_Category.work_type == 4))
                                        workstatus = 3;
                                    else if (_db_Order_Detail.Start_time >= _listdb_Order_Detail[i].Start_time && _db_Order_Detail.Start_time <= _listdb_Order_Detail[i].End_time)
                                        workstatus = 4;

                                    else if (_db_Order_Detail.End_time >= _listdb_Order_Detail[i].Start_time && _db_Order_Detail.End_time <= _listdb_Order_Detail[i].End_time)
                                        workstatus = 4;
                                    return workstatus;
                                }

                            }
                        }
                    }
                }
                    return workstatus;
                
            }


        }
        public int WorkCheckEdit(db_Order_Detail _db_Order_Detail)
        {
            int workstatus = 0;
            string[] From_Datealllist = null;
            List<db_Order_Detail> _listdb_Order_Detail = null;
            {
                _listdb_Order_Detail = (from item in dbContext.db_Order_Detail
                                        where item.Order_Detail_Id != _db_Order_Detail.Order_Detail_Id && item.Work_Id == _db_Order_Detail.Work_Id && item.Emp_Id == _db_Order_Detail.Emp_Id
                                        select item).ToList();
                db_Work_Category _db_Work_Category = dbContext.db_Work_Category.Where(x => x.Work_Id == _db_Order_Detail.Work_Id).SingleOrDefault();
                int i = 0;
                if (_listdb_Order_Detail.Count > 0)
                    From_Datealllist = _db_Order_Detail.From_Dateall.Split(',');
                if (From_Datealllist!=null)
                    foreach (string datea in From_Datealllist)
                {
                    DateTime date = DateTime.Parse(datea);


                    if (_db_Work_Category.work_type == 2)
                        {
                            if (date.DayOfWeek.ToString() == "Friday" || date.DayOfWeek.ToString() == "Saturday")
                                workstatus = 2;
                            db_calendar _db_calendar = dbContext.db_calendar.Where(x => x.HoliDay_Date == date).SingleOrDefault();
                            if (_db_calendar != null)
                                workstatus = 2;

                        }

                        for (i = 0; i < _listdb_Order_Detail.Count; i++)
                    {
                        for (DateTime listdate = _listdb_Order_Detail[i].From_Date; listdate.Date <= _listdb_Order_Detail[i].To_Date; listdate = listdate.AddDays(1))
                        {
                            if (date == listdate)
                            {
                                    long Work_Id_l = _listdb_Order_Detail[i].Work_Id;
                                    db_Work_Category _db_Work_Category_list = dbContext.db_Work_Category.Where(x => x.Work_Id == Work_Id_l).SingleOrDefault();
                                    if (_db_Order_Detail.Work_Id == _listdb_Order_Detail[i].Work_Id)
                                        workstatus = 1;
                                    else if (_db_Work_Category.work_type == 2 && (_db_Work_Category_list.work_type == 1 || _db_Work_Category_list.work_type == 3 || _db_Work_Category_list.work_type == 4))
                                        workstatus = 3;
                                    else if (_db_Work_Category_list.work_type == 2 && (_db_Work_Category.work_type == 1 || _db_Work_Category.work_type == 3 || _db_Work_Category.work_type == 4))
                                        workstatus = 3;
                                    else if ((_db_Order_Detail.Start_time >= _listdb_Order_Detail[i].Start_time && _db_Order_Detail.Start_time <= _listdb_Order_Detail[i].End_time) || (_db_Order_Detail.End_time >= _listdb_Order_Detail[i].Start_time && _db_Order_Detail.End_time <= _listdb_Order_Detail[i].End_time))
                                        workstatus = 4;
                                    return workstatus;
                                    
                                }

                        }
                    }
                }
                return workstatus;

            }


        }

        public bool NameCheckBankId(db_Employee _db_Employee)
        {
            List<db_Employee> _listdb_Employee = null;
            List<db_Employee> _listdb_Employeetem = null;
            if (_db_Employee.Emp_Id != 0)
            {
                _listdb_Employee = (from item in dbContext.db_Employee
                                    where item.Bank_Id == _db_Employee.Bank_Id && item.Emp_Id == _db_Employee.Emp_Id
                                    select item).ToList();
                if (_listdb_Employee.Count == 0)
                {
                    _listdb_Employeetem = (from item in dbContext.db_Employee
                                           where item.Bank_Id == _db_Employee.Bank_Id
                                           select item).ToList();

                }


                if (_listdb_Employee.Count > 0)
                    return true;
                else if (_listdb_Employeetem.Count > 0)
                    return false;
                else
                    return true;
            }
            else {
                _listdb_Employee = (from item in dbContext.db_Employee
                                    where item.Bank_Id == _db_Employee.Bank_Id
                                    select item).ToList();


                if (_listdb_Employee.Count > 0)
                    return false;
                else
                    return true;
            }


        }


    }
}
