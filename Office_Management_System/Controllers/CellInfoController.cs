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
    public class CellInfoController : Controller
    {
        // GET: CellInfo
        Bll_Component _Bll_Component = new Bll_Component();
        List<db_Cell> _listdb_cell = new List<db_Cell>();
        // GET: Work
        public ActionResult Index(int? page)
        {
            int pageSize = 10;


            if (page == null)
            {
                page = 1;
            }




            _listdb_cell = _Bll_Component.Get_All_list_Cell();




            int pageNumber = (page ?? 1);
            if (Session["userId"] != null)

                return View(_listdb_cell.ToPagedList(pageNumber, pageSize));
            else
                return RedirectToAction("Index", "Login");
          

        }
        public ActionResult AddData(db_Cell data)
        {
            data.Date = DateTime.Now;
            data.User_Id = long.Parse(Session["userId"].ToString());
            bool result ;
            result = _Bll_Component.NameCheckCell(data);
            if (result == false)
            {

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                result = _Bll_Component.Add(data);

                return Json(result, JsonRequestBehavior.AllowGet);
            }

           
            
        }

        public ActionResult EditData(db_Cell data)
        {
            data.Date = DateTime.Now;
            data.User_Id = long.Parse(Session["userId"].ToString());
            bool result;
            result = _Bll_Component.NameCheckCell(data);
            if (result == false)
            {

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                result = _Bll_Component.Edit_db_Cell(data);

                return Json(result, JsonRequestBehavior.AllowGet);
            }

     
        }
        public ActionResult Edit(long Id)
        {

            db_Cell _db_Cell1 = _Bll_Component.Get_Cell_ID(Id);
            var _db_Cell = new
            {
                _db_Cell1.Cell_Id,
                _db_Cell1.Name,
                _db_Cell1.Short_name,

                _db_Cell1.Status
            };
            return Json(_db_Cell, JsonRequestBehavior.AllowGet);
        }
    }
}