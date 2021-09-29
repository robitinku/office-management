using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Office_Dll;
using System.Data.Entity;
using System.IO;

namespace Office_Management_System.Controllers
{
    public class DbManagementController : Controller
    {
        // GET: DbManagement
        
        public ActionResult Index()
        {
            if(Session["Category"] != null && Session["Category"].ToString() == "Admin")
                {
                return View();//new FilePathResult(dbPath, "application/octet-stream");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult BackUp()
        {
            String message = "";
            //WITH COMPRESSION,ENCRYPTION(ALGORITHM = AES_128,SERVER CERTIFICATE = BackupEncryptCert),STATS = 10
            bool exists = Directory.Exists(Server.MapPath("~/backup/"));

            if (!exists)
            Directory.CreateDirectory(Server.MapPath("~/backup/"));
            string dbPath = Server.MapPath("~/backup/" + DateTime.Now.ToString("yyyyMMdd") + ".bak");
       
           

            if (System.IO.File.Exists(dbPath))
                System.IO.File.Delete(dbPath);
          
            using (var db = new Office_ManagementEntities())
            {
                string databaseName = db.Database.Connection.Database;
                try {
                var cmd = String.Format("backup database "+ databaseName + "  to disk='" + dbPath + "'");
                db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, cmd);
                    message = "BackUp Successfully";
                }
                catch(Exception ex)
                {
                     message = "Try Again";
                   
                }
            }
           
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RestoreDatabase()
        {

            var directory = new DirectoryInfo(Server.MapPath("~/backup/"));
            var myFile = (from f in directory.GetFiles()
                          orderby f.LastWriteTime descending
                          select f).ToList();
            List<string> files = new List<string>();
            
            
            string tr = "";
            foreach (var filePath in myFile)
            {
                string formatString = "yyyyMMdd";
                string[] time = filePath.Name.Split('.');
                DateTime Filetime = DateTime.ParseExact(time[0], formatString, null);
                tr += "<tr>";
                tr += "<td>" + Filetime.ToString("dd-MM-yyyy") + "</td>";
                tr += "<td>" + filePath.Directory + "</td>";
                tr += "<td>" + filePath.Name + "</td>";
                tr += "<td><input type='hidden' value='"+ filePath.Name + "' name='RestoreData' id='RestoreData'/> <input type='button' value='Restore' class='icon - 1 info - tooltip' id='Restoreback' /></td>";

                tr += "</tr>";
               

            }
            return Json(tr, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Restore(string name)
        {
            String message = "";
            string dbPath = Server.MapPath("~/backup/"+ name);
            using (var db = new Office_ManagementEntities())
            {
                string databaseName = db.Database.Connection.Database;
                try {
                    var cmd = String.Format("USE master ALTER DATABASE "+ databaseName + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                    db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, cmd);

                     cmd = String.Format("USE master restore DATABASE {0} from DISK='{1}' WITH REPLACE;"
                    , databaseName, dbPath);
                db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, cmd);
                    message = "Restore Successfully";
                }
                catch (Exception ex)
                {
                    message = "Try Again";

                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
    }
}