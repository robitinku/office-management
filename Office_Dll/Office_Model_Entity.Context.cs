﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Office_Dll
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Office_ManagementEntities1 : DbContext
    {
        public Office_ManagementEntities1()
            : base("name=Office_ManagementEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<db_calendar> db_calendar { get; set; }
        public virtual DbSet<db_Cell> db_Cell { get; set; }
        public virtual DbSet<db_Department> db_Department { get; set; }
        public virtual DbSet<db_Designation> db_Designation { get; set; }
        public virtual DbSet<db_Emp_Cell> db_Emp_Cell { get; set; }
        public virtual DbSet<db_Employee> db_Employee { get; set; }
        public virtual DbSet<db_image> db_image { get; set; }
        public virtual DbSet<db_Job_Cate> db_Job_Cate { get; set; }
        public virtual DbSet<db_Order> db_Order { get; set; }
        public virtual DbSet<db_Order_Detail> db_Order_Detail { get; set; }
        public virtual DbSet<db_User> db_User { get; set; }
        public virtual DbSet<db_Work_Category> db_Work_Category { get; set; }
        public virtual DbSet<db_work_text> db_work_text { get; set; }
    }
}
