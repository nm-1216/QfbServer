namespace QfbServer.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170720 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "userType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "userType");
        }
    }
}
