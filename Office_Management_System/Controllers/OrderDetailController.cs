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
    public class OrderDetailController : Controller
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


        public ActionResult Index(DateTime? SearchDate)
        {
            if (SearchDate == null)
            {
                SearchDate = DateTime.Now.Date;
            }


            if (Session["userId"] != null && Session["Category"].ToString() != "Member" && Session["Category"].ToString() != "Verifier" && Session["Category"].ToString() != "Forwarder")

                return View();
            else
                return RedirectToAction("Index", "Login");

        }


        public ActionResult search()
        {

            if (Session["userId"] != null && Session["Category"].ToString() != "Member")

                return View();
            else
                return RedirectToAction("Index", "Login");

        }


        public ActionResult OrderInfoDetail(db_Order_Detail _db_Order_Detail)
        {
            List<db_Order> _listOrder = _Bll_Employee.Get_Order_list(_db_Order_Detail);
            long userid = long.Parse(Session["userId"].ToString());
            if (Session["Category"].ToString() == "In-charge")
                _listOrder = _listOrder.Where(x => x.User_Id == userid).ToList();
            string tr = "";

            foreach (db_Order _db_Order in _listOrder)
            {

                tr += "<tr>";
                tr += "<td>" + _db_Order.Date.ToString("dd-MM-yy") + "</td>";
                tr += "<td>" + _db_Order.Order_No + "</td>";
                tr += "<td class='options - width'>";

                tr += "<a class='icon-1 info-tooltip'  href='" + @Url.Action("Edit") + "/" + _db_Order.Order_Id + "' title='Edit'> </a>";
                //tr += "<a class='print-1 info-tooltip' runat='server' href ='" + @Url.Action("Report") + "/" + _db_Order.Order_Id + "' title='Report' target='_blank'> </a>";
                tr += "<a class='Bill-1 info-tooltip' runat='server' href ='" + @Url.Action("Bill") + "/" + _db_Order.Order_Id + "' title='Report' target='_blank'> </a>";
                tr += "</td>";
                tr += "</tr>";



            }

            return Json(tr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderReport(int[] IdList, long Order_Id, long Work_Id, string Subject, string DetailHeader, string DetailFottor, string OrderDetail, string Emp_Name, string Designation, string RefNo, string Signatory, string SignatoryD, string CCCopy)
        {

            if (IdList != null)
            {
                List<orderinfo> _list_orderinfo = new List<orderinfo>();
                _list_orderinfo = ReportDataList(IdList, Order_Id, Work_Id, Subject, DetailHeader, DetailFottor, OrderDetail, "Order", Emp_Name, Designation, RefNo, Signatory, SignatoryD, CCCopy);
                ReportClass rptH = new ReportClass();
                rptH.FileName = Server.MapPath("~/Report/OrderResult.rpt");

                rptH.Load();
                rptH.SetDataSource(_list_orderinfo);
                Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Position = 0;
                if (Session["userId"] != null && Session["Category"].ToString() != "Member")
                {


                    Response.AppendHeader("Content-Disposition", "inline; filename=Order.pdf");
                    return File(stream, "application/pdf");

                }
                else
                    return RedirectToAction("Index", "Login");
            }
            else
            {
                if (Session["userId"] != null && Session["Category"].ToString() != "Member")
                    return View("Report");
                else
                    return RedirectToAction("Index", "Login");
            }
        }

        public List<orderinfo> ReportDataList(int[] IdList, long Order_Id, long Work_Id, string Subject, string DetailHeader, string DetailFottor, string OrderDetail, string category, string Emp_Name, string Designation, string RefNo, string Signatory, string SignatoryD, string CCCopy)
        {
            int i = 1;
            List<orderinfo> _list_orderinfo = new List<orderinfo>();
            if (IdList != null)
                foreach (var item in IdList)
                {
                    List<db_Order_Detail> list = _Bll_Employee.Get_Detail_id(item, Order_Id, Work_Id);
                    orderinfo _orderinfotemp = new orderinfo();
                    _orderinfotemp.Cellname = "";
                    int count = 0;
                    foreach (var detail in list)
                    {
                        int day = 0;
                        count++;
                        orderinfo _orderinfo = new orderinfo
                        {
                            From_Date = detail.From_Date,
                            To_Date = detail.To_Date,
                        };
                        for (DateTime listdate = _orderinfo.From_Date; listdate.Date <= _orderinfo.To_Date; listdate = listdate.AddDays(1))
                        {
                            ++day;

                            _orderinfo.OrderDateAll += listdate.ToString("dd-MM-yy");
                            if (_orderinfo.To_Date == listdate && count == list.Count())
                                _orderinfo.OrderDateAll += "";
                            else
                                _orderinfo.OrderDateAll += ", ";
                            _orderinfotemp.Details = detail.Details;
                        }
                        _orderinfotemp.day = _orderinfotemp.day + day;
                        _orderinfotemp.OrderDateAll = _orderinfotemp.OrderDateAll + _orderinfo.OrderDateAll;
                        string tem = detail.db_Cell.Name;
                        bool tempi = _orderinfotemp.Cellname.Contains(tem);
                        if (tempi == false)
                            _orderinfotemp.Cellname += tem + ", ";
                    }

                    _orderinfotemp.OrderDateAll = _Bll_Component.converttobangla(_orderinfotemp.OrderDateAll);


                    _orderinfotemp.Cellname = _orderinfotemp.Cellname.Remove(_orderinfotemp.Cellname.Length - 2);


                    _orderinfotemp.Empname = ToupperCaseWord(list[0].db_Employee.Emp_Name);
                    _orderinfotemp.Empname_bn = (list[0].db_Employee.Bn_Emp_Name);

                    _orderinfotemp.Designation = ToupperCaseWord(list[0].db_Employee.db_Designation.Name);
                    _orderinfotemp.Designation_bn = list[0].db_Employee.db_Designation.Bn_Name;

                    _orderinfotemp.Entertainment = list[0].db_Work_Category.Entertainment.Value;
                    _orderinfotemp.strEntertainment = _Bll_Component.converttobangla(_orderinfotemp.Entertainment.ToString());

                    if (_orderinfotemp.Designation == ToupperCaseWord("DEPUTY GENERAL MANAGER") || _orderinfotemp.Designation == ToupperCaseWord("ASSISTANT GENERAL MANAGER"))
                        _orderinfotemp.Travel = 0;
                    else
                        _orderinfotemp.Travel = list[0].db_Work_Category.Travel.Value;

                    _orderinfotemp.strTravel = _Bll_Component.converttobangla(_orderinfotemp.Travel.ToString());


                    _orderinfotemp.Ttravel = _orderinfotemp.day * _orderinfotemp.Travel;
                    _orderinfotemp.strTtravel = _Bll_Component.converttobangla(_orderinfotemp.Ttravel.ToString());

                    _orderinfotemp.TEntertainment = _orderinfotemp.day * _orderinfotemp.Entertainment;
                    _orderinfotemp.strTEntertainment = _Bll_Component.converttobangla(_orderinfotemp.TEntertainment.ToString());

                    _orderinfotemp.Total = _orderinfotemp.TEntertainment + _orderinfotemp.Ttravel;
                    _orderinfotemp.strTotal = _Bll_Component.converttobangla(_orderinfotemp.Total.ToString());
                    _orderinfotemp.strDay = _Bll_Component.converttobangla(_orderinfotemp.day.ToString());

                    CultureInfo ci = new CultureInfo("bn-BD");
                    if (category == "Bill")

                        _orderinfotemp.OrderDateReport = DateTime.Now.ToString("dd-MM-yyyy", ci);
                    else
                        _orderinfotemp.OrderDateReport = list[0].db_Order.Date.ToString("dd-MM-yyyy", ci);
                    _orderinfotemp.OrderDateReport = _Bll_Component.converttobangla(_orderinfotemp.OrderDateReport);
                    // _orderinfo.day = day;
                    // _orderinfo.TEntertainment = day * _orderinfo.Entertainment;
                    //_orderinfo.Total = _orderinfo.TEntertainment + _orderinfo.Ttravel;
                    _orderinfotemp.sl = _Bll_Component.converttobangla(i.ToString());
                    _orderinfotemp.Reportername = Emp_Name == null ? "-" : Emp_Name;
                    _orderinfotemp.ReporterDesignation = Designation;
                    _orderinfotemp.Subject = Subject; //Subject;
                    _orderinfotemp.DetailHeader = DetailHeader; //DetailHeader;
                    _orderinfotemp.DetailFottor = DetailFottor; //DetailFottor;
                    _orderinfotemp.Refno = RefNo;
                    _orderinfotemp.CCCopy = CCCopy;
                    _orderinfotemp.Signatory = Signatory; //DetailFottor;
                    _orderinfotemp.SignatoryD = SignatoryD;
                    _orderinfotemp.Detail = OrderDetail;
                    ///set nullable
                    _orderinfotemp.Bank_Id = "-";
                    _orderinfotemp.Order_No = "-";
                    _orderinfotemp.NEntertainment = 0;
                    _orderinfotemp.NTravel = 0;
                    _orderinfotemp.Serial = 0;
                    _orderinfotemp.work_type = 0;
                    _orderinfotemp.Workname = "-";
                    _orderinfotemp.promotion_Date = new DateTime(2017, 01, 18);

                    // _orderinfotemp.list = new List<long>(0);
                    _list_orderinfo.Add(_orderinfotemp);
                    i++;
                }


            return _list_orderinfo;
        }



        public List<orderinfo> selfassesmentDataList(int emp_id, int year)
        {

            int i = 1;


            List<orderinfo> _list_orderinfo = new List<orderinfo>();
            List<orderinfo> list = _Bll_Employee.Get_all_order(emp_id, year);
            int count = 0;
            foreach (var detail in list)
            {
                orderinfo _orderinfotemp = new orderinfo();
                int day = 0;
                count++;
                orderinfo _orderinfo = new orderinfo
                {
                    From_Date = detail.From_Date,
                    To_Date = detail.To_Date,
                };
                for (DateTime listdate = _orderinfo.From_Date; listdate.Date <= _orderinfo.To_Date; listdate = listdate.AddDays(1))
                {
                    ++day;

                    _orderinfo.OrderDateAll += listdate.ToString("dd-MM-yyyy");
                    if (_orderinfo.To_Date == listdate && count == list.Count())
                        _orderinfo.OrderDateAll += "";
                    else
                        _orderinfo.OrderDateAll += ", ";
                }
                _orderinfotemp.day = _orderinfotemp.day + day;
                _orderinfotemp.OrderDateAll = _orderinfotemp.OrderDateAll + _orderinfo.OrderDateAll;

                _orderinfotemp.Entertainment = detail.NEntertainment;
                if (_orderinfotemp.Designation == ToupperCaseWord("DEPUTY GENERAL MANAGER") || _orderinfotemp.Designation == ToupperCaseWord("ASSISTANT GENERAL MANAGER"))
                    _orderinfotemp.Travel = 0;
                else
                    _orderinfotemp.Travel = detail.NTravel;


                _orderinfotemp.Ttravel = _orderinfotemp.day * _orderinfotemp.Travel;
                _orderinfotemp.TEntertainment = _orderinfotemp.day * _orderinfotemp.Entertainment;
                _orderinfotemp.Total = _orderinfotemp.TEntertainment + _orderinfotemp.Ttravel;
                _orderinfotemp.Work_Id = detail.Work_Id;
                _orderinfotemp.work_type = detail.work_type;

                _list_orderinfo.Add(_orderinfotemp);
                i++;
            }


            List<orderinfo> allTotal = _list_orderinfo
                    .GroupBy(v => v.work_type)
                    .Select(g => new orderinfo
                    {
                        work_type = g.First().work_type,
                        day = g.Sum(v => v.day),
                        Total = g.Sum(v => v.Total)
                    }).ToList();

            // int[] myArray = new int[] { 1, 3, 5, 7, 9 };
            return allTotal;
        }




        public ActionResult BillReport(int[] IdList, long Order_Id, long Work_Id, string Subject, string DetailHeader, string DetailFottor, string OrderDetail, string Emp_Name, string Designation, string RefNo, string Signatory, string SignatoryD, string CCCopy)
        {

            if (IdList != null)
            {
                List<orderinfo> _list_orderinfo = new List<orderinfo>();
                _list_orderinfo = ReportDataList(IdList, Order_Id, Work_Id, Subject, DetailHeader, DetailFottor, OrderDetail, "Bill", Emp_Name, Designation, RefNo, Signatory, SignatoryD, CCCopy);

                decimal gTTravel = 0;
                decimal gTEntertainment = 0;
                decimal gTotal = 0;
                foreach (var value in _list_orderinfo)
                {
                    gTTravel = gTTravel + value.Ttravel;
                    gTEntertainment = gTEntertainment + value.TEntertainment;
                    gTotal = gTotal + value.Total;
                }
                string gTTravelBn = "";
                string gTEntertainmentBn = "";
                string gTotalBn = "";
                gTTravelBn = _Bll_Component.converttobangla(gTTravel.ToString());
                gTEntertainmentBn = _Bll_Component.converttobangla(gTEntertainment.ToString());
                gTotalBn = _Bll_Component.converttobangla(gTotal.ToString());

                ReportClass rptH = new ReportClass();
                // ReportDocument rptH = new ReportDocument();

                if (Work_Id == 11)
                {
                    rptH.FileName = Server.MapPath("~/Report/NightBillReport.rpt");
                }
                else {
                    rptH.FileName = Server.MapPath("~/Report/BillReport.rpt");
                }


                //rptH.Clone();
                rptH.Load();
                try
                {
                    rptH.SetDataSource(_list_orderinfo);
                }
                catch (NullReferenceException e)
                {
                    //eventListener.HandleEvent(Severity.Informational, line.GetType().Name, String.Format("Could not find the customer corresponding to the taxId '{0}' Applicant address will not be imported.", new TaxId(line.TaxId).Masked));
                    //eventListener.HandleEvent(Severity.Informational, line.GetType().Name, e.Message);
                }

                rptH.SetParameterValue("gTTravelBn", gTTravelBn);
                rptH.SetParameterValue("gTEntertainmentBn", gTEntertainmentBn);
                rptH.SetParameterValue("gTotalBn", gTotalBn);
                Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);


                Response.AppendHeader("Content-Disposition", "inline; filename=Bill.pdf");
                return File(stream, "application/pdf");
            }
            else
            {
                if (Session["userId"] != null && Session["Category"].ToString() != "Member")
                    return View("Report");
                else
                    return RedirectToAction("Index", "Login");
            }

        }

        public ActionResult Selfassesmentform()
        {
            if (Session["userId"] != null)

                return View();
            else
                return RedirectToAction("Index", "Login");

        }

        // public ActionResult selfassesment(long Emp_Id, long Work_Id, int year)
        public ActionResult selfassesment(db_Order_Detail _db_Order_Detail)
        {
            //List<orderinfo> _list_orderinfo = new List<orderinfo>();
            //_list_orderinfo = ReportDataList(Emp_Id, Work_Id, year);
            int Emp_id = (int)_db_Order_Detail.Emp_Id;
            int year = (int)_db_Order_Detail.Cell_Id;
            List<orderinfo> _list_orderinfo = selfassesmentDataList(Emp_id, year);
            //List<orderinfo> _list_orderinfo = selfassesmentDataList(51, 2016);
            var Employeeinfo = _Bll_Employee.Get_Emp_By_ID(Emp_id);
            if (_list_orderinfo != null)
            {
                // var _list_orderinfo1 = new string[3] { "Empname"=>"Matt", "Cell"=>"Joanne", "Work"=>"Robert" }; 

                ReportClass rptH = new ReportClass();
                rptH.FileName = Server.MapPath("~/Report/annualreport.rpt");
                rptH.Load();
                //rptH.SetDataSource(_list_orderinfo);
                //_list_orderinfo = _list_orderinfo.ToArray();
                rptH.SetParameterValue("name", Employeeinfo.Emp_Name);
                rptH.SetParameterValue("desig", Employeeinfo.db_Designation.Name);
                rptH.SetParameterValue("cell", Employeeinfo.db_Cell.Name);
                rptH.SetParameterValue("duration", year);

                if (_list_orderinfo.Count > 0)
                {
                    rptH.SetParameterValue("holiday", _list_orderinfo[0].Total);
                    rptH.SetParameterValue("hdays", _list_orderinfo[0].day);
                }
                else {
                    rptH.SetParameterValue("holiday", 0);
                    rptH.SetParameterValue("hdays", 0);
                }
                if (_list_orderinfo.Count > 1)
                {
                    rptH.SetParameterValue("latesetting", _list_orderinfo[1].Total);
                    rptH.SetParameterValue("ldays", _list_orderinfo[1].day);
                }
                else {
                    rptH.SetParameterValue("latesetting", 0);
                    rptH.SetParameterValue("ldays", 0);
                }
                if (_list_orderinfo.Count > 2)
                {
                    rptH.SetParameterValue("night", _list_orderinfo[2].Total);
                    rptH.SetParameterValue("ndays", _list_orderinfo[2].day);
                }
                else {
                    rptH.SetParameterValue("night", 0);
                    rptH.SetParameterValue("ndays", 0);
                }
                //  int var = 0;
                Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
                Response.AppendHeader("Content-Disposition", "inline; filename=annualreport.pdf");
                return File(stream, "application/pdf");
            }
            else
            {
                if (Session["userId"] != null && Session["Category"].ToString() != "Member")
                    return View("selfassesment");
                else
                    return RedirectToAction("Index", "Login");
            }
            //return RedirectToAction("Index", "Login");
        }




        public List<orderinfo> BillData(long Id, long Work_Id)
        {

            List<orderinfo> _list_orderinfo = _Bll_Employee.Get_OrderList(Id);

            _list_orderinfo = _list_orderinfo.Where(x => x.Status == "verify" || x.Status==null).ToList();
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
                                        Order_Id = item.Order_Id
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
            tempviewlist.Add(_orderinfo);
            return tempviewlist;
            //}

            //else
            //{
            //    return _list_orderinfo;

            //}
        }


        public List<orderinfo> OrderData(long Id, long Work_Id)
        {

            List<orderinfo> _list_orderinfo = _Bll_Employee.Get_OrderList(Id);

            /*  _list_work_category = _Bll_Employee.Get_Work_list();
              db_work_text _db_work_text;
              if (Work_Id == 0)
              {
                  ViewBag._list_work_category = new SelectList(_list_work_category, "Work_Id", "Name", _list_orderinfo[0].Work_Id);
                  _db_work_text = _Bll_Component.Work_Edit_text(_list_orderinfo[0].Work_Id);
                 // _list_orderinfo = _list_orderinfo.Where(x => x.Work_Id == _list_orderinfo[0].Work_Id).ToList();
              }

              else
              {
                  ViewBag._list_work_category = new SelectList(_list_work_category, "Work_Id", "Name", Work_Id);
                  _db_work_text = _Bll_Component.Work_Edit_text(Work_Id);
                  //_list_orderinfo = _list_orderinfo.Where(x => x.Work_Id == Work_Id).ToList();
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
                                          EmpId = item.EmpId,
                                          From_Date = item.From_Date,
                                          To_Date = item.To_Date,
                                          OrderDateAll = item.OrderDateAll,
                                          OrderDateReport = item.OrderDate.ToString("dd-MM-yy"),
                                          Entertainment = item.Entertainment,
                                          Travel = item.Travel,
                                          detailid = item.detailid,
                                          Order_Id = item.Order_Id,
                                          Work_Id = item.Work_Id
                                      }).ToList();
              List<orderinfo> tempviewlist = new List<orderinfo>();
              //List<long> Emp_ID = list.OrderBy(y => y.DesignationId).GroupBy(y => y.EmpId).Select(x => x.Key).ToList();
              //List<long> Emp_ID = list.OrderBy(y => y.DesignationId).GroupBy(y => y.Work_Id).Select(x => x.Key).ToList();
              List<long> Emp_ID = list.OrderBy(y => y.DesignationId).Select(x => x.EmpId).ToList();
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
              tempviewlist.Add(_orderinfo);*/
            return _list_orderinfo;
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


        public ActionResult BillReportSelect(orderinfo Data)
        {

            List<orderinfo> list = BillData(Data.Order_Id, Data.Work_Id);



            if (Session["userId"] != null && Session["Category"].ToString() == "In-charge")
                return PartialView(list);
            else
                return RedirectToAction("Index", "Login");

        }


        public string convert_number(double number)
        {


            //if (num)
            //    return "Not supported";
            /*
            = array("", "এক", "দুই", "তিন", "চার", "পাচ", "ছয়", 
                                        "সাত", "আট", "নয়", "দশ", "েগার", "বার", "তের", 
                                        "চৌদ্দ", "পনের", "ষোল", "সতের", "আঠারো", 
                                        "উনিশ","বিশ","একুশ","বাইশ","তেইশ","চব্বিশ","পচিশ","ছাব্বিশ","সাতাশ","আঠাস","ঊনত্রিশ","ত্রিশ","একত্রিশ","বত্রিশ","তেত্রিশ","চৌত্রিশ","পঁয়ত্রিশ","ছত্রিশ","সাতত্রিশ","আটত্রিশ","ঊনচল্লিশ","চল্লিশ","একচল্লিশ","বেয়াল্লিশ","তেতাল্লিশ","চুয়াল্লিশ","পঁয়তাল্লিশ","ছেচল্লিশ","সাতচল্লিশ","আটচল্লিশ","ঊনপঞ্চাশ","পঞ্চাশ","একান্ন","বায়ান্ন","তেপ্পান্ন","চুয়ান্ন","পঞ্চান্ন","ছাপ্পান্ন","সাতান্ন","আটান্ন","ঊনষ্ট","ষাট","একষট্টি","বাষট্টি","তেষট্টি","চৌষট্টি","পয়ষট্টি","ছেষট্টি","সাতষট্টি","আটষট্টি","ঊনসত্তর","সত্তর","একত্তর","বাহাত্তর","তেহাত্তর","চুহাত্তর","পঁচাত্তর","ছিয়াত্তর","সাতাত্তর","আটাত্তর","ঊন আশি","আশি","একাশি","বিরাশি","তেরাশি","চুরাশি","পঁচাশি","ছেয়াশি","সাতাশি","আটাশি","ঊননব্বই","নব্বই","একানব্বই","বিরানব্বই","তিরানব্বই","চুরানব্বই","পচানব্বই","ছিয়াননব্বই","সাতানব্বই","আটানব্বই","নিরানব্বই"); 
                                    $tens = array("", "", "বিশ", "ত্রিশ", "চল্লিশ", "প্নচাশ", "ষাট", 
                                        "সত্তর", "আশি", "নব্বই"); 
                                        */
            double my_number, Kt, Gn, kn, Hn;
            int Dn, n; 
            my_number = number;
            string res;
            if ((number < 0) || (number > 999999999))
            {
                throw new Exception("Number is out of range");
            }
            Kt = Math.Floor(number / 10000000); /* Koti */
            number = Kt * 10000000;
            Gn = Math.Floor(number / 100000);  /* lakh  */
            number = Gn * 100000;
            kn = Math.Floor(number / 1000);     /* Thousands (kilo) */
            number = kn * 1000;
            Hn = Math.Floor(number / 100);      /* Hundreds (hecto) */
            number -= Hn * 100;
            Dn = (int) Math.Floor(number / 10);       /* Tens (deca) */
            n = (int) number % 10;               /* Ones */
            res = "";
            if (Kt != 0)
            {
                res += convert_number(Kt) + " কোটি ";
            }
            if (Gn != 0)
            {
                res += convert_number(Gn) + " লাখ";
            }
            if (kn != 0)
            {
                res += res == "" ? "" : " " + convert_number(kn) + " হাজার";
            }
            if (Hn != 0)
            {
                res += res == "" ? "" : " " + convert_number(Hn) + " শত";
            }

            string[] ones = { "", "এক", "দুই", "তিন", "চার", "পাচ", "ছয়",
                                        "সাত", "আট", "নয়", "দশ", "েগার", "বার", "তের",
                                        "চৌদ্দ", "পনের", "ষোল", "সতের", "আঠারো",                                     "উনিশ","বিশ","একুশ","বাইশ","তেইশ","চব্বিশ","পচিশ","ছাব্বিশ", "সাতাশ","আঠাস","ঊনত্রিশ","ত্রিশ","একত্রিশ","বত্রিশ","তেত্রিশ","চৌত্রিশ","পঁয়ত্রিশ","ছত্রিশ","সাতত্রিশ","আটত্রিশ","ঊনচল্লিশ","চল্লিশ","একচল্লিশ","বেয়াল্লিশ","তেতাল্লিশ","চুয়াল্লিশ","পঁয়তাল্লিশ","ছেচল্লিশ","সাতচল্লিশ","আটচল্লিশ","ঊনপঞ্চাশ","পঞ্চাশ","একান্ন","বায়ান্ন","তেপ্পান্ন","চুয়ান্ন","পঞ্চান্ন","ছাপ্পান্ন","সাতান্ন","আটান্ন","ঊনষ্ট","ষাট","একষট্টি","বাষট্টি","তেষট্টি","চৌষট্টি","পয়ষট্টি","ছেষট্টি","সাতষট্টি","আটষট্টি","ঊনসত্তর","সত্তর","একত্তর","বাহাত্তর","তেহাত্তর","চুহাত্তর","পঁচাত্তর","ছিয়াত্তর","সাতাত্তর","আটাত্তর","ঊন আশি","আশি","একাশি","বিরাশি","তেরাশি","চুরাশি","পঁচাশি","ছেয়াশি","সাতাশি","আটাশি","ঊননব্বই","নব্বই","একানব্বই","বিরানব্বই","তিরানব্বই","চুরানব্বই","পচানব্বই","ছিয়াননব্বই","সাতানব্বই","আটানব্বই","নিরানব্বই" };
            string[] tens = {"", "", "বিশ", "ত্রিশ", "চল্লিশ", "প্নচাশ", "ষাট",
                                        "সত্তর", "আশি", "নব্বই"};


            // int crore = 0, lakhs = 0, thousands = 0, hundreds = 0, tens = 0, single = 0;


            if ((Dn !=0) || (n !=0))
            {
                if (res != null)
                {
                    res += "  ";
                }
               res += ones[Dn * 10 + n];
            }

            if (res == null)
            {
                res = "শূণ্য";
            }
            return res;
        }


        public ActionResult Bill(long? Id, long? Work_Id)
        {


            string words = convert_number(2135);


            bool check = false;
            long userId = long.Parse(Session["userId"].ToString());
            List<orderinfo> _list_orderinfo;
            if (Session["Category"].ToString() == "Admin")
                _list_orderinfo = _Bll_Employee.Get_OrderList(Id.Value);
            else
                _list_orderinfo = _Bll_Employee.Get_OrderList(Id.Value, userId);
            if (_list_orderinfo.Count > 0 || Session["Category"].ToString() == "Admin")
                check = true;

            if (Session["userId"] != null && Session["Category"].ToString() != "Member" && check == true)
            {
                if (Work_Id == null)
                    Work_Id = 0;
                List<orderinfo> list = BillData(Id.Value, Work_Id.Value);
                return View(list);
            }
            else
                return RedirectToAction("Index", "Login");

        }


        public ActionResult orderResult(long? orderid)
        {
            long Work_Id = 0;
            bool check = false;
            long userId = long.Parse(Session["userId"].ToString());

            if (orderid == null)
            {
                return RedirectToAction("search", "OrderDetail");
            }


            db_Order orderinfo = new db_Order();
            orderinfo = _Bll_Employee.Get_Order_By_ID(orderid.Value);
            ////added by bahar for shown report creator
            ViewBag.creator = orderinfo.db_User.db_Employee.Emp_Name;
            ViewBag.designation = orderinfo.db_User.db_Employee.db_Designation.Name;


            List<orderinfo> _list_orderinfo;
            if (Session["Category"].ToString() == "Admin")
                _list_orderinfo = _Bll_Employee.Get_OrderList(orderid.Value);
            else
                _list_orderinfo = _Bll_Employee.Get_OrderList(orderid.Value, userId);
            if (_list_orderinfo.Count > 0 || Session["Category"].ToString() == "Admin")
                check = true;

            if (Session["userId"] != null && Session["Category"].ToString() != "Member" && check == true)
            {
                //if (Work_Id == null)
                //    Work_Id = 0;
                List<orderinfo> list = OrderData(orderid.Value, Work_Id);
                return View(list);
            }
            else
                return RedirectToAction("Index", "Login");

        }


        // GET: db_Order_Detail/Create
        public ActionResult Create()
        {
            _listCell = _Bll_Employee.Get_Cell_list();

            ViewBag._listCell = new SelectList(_listCell, "Cell_Id", "Name");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("bn-BD");
            if(Session["Cell_Name"] !=null)
            ViewBag.refno = "৯০৪৪/অঃ নি:-0১/" + Session["Cell_Name"].ToString()+"/"+DateTime.Now.ToString("MM") + "/" + DateTime.Now.Year;
            else
                ViewBag.refno = "৯০৪৪/অঃ নি:-0১/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.Year;

            _list_work_category = _Bll_Employee.Get_Work_list();
            //long CellID = long.Parse(Session["Cell_Id"].ToString());
            //_list_work_category=_list_work_category.Where(x => x.Cell_ID == CellID).ToList();
            //_list_work_category = _list_work_category.ToList();
            ViewBag._list_work_category = new SelectList(_list_work_category, "Work_Id", "Name");

            if (Session["userId"] != null && Session["Category"].ToString() != "Member")

                return View();
            else
                return RedirectToAction("Index", "Login");

        }

        public ActionResult Edit(long? Id)

        {
            //Id = 14;
            bool check = false;
            long userId = long.Parse(Session["userId"].ToString());
            List<orderinfo> _list_orderinfo;
            if (Session["Category"].ToString() == "Admin")
                _list_orderinfo = _Bll_Employee.Get_OrderList(Id.Value);
            else
                _list_orderinfo = _Bll_Employee.Get_OrderList(Id.Value, userId);
            if (_list_orderinfo.Count > 0 || Session["Category"].ToString() == "Admin")
                check = true;
            _list_orderinfo = _list_orderinfo.Where(x => x.Status == null).ToList();
            List<orderinfo> _list_orderinfotem=new List<orderinfo>();
            List<long> Emp_ID = _list_orderinfo.GroupBy(y => y.EmpId).Select(x => x.Key).ToList();
            foreach (var emp in Emp_ID)
            {
                var templist = _list_orderinfo.Where(x => x.EmpId == emp).ToList();

                orderinfo _orderinfotemp = new orderinfo();
                _orderinfotemp.day = 0;
                foreach (var item in templist)
                {
                    
                       

                     Thread.CurrentThread.CurrentCulture = new CultureInfo("bn-BD");
                    for (DateTime listdate = item.From_Date; listdate.Date <= item.To_Date; listdate = listdate.AddDays(1))
                    {
                        _orderinfotemp.From_Dateall += listdate.ToString("yyyy-MM-dd");
                        //if (item.To_Date != listdate)
                        //if (listdate != item.To_Date)
                        if(templist.Last() != item )
                            _orderinfotemp.From_Dateall += ", ";
                        else if (item.To_Date != listdate)
                                _orderinfotemp.From_Dateall += ", ";
                    }
                    _orderinfotemp.Empname = item.Empname;
                    _orderinfotemp.EmpId = item.EmpId;
                    _orderinfotemp.Designation = item.Designation;
                    _orderinfotemp.Bank_Id = item.Bank_Id;
                    _orderinfotemp.Details = item.Details;
                    _orderinfotemp.detailid = item.detailid;
                    _orderinfotemp.Order_Id = item.Order_Id;




                }

                _list_orderinfotem.Add(_orderinfotemp);


            }


            _listCell = _Bll_Employee.Get_Cell_list();
            ViewBag._listCell = new SelectList(_listCell, "Cell_Id", "Name");
            _list_work_category = _Bll_Employee.Get_Work_list();
            // long CellID = long.Parse(Session["Cell_Id"].ToString());
            //_list_work_category = _list_work_category.Where(x => x.Cell_ID == CellID).ToList();
           long WORK_id= _list_orderinfo.Select(X => X.Work_Id).FirstOrDefault();
            ViewBag._list_work_category = new SelectList(_list_work_category, "Work_Id", "Name");
            ViewBag.WORK_id = WORK_id;

            if (Session["userId"] != null && Session["Category"].ToString() != "Member" && check == true)

                return View(_list_orderinfotem);
            else
                return RedirectToAction("Index", "Login");

        }

        public ActionResult SaveEdit(List<orderinfo> data)
        {
            string result = _Bll_Employee.SaveEdit(data, long.Parse(Session["userId"].ToString()));


            return Json(result, JsonRequestBehavior.AllowGet);
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

        public ActionResult EmpInfoId(long empid)
        {
            var Employeeinfo = _Bll_Employee.Get_Emp_By_ID_Json(empid);


            return Json(Employeeinfo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WorkInfo(long Work_Id)
        {
            db_Work_Category _db_Work_Category1 = _Bll_Component.Edit(Work_Id);
            var _db_Work_Category = new
            {
                _db_Work_Category1.Work_Id,
                _db_Work_Category1.Name,
                _db_Work_Category1.Start_Time,
                _db_Work_Category1.End_Time,
                _db_Work_Category1.Status
            };
            return Json(_db_Work_Category, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Save(List<db_Order_Detail> data)
        {
            string result = _Bll_Employee.Save_order_detail(data, long.Parse(Session["userId"].ToString()));


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public object Data { get; set; }
    }
}
