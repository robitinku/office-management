using System;
using System.Collections.Generic;

namespace Office_Dll
{
    public class orderinfo
    {
        public int day { get; set; }
        public string Status { get; set; }
        public string OrderDateReport { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderDateAll { get; set; }
        public string Bank_Id { get; set; }
        public long Cellid { get; set; }
        public string Cellname { get; set; }
        public string Designation { get; set; }
        public string Designation_bn { get; set; }
        public long detailid { get; set; }
        public long EmpId { get; set; }
        public string Empname { get; set; }
        public string Empname_bn { get; set; }
        public TimeSpan End_time { get; set; }
        public DateTime From_Date { get; set; }
        public long Order_Id { get; set; }
        public string Order_No { get; set; }
        public TimeSpan Start_time { get; set; }
        public DateTime To_Date { get; set; }
        public string Workname { get; set; }
        public long Work_Id { get; set; }
        public string Detail { get; set; }
        public string Refno { get;  set; }
        public decimal Travel { get; set; }
        public decimal Entertainment { get; set; }
        public decimal Ttravel { get; set; }
        public decimal TEntertainment { get; set; }
        public string ReporterDesignation { get; set; }
        public string Reportername { get; set; }
        public decimal Total { get; set; }
        public string strTotal { get; set; }
        public string Subject { get; set; }
        public string Signatory { get; set; }
        public string SignatoryD { get; set; }
        
        public string DetailHeader { get; set; }
        public string DetailFottor { get; set; }
        public string CCCopy { get; set; }
        //public List<long> list { get; set; }
        public long DesignationId { get; set; }
        public string strTravel { get; set; }
        public string sl { get; set; }
        public string strDay { get; set; }
        public string strTtravel { get; set; }
        public string strTEntertainment { get; set; }
        public string strEntertainment { get; set; }
        public int Serial { get; set; }
        public System.DateTime promotion_Date { get; set; }
        public int work_type { get; set; }
        public decimal NTravel { get; set; }
        public decimal NEntertainment { get; set; }

        public string Details { get; set; }
        public string From_Dateall { get; set; }

    }
}