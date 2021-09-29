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
    public class JobCategoryController : Controller
    {
        // GET: JobCategory

        // GET: CellInfo
        Bll_Component _Bll_Component = new Bll_Component();
        List<db_Job_Cate> _listdb_Job_Cate = new List<db_Job_Cate>();
        // GET: Work
        public ActionResult Index(int? page)
        {
            int pageSize = 10;


            if (page == null)
            {
                page = 1;
            }




            _listdb_Job_Cate = _Bll_Component.Get_All_JobCategory();




            int pageNumber = (page ?? 1);
            if (Session["userId"] != null)

                return View(_listdb_Job_Cate.ToPagedList(pageNumber, pageSize));
            else
                return RedirectToAction("Index", "Login");


        }
        public ActionResult AddData(db_Job_Cate data)
        {
            bool result;
            
            data.Date = DateTime.Now;
            data.User_Id = long.Parse(Session["userId"].ToString());
            result= _Bll_Component.NameCheck(data);
            if(result==false)
            {
                
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else { 
            result = _Bll_Component.Add(data);

            return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditData(db_Job_Cate data)
        {
            bool result;
            data.Date = DateTime.Now;
            data.User_Id = long.Parse(Session["userId"].ToString());
            result = _Bll_Component.NameCheck(data);
            if (result == false)
            {

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                result = _Bll_Component.Edit_db_Job_Cate(data);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
           
        }
        public ActionResult Edit(long Id)
        {

            db_Job_Cate _db_Job_Cate1 = _Bll_Component.Get_Job_Cate_ID(Id);
            var _db_Job_Cate = new
            {
                _db_Job_Cate1.Category_Id,
                _db_Job_Cate1.Name,

                _db_Job_Cate1.Status
            };
            return Json(_db_Job_Cate, JsonRequestBehavior.AllowGet);
        }
    }
}