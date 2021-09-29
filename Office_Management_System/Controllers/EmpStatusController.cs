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
    public class EmpStatusController : Controller
    {
        // GET: EmpStatus
        
        Bll_Employee _Bll_Employee = new Bll_Employee();
        Bll_Component _Bll_Component = new Bll_Component();
        List<db_Cell> _listCell = new List<db_Cell>();
        public object _list_Emp_list { get; set; }
        public object Data { get; set; }

        public ActionResult Index()
        {
            if (Session["userId"] != null)

                return View();
            else
                return RedirectToAction("Index", "Login");
            
        }

        public ActionResult Empcell()
        {
            if (Session["userId"] != null)
            {
                _listCell = _Bll_Employee.Get_Cell_list();
                ViewBag._listCell = new SelectList(_listCell, "Cell_Id", "Name");
                return View();
            }
            else
                return RedirectToAction("Index", "Login");

        }

        public ActionResult EmpStatusCellPrint(db_Order_Detail _db_Order_Detail)
        {
            if (_db_Order_Detail.Emp_Id == 0)
            {

                _db_Order_Detail.Emp_Id = long.Parse(Session["Emp_Id"].ToString());
            }
            /*List<EmpInfo> _list_Emp_Result = new List<EmpInfo>();
            _list_Emp_Result = _Bll_Employee.Get_EmpInfoStatus(_db_Order_Detail);
            _list_Emp_Result = _list_Emp_Result.OrderBy(x => x.DateReport).ThenBy(y => y.StartTime).ToList();
            */
            List<EmpInfo> _list_Emp_Result = new List<EmpInfo>();
            _list_Emp_Result = _Bll_Employee.Get_EmpInfoCell(_db_Order_Detail);
            //_list_Emp_Result = _list_Emp_Result.OrderBy(x => x.Date).ThenBy(y => y.StartTime).ToList();
            _list_Emp_Result = _list_Emp_Result.ToList();
            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath("~/Report/CellWise.rpt");
            rptH.Load();
            rptH.SetDataSource(_list_Emp_Result);
            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Position = 0;
            if (Session["userId"] != null)
            {


                Response.AppendHeader("Content-Disposition", "inline; filename=CellWise.pdf");
                return File(stream, "application/pdf");

            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult EmpStatusCell(db_Order_Detail _db_Order_Detail)
        {
          
            if (_db_Order_Detail.Emp_Id == 0)
            {

                _db_Order_Detail.Emp_Id = long.Parse(Session["Emp_Id"].ToString());
            }
            List<EmpInfo> _list_Emp_Result = new List<EmpInfo>();
            _list_Emp_Result = _Bll_Employee.Get_EmpInfoCell(_db_Order_Detail);
            //_list_Emp_Result = _list_Emp_Result.OrderBy(x => x.Date).ThenBy(y => y.StartTime).ToList();
            _list_Emp_Result = _list_Emp_Result.ToList();
            string tr = "";
            TimeSpan Totalhour = new TimeSpan();
            foreach (EmpInfo _EmpInfo in _list_Emp_Result)
            {
                tr += "<tr>";
                tr += "<td>" + _EmpInfo.DateReport.ToString("dd-MM-yy")+ "</td>";
                tr += "<td>" + _EmpInfo.Bank_Id + "</td>";
                tr += "<td>" + _EmpInfo.Empname + "</td>";
                tr += "<td>" + _EmpInfo.Designation + "</td>";
                tr += "<td>" + _EmpInfo.Work + "</td>";
                tr += "<td>" + _EmpInfo.StartTime + "</td>";
                tr += "<td>" + _EmpInfo.EndTime + "</td>";
                tr += "<td>" + _EmpInfo.Total + "</td>";
                tr += "</tr>";
                Totalhour += _EmpInfo.Total;

            }
            tr += "<tr>";
            tr += "<td></td>";
            tr += "<td></td>";
            tr += "<td></td>";
            tr += "<td></td>";
            tr += "<td></td>";
            tr += "<td></td>";
            tr += "<td>Total Hour:</td>";
            tr += "<td>" + Totalhour.TotalHours + "</td>";
            tr += "</tr>";
            return Json(tr, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EmpInfoDetail()
        {

            _list_Emp_list = _Bll_Employee.Get_Emp_list();



            Data = new
            {
                _list_Emp_list = _list_Emp_list,

            };


            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmpInfoStatusPrint(db_Order_Detail _db_Order_Detail)
        {
            if(_db_Order_Detail.Emp_Id==0)
            {

                _db_Order_Detail.Emp_Id = long.Parse(Session["Emp_Id"].ToString());
            }
            List<EmpInfo> _list_Emp_Result = new List<EmpInfo>();
            _list_Emp_Result = _Bll_Employee.Get_EmpInfoStatus(_db_Order_Detail);
            _list_Emp_Result = _list_Emp_Result.OrderBy(x => x.DateReport).ThenBy(y => y.StartTime).ToList();
            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath("~/Report/EmpStatus.rpt");
            rptH.Load();
            rptH.SetDataSource(_list_Emp_Result);
            Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Position = 0;
            if (Session["userId"] != null)
            {


                Response.AppendHeader("Content-Disposition", "inline; filename=Order.pdf");
                return File(stream, "application/pdf");

            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult EmpInfoStatus(db_Order_Detail _db_Order_Detail)
        {

            if (_db_Order_Detail.Emp_Id == 0)
            {

                _db_Order_Detail.Emp_Id = long.Parse(Session["Emp_Id"].ToString());
            }
            List<EmpInfo> _list_Emp_Result = new List<EmpInfo>();
            _list_Emp_Result = _Bll_Employee.Get_EmpInfoStatus(_db_Order_Detail);
            _list_Emp_Result = _list_Emp_Result.OrderBy(x => x.DateReport).ThenBy(y => y.StartTime).ToList();
            string tr = "";
            TimeSpan Totalhour = new TimeSpan();
            foreach (EmpInfo _EmpInfo in _list_Emp_Result)
            {
                tr += "<tr>";
                tr += "<td>"+ _EmpInfo.DateReport.ToString("dd-MM-yy") + "</td>";
                tr += "<td>" + _EmpInfo.Cell + "</td>";
                tr += "<td>" + _EmpInfo.Work + "</td>";
                tr += "<td>" + _EmpInfo.StartTime + "</td>";
                tr += "<td>" + _EmpInfo.EndTime + "</td>";
                tr += "<td>" + _EmpInfo.Total + "</td>";
                tr += "</tr>";
                Totalhour+= _EmpInfo.Total;

            }
            tr += "<tr>";
            tr += "<td></td>";
            tr += "<td></td>";
            tr += "<td></td>";
            tr += "<td></td>";
            tr += "<td>Total Hour:</td>";
            tr += "<td>" + Totalhour.TotalHours+ "</td>";
            tr += "</tr>";
            return Json(tr, JsonRequestBehavior.AllowGet);
        }
    }
}