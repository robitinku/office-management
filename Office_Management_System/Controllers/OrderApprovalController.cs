using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Office_Dll;
using Office_Bll;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using System.Threading;
using System.Globalization;

namespace Office_Management_System.Controllers
{
    public class OrderApprovalController : Controller
    {
        private Office_ManagementEntities db = new Office_ManagementEntities();
        Bll_Employee _Bll_Employee = new Bll_Employee();
        Bll_Component _Bll_Component = new Bll_Component();

        db_Employee _db_Employee = new db_Employee();
        List<db_Cell> _listCell = new List<db_Cell>();
        List<db_Work_Category> _list_work_category = new List<db_Work_Category>();
        List<db_Job_Cate> _list_Job_Cate = new List<db_Job_Cate>();
        List<db_Employee> Employee_list = new List<db_Employee>();
        public object _list_Emp_list { get; set; }
        // GET: OrderApproval
        public ActionResult Index()
        {

            if (Session["userId"] != null && ( Session["Category"].ToString()== "Verifier" || Session["Category"].ToString() == "Forwarder"))

                return View();
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult OrderInfoDetail(db_Order_Detail _db_Order_Detail)
        {
            List<db_Order> _listOrder = _Bll_Employee.Get_Order_list(_db_Order_Detail);
            long userid = long.Parse(Session["userId"].ToString());
         
                //_listOrder = _listOrder.Where(x => x.User_Id == userid).ToList();
            string tr = "";

            foreach (db_Order _db_Order in _listOrder)
            {

                tr += "<tr>";
                tr += "<td>" + _db_Order.Date.ToString("dd-MM-yy") + "</td>";
                tr += "<td>" + _db_Order.Order_No + "</td>";
                tr += "<td>" + _db_Order.db_User.db_Employee.db_Cell.Name + "</td>";
                tr += "<td class='options - width'>";

               
                //tr += "<a class='print-1 info-tooltip' runat='server' href ='" + @Url.Action("Report") + "/" + _db_Order.Order_Id + "' title='Report' target='_blank'> </a>";
                tr += "<a class='Bill-1 info-tooltip' runat='server' href ='" + @Url.Action("OrderApproval") + "/" + _db_Order.Order_Id + "' title='Order Approval' target='_blank'> </a>";
                tr += "</td>";
                tr += "</tr>";



            }

            return Json(tr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Verify(db_Order_Detail data)
        {

            bool result;
            string Status = "verify";
            result =_Bll_Employee.Order_Status_Up(data, Status);

             return Json(result, JsonRequestBehavior.AllowGet); 
        }
        public ActionResult Forward(db_Order_Detail data)
        {

            bool result;
            string Status = "forward";
            result = _Bll_Employee.Order_Status_Up(data, Status);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderApproval(long? Id, long? Work_Id)
        {



            bool check = false;
            long userId = long.Parse(Session["userId"].ToString());
            List<orderinfo> _list_orderinfo;
            if (Session["userId"] != null && ( Session["Category"].ToString()== "Verifier" || Session["Category"].ToString() == "Forwarder"))
                _list_orderinfo = _Bll_Employee.Get_OrderList(Id.Value);
            //else
                //_list_orderinfo = _Bll_Employee.Get_OrderList(Id.Value, userId);
           // if (_list_orderinfo.Count > 0 || Session["Category"].ToString() == "Admin")
                check = true;

            if (Session["userId"] != null && (Session["Category"].ToString() == "Verifier" || Session["Category"].ToString() == "Forwarder") && check == true)
            {
                if (Work_Id == null)
                    Work_Id = 0;
                List<orderinfo> list = BillData(Id.Value, Work_Id.Value);
                return View(list);
            }
            else
                return RedirectToAction("Index", "Login");

        }


        public List<orderinfo> BillData(long Id, long Work_Id)
        {

            List<orderinfo> _list_orderinfo = _Bll_Employee.Get_OrderList(Id);

            _list_work_category = _Bll_Employee.Get_Work_list_Order(Id);
            db_work_text _db_work_text;
            if (Work_Id == 0)
            {
                ViewBag._list_work_category = new SelectList(_list_work_category, "Work_Id", "Name", _list_orderinfo[0].Work_Id);
                _db_work_text = _Bll_Component.Work_Edit_text(_list_orderinfo[0].Work_Id);
                _list_orderinfo = _list_orderinfo.Where(x => x.Work_Id == _list_orderinfo[0].Work_Id).ToList();
            }

            else
            {
                ViewBag._list_work_category = new SelectList(_list_work_category, "Work_Id", "Name", Work_Id);
                _db_work_text = _Bll_Component.Work_Edit_text(Work_Id);
                _list_orderinfo = _list_orderinfo.Where(x => x.Work_Id == Work_Id).ToList();
            }


            ViewBag.OrderDetail = _db_work_text.db_Work_Category.Detail;
            ViewBag.Subject = _db_work_text.Subject;
            ViewBag.DetailHeader = _db_work_text.DetailHeader;
            ViewBag.DetailFottor = _db_work_text.DetailFottor;
            if (_list_orderinfo.Count > 0)
                ViewBag.RefNo = _list_orderinfo[0].Refno;
            else
                ViewBag.RefNo = "";
            ViewBag.Id = Id;

            List<orderinfo> list = (from item in _list_orderinfo

                                    select new orderinfo
                                    {
                                        Empname = item.Empname,
                                        Designation = item.Designation,
                                        DesignationId = item.DesignationId,
                                        Serial = item.Serial,
                                        EmpId = item.EmpId,
                                        From_Date = item.From_Date,
                                        To_Date = item.To_Date,
                                        promotion_Date = item.promotion_Date,
                                        OrderDateAll = item.OrderDateAll,
                                        OrderDateReport = item.OrderDate.ToString("dd-MM-yy"),
                                        Entertainment = item.Entertainment,
                                        Travel = item.Travel,
                                        detailid = item.detailid,
                                        Order_Id = item.Order_Id,
                                        Status= item.Status
                                    }).ToList();
            List<orderinfo> tempviewlist = new List<orderinfo>();
            List<long> Emp_ID = list.OrderBy(y => y.Serial).ThenBy(y => y.promotion_Date).GroupBy(y => y.EmpId).Select(x => x.Key).ToList();
            foreach (var emp in Emp_ID)
            {
                var templist = list.Where(x => x.EmpId == emp).ToList();

                orderinfo _orderinfotemp = new orderinfo();
                _orderinfotemp.day = 0;
                foreach (var item in templist)
                {
                    int day = 0;
                    for (DateTime listdate = item.From_Date; listdate.Date <= item.To_Date; listdate = listdate.AddDays(1))
                    {
                        ++day;

                        Thread.CurrentThread.CurrentCulture = new CultureInfo("bn-BD");
                        item.OrderDateAll += listdate.ToShortDateString();
                        //if (item.To_Date != listdate)
                        item.OrderDateAll += ",";
                    }
                    //item.day = day;

                    _orderinfotemp.day = _orderinfotemp.day + day;
                    _orderinfotemp.OrderDateAll = _orderinfotemp.OrderDateAll + item.OrderDateAll;
                }
                _orderinfotemp.Empname = ToupperCaseWord(templist[0].Empname);
                _orderinfotemp.EmpId = emp;
                _orderinfotemp.Order_Id = Id;
                _orderinfotemp.Work_Id = Work_Id;
                _orderinfotemp.Status = templist[0].Status;

                _orderinfotemp.Designation = ToupperCaseWord(templist[0].Designation);
                _orderinfotemp.Entertainment = templist[0].Entertainment;
                if (_orderinfotemp.Designation == ToupperCaseWord("DEPUTY GENERAL MANAGER") || _orderinfotemp.Designation == ToupperCaseWord("ASSISTANT GENERAL MANAGER"))
                    _orderinfotemp.Travel = 0;
                else
                    _orderinfotemp.Travel = templist[0].Travel;
                _orderinfotemp.Ttravel = _orderinfotemp.day * _orderinfotemp.Travel;

                _orderinfotemp.TEntertainment = _orderinfotemp.day * templist[0].Entertainment;
                _orderinfotemp.Total = _orderinfotemp.TEntertainment + _orderinfotemp.Ttravel;

                tempviewlist.Add(_orderinfotemp);
            }

            orderinfo _orderinfo = new orderinfo();
            _orderinfo.TEntertainment = tempviewlist.Sum(x => x.TEntertainment);
            _orderinfo.Ttravel = tempviewlist.Sum(x => x.Ttravel);
            _orderinfo.Total = tempviewlist.Sum(x => x.Total);
            //tempviewlist.Add(_orderinfo);
            return tempviewlist;
            //}

            //else
            //{
            //    return _list_orderinfo;

            //}
        }
        public string ToupperCaseWord(string name)
        {
            name = name.Trim().ToLower();

            string[] temp = name.Split(' ');
            name = "";
            foreach (var item in temp)
            {
                char[] array = item.ToCharArray();
                array[0] = char.ToUpper(array[0]);
                name += new string(array) + " ";
            }

            return name;
        }
    }
}