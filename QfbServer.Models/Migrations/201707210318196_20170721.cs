namespace QfbServer.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170721 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeasureDatas", "PointId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeasureDatas", "PointId");
        }
    }
}
