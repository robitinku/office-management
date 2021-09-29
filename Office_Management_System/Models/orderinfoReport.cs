using System;

namespace Office_Management_System.Models
{
    public class orderinfoReport
    {
        public string Bank_Id { get; set; }
        public long Cellid { get; set; }
        public string Cellname { get; set; }
        public string Designation { get; set; }
        public long detailid { get; set; }
        public long EmpId { get; set; }
        public string Empname { get; set; }
        public TimeSpan End_time { get; set; }
        public DateTime From_Date { get; set; }
        public long Order_Id { get; set; }
        public string Order_No { get; set; }
        public TimeSpan Start_time { get; set; }
        public DateTime To_Date { get; set; }
        public string Workname { get; set; }
        public long Work_Id { get; set; }
    }
}