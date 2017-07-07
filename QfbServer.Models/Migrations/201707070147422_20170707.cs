namespace QfbServer.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170707 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeasureDatas", "UpperTolerance", c => c.String());
            AddColumn("dbo.MeasureDatas", "LowerTolerance", c => c.String());
            AddColumn("dbo.MeasureDatas", "Value5", c => c.String());
            AddColumn("dbo.MeasureDatas", "Value6", c => c.String());
            AddColumn("dbo.MeasureDatas", "Value7", c => c.String());
            AddColumn("dbo.MeasureDatas", "Value8", c => c.String());
            AddColumn("dbo.MeasureDatas", "Value9", c => c.String());
            AddColumn("dbo.MeasureDatas", "Value10", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeasureDatas", "Value10");
            DropColumn("dbo.MeasureDatas", "Value9");
            DropColumn("dbo.MeasureDatas", "Value8");
            DropColumn("dbo.MeasureDatas", "Value7");
            DropColumn("dbo.MeasureDatas", "Value6");
            DropColumn("dbo.MeasureDatas", "Value5");
            DropColumn("dbo.MeasureDatas", "LowerTolerance");
            DropColumn("dbo.MeasureDatas", "UpperTolerance");
        }
    }
}
