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
    public class DepartmentController : Controller
    {
        // GET: Department
        // GET: CellInfo
        Bll_Component _Bll_Component = new Bll_Component();
        List<db_Department> _listdb_Department = new List<db_Department>();
        // GET: Work
        public ActionResult Index(int? page)
        {
            int pageSize = 10;


            if (page == null)
            {
                page = 1;
            }




            _listdb_Department = _Bll_Component.Get_All_list_Dep();




            int pageNumber = (page ?? 1);
            if (Session["userId"] != null)

                return View(_listdb_Department.ToPagedList(pageNumber, pageSize));
            else
                return RedirectToAction("Index", "Login");


        }
        public ActionResult AddData(db_Department data)
        {
            data.Date = DateTime.Now;
            data.User_Id = long.Parse(Session["userId"].ToString());
            bool result;
            result = _Bll_Component.NameCheckDep(data);
            if (result == false)
            {

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                result = _Bll_Component.Add(data);

                return Json(result, JsonRequestBehavior.AllowGet);
            }



        }

        public ActionResult EditData(db_Department data)
        {
            data.Date = DateTime.Now;
            data.User_Id = long.Parse(Session["userId"].ToString());
            bool result;
            result = _Bll_Component.NameCheckDep(data);
            if (result == false)
            {

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                result = _Bll_Component.Edit_db_Dep(data);

                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult Edit(long Id)
        {

            db_Department _db_Department1 = _Bll_Component.Get_Dep_ID(Id);
            var _db_Department = new
            {
                _db_Department1.Dep_id,
                _db_Department1.Name,

                _db_Department1.Status
            };
            return Json(_db_Department, JsonRequestBehavior.AllowGet);
        }
    
}
}