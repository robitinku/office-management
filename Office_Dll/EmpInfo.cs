

namespace Office_Dll
{
    using System;
    using System.Collections.Generic;
    public partial class EmpInfo
    {
        public DateTime DateReport { get; set; }
        public String Date{ get; set; }
        public string Designation { get;  set; }
        public string Bank_Id { get; set; }
        public string Empname { get; set; }
        public string Cell { get;  set; }
        public string Work { get;  set; }
        public TimeSpan StartTime { get;  set; }
        public TimeSpan EndTime { get;  set; }
        public TimeSpan Total { get;  set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        //public Nullable<int> Serial { get; set; }
    }
}
