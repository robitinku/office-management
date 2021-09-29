using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Office_Bll;
using Office_Dll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Office_Management_System.Controllers
{
    public class ReportController : Controller
    {
        Bll_Employee _Bll_Employee = new Bll_Employee();
        Bll_Component _Bll_Component = new Bll_Component();
        Dll_Employee _Dll_Employee = new Dll_Employee();
        List<db_Employee> _listEmp = new List<db_Employee>();
        List<db_Cell> _listCell = new List<db_Cell>();
        List<db_calendar> _listHoliday = new List<db_calendar>();
        public ActionResult AllEmp()
        {

           if (Session["userId"] != null)
            {
                _listEmp = _Dll_Employee.GetEmp();
                return View(_listEmp);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult AllCell()
        {
            if (Session["userId"] != null)
            {
                _listCell = _Dll_Employee.Get_Cell_list();
                return View(_listCell);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult CellWEmp()
        {
            _listCell = _Bll_Employee.Get_Cell_list();
            ViewBag._listCell = new SelectList(_listCell, "Cell_Id", "Name");
            if (Session["userId"] != null)
            {
               return View();
            }
            else
                return RedirectToAction("Index", "Login");

        }
        public ActionResult CellWEmpList(db_Employee _db_Employee)
        {
            List<db_Employee> _list_db_Employee = new List<db_Employee>();
            _list_db_Employee = _Bll_Employee.Get_EmpCellWise(_db_Employee);
            //string empname = _list_db_Order_Detail[0].db_Employee.Emp_Name;
            string tr = "";
            int i = 1;
            foreach (db_Employee _EmpInfo in _list_db_Employee)
            {
                tr += "<tr>";
                tr += "<td>" + i+ "</td>";
                tr += "<td>" + _EmpInfo.Emp_Name + "</td>";
                tr += "<td>" + _EmpInfo.db_Designation.Name + "</td>";
                tr += "<td>" + _EmpInfo.Email + "</td>";
                tr += "<td>" + _EmpInfo.Mobile + "</td>";
                tr += "<td>";
                if (_EmpInfo.Status == true)
                {
                    tr += "Active";
                }
                else {
                    tr += "Inactive";
                }
                tr += "</td>";

                tr += "</tr>";
                i += 1;
            }
            return Json(tr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Holiday()
        {
            if (Session["userId"] != null)
            {
                _listHoliday = _Bll_Component.Get_All_list_calendar();
                return View(_listHoliday);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult WorkCalender()
        {
            _listCell = _Bll_Employee.Get_Cell_list();
            ViewBag._listCell = new SelectList(_listCell, "Cell_Id", "Name");
            if (Session["userId"] != null)
            {
                
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

         public ActionResult EmpStatusCell(db_Order_Detail _db_Order_Detail)
        {
            List<db_Order_Detail> _list_db_Order_Detail = new List<db_Order_Detail>();
            string tr="";
           
         
            for (DateTime dateTime = _db_Order_Detail.From_Date; dateTime <= _db_Order_Detail.To_Date;
                     dateTime += TimeSpan.FromDays(1))
            {
                string date_now = dateTime.ToString("D");

                tr += "<div style='min-height:30px; border:1px solid #808080; margin-bottom:8px;'>";

                tr += "<div style='background-color:#8b8bca;color:#ffffff; font-weight:bold; padding:5px;'>" + date_now +"</div>";
               

                _list_db_Order_Detail = _Bll_Employee.Get_EmpInfo_Cell_Cal(_db_Order_Detail.Cell_Id, dateTime);

                foreach (db_Order_Detail _EmpInfo in _list_db_Order_Detail)
                {

                    tr += "<div style='border-bottom:1px solid #808080; padding:5px;'>";
                    tr += "<div style='float:left; width:300px;'>" + _EmpInfo.db_Employee.Emp_Name + "</div>";
                    
                    tr += "<div style='float:left; width:300px;'>" + _EmpInfo.db_Work_Category.Name + "</div>";

                    tr += "<div style='clear: both'></div></div>";
                }
                tr += "<div style='clear: both'></div></div>";
            }
            

            return Json(tr, JsonRequestBehavior.AllowGet);
        }

            }
}
