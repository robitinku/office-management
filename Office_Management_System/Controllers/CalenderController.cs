using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Office_Bll;
using Office_Dll;
using PagedList;
namespace Office_Management_System.Controllers
{
    public class CalenderController : Controller
    {
        Bll_Component _Bll_Component = new Bll_Component();
        List<db_calendar> _listdb_calendar = new List<db_calendar>();
        // GET: Work
        public ActionResult Index(int? page)
        {
            int pageSize = 10;


            if (page == null)
            {
                page = 1;
            }




            _listdb_calendar = _Bll_Component.Get_All_list_calendar();




            int pageNumber = (page ?? 1);
            if (Session["userId"] != null)

                return View(_listdb_calendar.ToPagedList(pageNumber, pageSize));
            else
                return RedirectToAction("Index", "Login");
           

        }
        public ActionResult AddData(db_calendar data)
        {

            bool result = _Bll_Component.Add(data);
            string message = "";
            if (result == true)
                message = "Save Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditData(db_calendar data)
        {

            bool result = _Bll_Component.Edit_Calender(data);
            string message = "";
            if (result == true)
                message = "Save Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Delete(db_calendar data)
        {

            bool result = _Bll_Component.Delete_Calender(data);
            string message = "";
            if (result == true)
                message = "Delete Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(long Id)
        {

            db_calendar _db_calendar1 = _Bll_Component.Get_Calender_ID(Id);
            var _db_calendar = new
            {
                _db_calendar1.Id,
                _db_calendar1.Discription,
                _db_calendar1.HoliDay_Date,
                
                _db_calendar1.Status
            };
            return Json(_db_calendar, JsonRequestBehavior.AllowGet);
        }

    }
}