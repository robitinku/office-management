using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office_Dll;

namespace Office_Bll
{
    public class Bll_Employee
    {
        
        Dll_Employee _Dll_Employee = new Dll_Employee();
        List<db_Employee> _listEmp = new List<db_Employee>();
        List<db_Cell> _listCell = new List<db_Cell>();
        public object _listEmpAct { get; set; }
        public object _list_Emp_list { get; set; }
        public object Employeeinfo { get; private set; }

        List<db_Job_Cate> _list_Job_Cate = new List<db_Job_Cate>();
        List<db_Work_Category> _list_work_category = new List<db_Work_Category>();
        db_Employee _db_Employee = new db_Employee();
        db_Order _db_Order = new db_Order();

        public List<db_Employee> Get_EmpCellWise(db_Employee _db_Employee)
        {
            List<db_Employee> _listEmp = _Dll_Employee.Get_EmpCellWise(_db_Employee);

            return _listEmp;
        }

        public List<db_User> GetUser_Info(string username, string pass)
        {
            List<db_User> list= _Dll_Employee.GetUser_Info(username, pass);

            return list;
        }
        public bool Add (db_Employee db_Employee)
        {
            _Dll_Employee.Create(db_Employee);

            return true;
        }
        public List<db_Order_Detail> Get_EmpInfo_Cell_Cal(long cell_id, DateTime date_time)
        {
            List<db_Order_Detail> _listEmp = _Dll_Employee.Get_EmpInfo_Cell_Cal(cell_id, date_time);

            return _listEmp;
        }

        public List<db_Work_Category> Get_Work_list()
        {
            _list_work_category = _Dll_Employee.Get_Work_list();

            return _list_work_category;
        }
        public List<db_Work_Category> Get_Work_list_Order(long Id)
        {
            _list_work_category = _Dll_Employee.Get_Work_list_Order(Id);

            return _list_work_category;
        }
        public List<orderinfo> Get_OrderList(long Id, long userId)
        {
            List<orderinfo> _list_orderinfo = _Dll_Employee.Get_OrderList(Id, userId);

            return _list_orderinfo;
        }
        public List<orderinfo> Get_OrderList(long Id)
        {
            List<orderinfo> _list_orderinfo = _Dll_Employee.Get_OrderList(Id);

            return _list_orderinfo;
        }
        public List<db_Order_Detail> Get_Detail_id(long Id, long Order_Id, long Work_Id)
        {
            List<db_Order_Detail> list = _Dll_Employee.Get_Detail_id(Id,Order_Id, Work_Id);

            return list;
        }

        public List<orderinfo> Get_all_order(int Emp_Id, int year)
        {
            List<orderinfo> list = _Dll_Employee.Get_all_order(Emp_Id, year);

            return list;
        }

        public object Get_Emp_list()
        {
            _list_Emp_list = _Dll_Employee.Get_Emp_list();

            return _list_Emp_list;
        }
        public List<db_Cell> Get_Cell_list()
        {
            _listCell=_Dll_Employee.Get_Cell_list();

            return _listCell;
        }

        public List<db_Designation> Get_Designation_list_Active()
        {
            List<db_Designation> _listdb_Designation = _Dll_Employee.Get_Designation_list_Active();

            return _listdb_Designation;
        }
        public List<db_Job_Cate> Get_Job_Cate()
        {
            _list_Job_Cate = _Dll_Employee.Get_Job_Cate();

            return _list_Job_Cate;
        }
        public List<db_Employee> GetEmpAll(int page,string searchString)
        {
            _listEmp = _Dll_Employee.GetEmpAll(page, searchString);

            return _listEmp;
        }
        public List<db_Employee> GetEmpAll2(int page, string searchString)
        {
            _listEmp = _Dll_Employee.GetEmpAll2(page, searchString);

            return _listEmp;
        }
        public List<db_Employee> GetEmpAllpdf()
        {
            _listEmp = _Dll_Employee.GetEmpAllpdf();

            return _listEmp;
        }
        public int GetEmpAll( string searchString)
        {
            int count;
            count = _Dll_Employee.GetEmpAll( searchString);

            return count;
        }
       

               public List<db_Department> Get_Dep_all()
        {

            List<db_Department> _listdb_Department = _Dll_Employee.Get_Dep_all();

            return _listdb_Department;
        }
        public db_Employee Get_Emp_By_ID(long id)
        {
            _db_Employee = _Dll_Employee.Get_Emp_By_ID(id);

            return _db_Employee;
        }

        public db_Order Get_Order_By_ID(long id)
        {
            _db_Order = _Dll_Employee.Get_Order_By_ID(id);

            return _db_Order;
        }

        public object Get_Emp_By_ID_Json(long id)
        {
            Employeeinfo = _Dll_Employee.Get_Emp_By_ID_Json(id);

            return Employeeinfo;
        }

        public bool EditData(db_Employee db_Employee)
        {
            _Dll_Employee.EditData(db_Employee);

            return true;
        }
        public string Save_order_detail(List<db_Order_Detail> data, long userid)
        {
            string result=_Dll_Employee.Save_order_detail(data,  userid);

            return result;
        }
        public List<EmpInfo> Get_EmpInfoStatus(db_Order_Detail _db_Order_Detail)
        {
            List<EmpInfo> _listEmp = _Dll_Employee.Get_EmpInfoStatus(_db_Order_Detail);

            return _listEmp;
        }

        public List<EmpInfo> Get_EmpInfoCell(db_Order_Detail _db_Order_Detail)
        {
            //Get_CellInfoStatus
            //List<db_Order_Detail> _listEmp = _Dll_Employee.Get_EmpInfoCell(_db_Order_Detail);
            List<EmpInfo> _listEmp = _Dll_Employee.Get_CellInfoStatus(_db_Order_Detail);
            return _listEmp;
        }

        public string SaveEdit(List<orderinfo> data, long userid)
        {
            string result= _Dll_Employee.SaveEdit(data, userid);

            return result;
        }
        public List<db_Order> Get_Order_list(db_Order_Detail _db_Order)
        {
            List<db_Order> _listorder = _Dll_Employee.Get_Order_list(_db_Order);

            return _listorder;
        }
        public bool Order_Status_Up(db_Order_Detail _db_Order_detail,string Status)
        {
            bool result;
            result = _Dll_Employee.Order_Status_Up(_db_Order_detail, Status);

            return result;
        }
        
        public bool NameCheckBankId(db_Employee _db_Employee)
        {
            bool result = _Dll_Employee.NameCheckBankId(_db_Employee);

            return result;
        }

    }
}
