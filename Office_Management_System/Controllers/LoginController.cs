using Office_Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Office_Dll;
namespace Office_Management_System.Controllers
{
    public class LoginController : Controller
    {
        Bll_Employee _Bll_Employee = new Bll_Employee();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogInCheck(db_User _db_User)
        {


            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytPassword = uEncode.GetBytes(_db_User.Password);
            SHA512Managed sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(bytPassword);
            string pass = Convert.ToBase64String(hash);

            List<db_User> listdb_User = _Bll_Employee.GetUser_Info(_db_User.User_Name, pass);
            if (listdb_User.Count > 0)
            {
                Session["userId"] = listdb_User[0].User_Id;
                Session["Category"] = listdb_User[0].Category;
                Session["Emp_Name"] = listdb_User[0].db_Employee.Emp_Name;
                Session["Bn_Emp_Name"] = listdb_User[0].db_Employee.Bn_Emp_Name;
                Session["Emp_Id"] = listdb_User[0].db_Employee.Emp_Id;
                Session["Cell_Id"] = listdb_User[0].db_Employee.db_Cell.Cell_Id;
                Session["Cell_Name"] = listdb_User[0].db_Employee.db_Cell.Short_name;
                Session["Designation"] = listdb_User[0].db_Employee.db_Designation.Name;
                Session["Bn_Designation"] = listdb_User[0].db_Employee.db_Designation.Bn_Name;
                return RedirectToAction("Index", "Emp");
            }
            else {
                ViewBag.message = "Invalid User Name Or Password";
                return View("Index");
            }


        }

        public ActionResult Logout(db_User _db_User)
        {
            Session.RemoveAll();
            return View("Index");
        }
    }
}