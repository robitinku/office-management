using Office_Bll;
using Office_Dll;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Office_Management_System.Controllers
{
    public class UserpanelController : Controller
    {
        // GET: Userpanel
        // GET: CellInfo
        Bll_Component _Bll_Component = new Bll_Component();
        Bll_Employee _Bll_Employee = new Bll_Employee();
        List<db_User> _listdb_User = new List<db_User>();
        List<db_Log> _listdb_Log = new List<db_Log>();
        public object _list_Emp_list { get; set; }
        // GET: Work
        public ActionResult Index(int? page)
        {
            int pageSize = 10;


            if (page == null)
            {
                page = 1;
            }

            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "In-charge", Value = "1"},
                new SelectListItem{Text = "Member", Value = "2"},
                new SelectListItem{Text = "Verifier", Value = "3"},
                new SelectListItem{Text = "Forwarder", Value = "4"}

            };
            ViewBag._listCategory=  new SelectList(items, "Value", "Text"); ;


            _listdb_User = _Bll_Component.Get_All_UserList();
            ViewBag._list_Emp_list = _Bll_Employee.Get_Emp_list();



            int pageNumber = (page ?? 1);
            if (Session["userId"] != null && Session["Category"].ToString() != "Member" && Session["Category"].ToString() != "Verifier" && Session["Category"].ToString() != "Forwarder")

                return View(_listdb_User.ToPagedList(pageNumber, pageSize));
            else
                return RedirectToAction("Index", "Login");


        }


       

        public ActionResult logs(int? page)
        {
           
            int pageSize = 10;

            if (page == null)
            {
                page = 1;
            }

            _listdb_Log = _Bll_Component.Get_All_LogList();
            //ViewBag._list_Emp_list = _Bll_Employee.Get_Emp_list();
            
            int pageNumber = (page ?? 1);
            

            if (Session["userId"] != null && Session["Category"].ToString() == "Admin")

                return View(_listdb_Log.ToPagedList(pageNumber, pageSize));
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult AddData(db_User data)
        {
          
            bool result;
            result = _Bll_Component.NameCheckUser(data);
            if (result == false)
            {

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                data.Password = passwordenq(data.Password);
                result = _Bll_Component.Add(data);

                return Json(result, JsonRequestBehavior.AllowGet);
            }



        }

        public ActionResult EditData(db_User data)
        {
            
            bool result;
            result = _Bll_Component.NameCheckUser(data);
            if (result == false)
            {

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                if (data.Password != null)
                {
                    data.Password = passwordenq(data.Password);
                }
                result = _Bll_Component.Edit_User(data, long.Parse(Session["userId"].ToString()));

                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }

        public string passwordenq(string p)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytPassword = uEncode.GetBytes(p);
            SHA512Managed sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(bytPassword);
            p= Convert.ToBase64String(hash);
            return p;

        }
        public ActionResult PasswordChange()
        {
            if (Session["userId"] != null)

                return View("PasswordChange");
            else
                return RedirectToAction("Index", "Login");
        
        }

        public ActionResult PasswordEdit(string Password, string cupassword)
        {
            db_User _db_User= new db_User();
            _db_User.Password = passwordenq(cupassword);
            _db_User.User_Id =long.Parse(Session["userId"].ToString());
            bool result;
            result= _Bll_Component.CheckPass(_db_User);
            if(result==true)
            {
                db_User _db_Userp = new db_User();
                _db_Userp.Password = passwordenq(Password);
                _db_Userp.User_Id = long.Parse(Session["userId"].ToString());
                result = _Bll_Component.ChangePass(_db_Userp);
            }

            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(long Id)
        {

            db_User _db_User1 = _Bll_Component.Get_User_ID(Id);
            var _db_User = new
            {
                _db_User1.User_Id,
                _db_User1.User_Name,
                _db_User1.Category,
                _db_User1.Status,
                Emp_Name= _db_User1.db_Employee.Emp_Name,
                Emp_Id = _db_User1.db_Employee.Emp_Id
            };
            return Json(_db_User, JsonRequestBehavior.AllowGet);
        }
    }
}