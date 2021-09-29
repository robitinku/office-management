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
    public class BenifitController : Controller
    {
        Bll_Component _Bll_Component = new Bll_Component();
        List<db_benifit> _listdb_benifit = new List<db_benifit>();
        // GET: Work
        public ActionResult Index(int? page)
        {
            int pageSize = 10;


            if (page == null)
            {
                page = 1;
            }




            _listdb_benifit = _Bll_Component.Get_All_list_benifit();




            int pageNumber = (page ?? 1);
            if (Session["userId"] != null)

                return View(_listdb_benifit.ToPagedList(pageNumber, pageSize));
            else
                return RedirectToAction("Index", "Login");
           

        }
        public ActionResult AddData(db_benifit data)
        {

            bool result = _Bll_Component.Add(data);
            string message = "";
            if (result == true)
                message = "Save Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditData(db_benifit data)
        {

            bool result = _Bll_Component.Edit_Benifit(data);
            string message = "";
            if (result == true)
                message = "Save Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Delete(db_benifit data)
        {

            bool result = _Bll_Component.Delete_Benifit(data);
            string message = "";
            if (result == true)
                message = "Delete Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(long Id)
        {

            db_benifit _db_benifit1 = _Bll_Component.Get_Benifit_ID(Id);
            var _db_benifit = new
            {
                _db_benifit1.id,
                _db_benifit1.amount,
                _db_benifit1.type,
                _db_benifit1.date,
                _db_benifit1.comments
            };
            return Json(_db_benifit, JsonRequestBehavior.AllowGet);
        }

    }
}