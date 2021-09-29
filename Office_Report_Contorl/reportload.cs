using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office_Dll;
using System.Web;
namespace Office_Report_Contorl
{
   public class reportload
    {
        public Stream load(List<db_Employee> Employee_list,string )
        { 
           ReportClass rptH = new ReportClass();
            string path = HttpContext.Current.Server.MapPath("~/Report/OrderResult.rpt");
            rptH.FileName = path;
            rptH.Load();
            rptH.SetDataSource(Employee_list);
            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return stream;
        }
    }
}
