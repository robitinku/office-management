using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Office_Dll;
using Office_Bll;
using PagedList;
using CrystalDecisions.CrystalReports.Engine;
//using Office_Report_Contorl;
using System.IO;
using CrystalDecisions.Shared;

namespace Office_Management_System.Controllers
{
    public class EmpController : Controller
    {
        //reportload _reportload = new reportload();
        Bll_Employee _Bll_Employee = new Bll_Employee();
        db_Employee _db_Employee = new db_Employee();
        List<db_Cell> _listCell = new List<db_Cell>();
        List<db_Department> _listDepartmen = new List<db_Department>();
        List<db_Designation> _list_Designation = new List<db_Designation>();
        
        List<db_Work_Category> _list_work_category = new List<db_Work_Category>();
        List<db_Job_Cate> _list_Job_Cate = new List<db_Job_Cate>();
        List<db_Employee> Employee_list = new List<db_Employee>();
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            int pageSize = 100;


            if (page == null)
            {
                page = 1;
            }


            ViewBag.CurrentFilter = searchString;

            Employee_list = _Bll_Employee.GetEmpAll(page.Value, searchString);




            int pageNumber = (page ?? 1);

            if (Session["userId"] != null)

                return View(Employee_list.ToPagedList(pageNumber, pageSize));
            else
                return RedirectToAction("Index", "Login");




        }

        public ActionResult Index2(string sortOrder, string currentFilter, string searchString, int? page)
        {

            int pageSize = 1000;


            if (page == null)
            {
                page = 1;
            }


            ViewBag.CurrentFilter = searchString;

            Employee_list = _Bll_Employee.GetEmpAll2(page.Value, searchString);




            int pageNumber = (page ?? 1);

            if (Session["userId"] != null)

                return View(Employee_list.ToPagedList(pageNumber, pageSize));
            else
                return RedirectToAction("Index", "Login");




        }

        public ActionResult contact_list()
        {
            List<EmpInfo> _list_Emp_Result = new List<EmpInfo>();
            Employee_list = _Bll_Employee.GetEmpAllpdf();

            foreach (db_Employee _db_Employee in Employee_list)
            {
               //sl Bank ID, NameBn, Bndes, cell, email, mobile
                EmpInfo _EmpInfo = new EmpInfo();
                _EmpInfo.Empname = _db_Employee.Bn_Emp_Name;
                _EmpInfo.Bank_Id = _db_Employee.Bank_Id;
                _EmpInfo.Designation = _db_Employee.db_Designation.Bn_Name;
               // _EmpInfo.Serial = _db_Employee.db_Designation.Serial;
                _EmpInfo.Cell = _db_Employee.db_Cell.Name;
                _EmpInfo.Email = _db_Employee.Email;
                _EmpInfo.Mobile = _db_Employee.Mobile;
                _list_Emp_Result.Add(_EmpInfo);
                
            }


            //_list_Emp_Result = _list_Emp_Result.OrderBy(y => y.Serial).ToList();
            /*var _list_Emp_Result1 = from s in _list_Emp_Result
                               orderby s.Serial
                            select s;*/

            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath("~/Report/ContactList.rpt");
            rptH.Load();
            rptH.SetDataSource(_list_Emp_Result);
            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Position = 0;
           // if (Session["userId"] != null)
           // {


                Response.AppendHeader("Content-Disposition", "inline; filename=contact_list.pdf");
                return File(stream, "application/pdf");

           // }
           // else
              //  return RedirectToAction("Index", "Login");
        }

        public ActionResult AddData(db_Employee data)
        {
            data.User_Id = long.Parse(Session["userId"].ToString());
            data.Date = DateTime.Now;
            bool result;
           
            result = _Bll_Employee.NameCheckBankId(data);
            if (result == true)
                _Bll_Employee.Add(data);
          
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        


        public ActionResult AddEmp()
        {
            _listCell = _Bll_Employee.Get_Cell_list();
           ViewBag._listCell = new SelectList(_listCell, "Cell_Id", "Name");

            _list_work_category = _Bll_Employee.Get_Work_list();
            ViewBag._list_work_category = new SelectList(_list_work_category, "Work_Id", "Name");

            _list_Designation = _Bll_Employee.Get_Designation_list_Active();
            ViewBag._list_Designation = new SelectList(_list_Designation, "Designation_Id", "Name");

            _list_Job_Cate = _Bll_Employee.Get_Job_Cate();
            ViewBag._list_Job_Cate = new SelectList(_list_Job_Cate, "Category_Id", "Name");

            _listDepartmen = _Bll_Employee.Get_Dep_all();
            ViewBag._listDepartmen = new SelectList(_listDepartmen, "Dep_id", "Name");
            if (Session["userId"] != null)

                return View();
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult View(long id)
        {

            _db_Employee = _Bll_Employee.Get_Emp_By_ID(id);
            db_image _db_img = _db_Employee.db_image.Where(x => x.Emp_Id == id).FirstOrDefault();
            if (_db_img != null)
                _db_Employee.Imagetext = System.Text.Encoding.ASCII.GetString(_db_img.image);
            if (Session["userId"] != null)

                return View(_db_Employee);
            else
                return RedirectToAction("Index", "Login");
            
        }

        public ActionResult Edit(long id)
        {

            _db_Employee = _Bll_Employee.Get_Emp_By_ID(id);
            _listCell = _Bll_Employee.Get_Cell_list();
            ViewBag._listCell = new SelectList(_listCell, "Cell_Id", "Name", _db_Employee.Cell_Id);

            _list_work_category = _Bll_Employee.Get_Work_list();
            ViewBag._list_work_category = new SelectList(_list_work_category, "Work_Id", "Name", _db_Employee.Work_Id);

            _list_Job_Cate = _Bll_Employee.Get_Job_Cate();
            ViewBag._list_Job_Cate = new SelectList(_list_Job_Cate, "Category_Id", "Name", _db_Employee.Category_Id);

            _list_Designation = _Bll_Employee.Get_Designation_list_Active();
            ViewBag._list_Designation = new SelectList(_list_Designation, "Designation_Id", "Name", _db_Employee.Designation_Id);

            _listDepartmen = _Bll_Employee.Get_Dep_all();
            ViewBag._listDepartmen = new SelectList(_listDepartmen, "Dep_id", "Name", _db_Employee.Present_Posting);
            db_image _db_img = _db_Employee.db_image.Where(x => x.Emp_Id == id).FirstOrDefault();
            if (_db_img!=null)
                _db_Employee.Imagetext = System.Text.Encoding.ASCII.GetString(_db_img.image);
            if (Session["userId"] != null)

                return View(_db_Employee);
            else
                return RedirectToAction("Index", "Login");
            
        }

        public ActionResult EditData(db_Employee data)
        {
            data.User_Id = long.Parse(Session["userId"].ToString());
            data.Date = DateTime.Now;
            bool result;

            result = _Bll_Employee.NameCheckBankId(data);
              if (result == true)
                _Bll_Employee.EditData(data);
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}