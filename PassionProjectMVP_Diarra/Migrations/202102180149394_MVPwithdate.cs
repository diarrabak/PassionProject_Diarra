namespace PassionProjectMVP_Diarra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MVPwithdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Classes", "startDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Classes", "endDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Classes", "endDate");
            DropColumn("dbo.Classes", "startDate");
        }
    }
}
