using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QfbServer.Models
{
    public class QfbServerContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public QfbServerContext() : base("name=QfbServerContext")
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Page> Pages { get; set; }

        public DbSet<MeasurementItem> MeasurementItem { get; set; }
        public DbSet<MeasurementPage> MeasurementPage { get; set; }
        public DbSet<MeasurementPoint> MeasurementPoint { get; set; }
        public DbSet<MeasurementReport> MeasurementReport { get; set; }
        public System.Data.Entity.DbSet<QfbServer.Models.MeasureData> MeasureDatas { get; set; }
        public System.Data.Entity.DbSet<QfbServer.Models.Warning> Warnings { get; set; }
    }
}
