using Newtonsoft.Json;
using Office_Bll;
using Office_Dll;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Office_Management_System.Controllers
{
    public class WorkController : Controller
    {
        Bll_Component _Bll_Component = new Bll_Component();
        List<db_Work_Category> _listWorkCategory = new List<db_Work_Category>();
        Bll_Employee _Bll_Employee = new Bll_Employee();
       
        List<db_Cell> _listCell = new List<db_Cell>();
        // GET: Work
        public ActionResult Index(int? page)
        {
            int pageSize = 10;


            if (page == null)
            {
                page = 1;
            }




            _listWorkCategory = _Bll_Component.Get_All_WorkCategory();

            _listCell = _Bll_Employee.Get_Cell_list();
            ViewBag._listCell = new SelectList(_listCell, "Cell_ID", "Name");


            int pageNumber = (page ?? 1);
            if (Session["userId"] != null)

                return View(_listWorkCategory.ToPagedList(pageNumber, pageSize));
            else
                return RedirectToAction("Index", "Login");


        }
        public ActionResult AddData(db_Work_Category data)
        {
            data.Date = DateTime.Now;
            data.User_Id = long.Parse(Session["userId"].ToString());
            bool result;
            result = _Bll_Component.NameCheckWork(data);
            if (result == false)
            {

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                result = _Bll_Component.Add(data);

                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult EditData(db_Work_Category data)
        {
            data.Date = DateTime.Now;
            data.User_Id = long.Parse(Session["userId"].ToString());
            bool result;
            result = _Bll_Component.NameCheckWork(data);
            if (result == false)
            {

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                result = _Bll_Component.EditData(data); ;

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Edit(long Work_Id)
        {

            db_Work_Category _db_Work_Category1 = _Bll_Component.Edit(Work_Id);
            var _db_Work_Category = new
            {
                _db_Work_Category1.Work_Id,
                _db_Work_Category1.Name,
                _db_Work_Category1.Start_Time,
                _db_Work_Category1.End_Time,
                _db_Work_Category1.Status,
                _db_Work_Category1.Entertainment,
                _db_Work_Category1.work_type,
                _db_Work_Category1.Travel,
                _db_Work_Category1.Detail,
                _db_Work_Category1.Cell_ID
            };
            return Json(_db_Work_Category, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WorkText(long? id)
        {
            db_work_text _db_work_text = _Bll_Component.Work_Edit_text(id.Value);
            if (Session["userId"] != null)

                return View(_db_work_text);
            else
                return RedirectToAction("Index", "Login");
          
        }

        public ActionResult EditText(db_work_text data)
        {
            
            bool result;

            result = _Bll_Component.EditText(data); ;
            string message = "Save Successfully" ;
                return Json(message, JsonRequestBehavior.AllowGet);
         }

        
    }
}