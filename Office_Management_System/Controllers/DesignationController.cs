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
    public class DesignationController : Controller
    {
        // GET: CellInfo
        Bll_Component _Bll_Component = new Bll_Component();
        List<db_Designation> _listdb_Designation = new List<db_Designation>();
        // GET: Work
        public ActionResult Index(int? page)
        {
            int pageSize = 10;


            if (page == null)
            {
                page = 1;
            }




            _listdb_Designation = _Bll_Component.Get_All_list_Designation();




            int pageNumber = (page ?? 1);
            if (Session["userId"] != null)

                return View(_listdb_Designation.ToPagedList(pageNumber, pageSize));
            else
                return RedirectToAction("Index", "Login");
            

        }
        public ActionResult AddData(db_Designation data)
        {
            data.Date = DateTime.Now;
            data.User_Id = long.Parse(Session["userId"].ToString());
            bool result;
            result = _Bll_Component.NameCheckDesignation(data);
            if (result == false)
            {

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                result = _Bll_Component.Add(data);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
             
           
        }

        public ActionResult EditData(db_Designation data)
        {

            bool result = _Bll_Component.Edit_db_Designation(data);
            string message = "";
            if (result == true)
                message = "Save Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(long Id)
        {

            db_Designation _db_Designation1 = _Bll_Component.Get_Designation_ID(Id);
            var _db_Designation = new
            {
                _db_Designation1.Designation_Id,
                _db_Designation1.Name,
                _db_Designation1.Bn_Name,
                _db_Designation1.Status
            };
            return Json(_db_Designation, JsonRequestBehavior.AllowGet);
        }
    }
}