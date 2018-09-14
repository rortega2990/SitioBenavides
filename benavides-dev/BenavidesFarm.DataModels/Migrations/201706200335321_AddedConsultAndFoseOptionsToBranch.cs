namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedConsultAndFoseOptionsToBranch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Branches", "BranchTwentyFourHours", c => c.Boolean(nullable: false));
            AddColumn("dbo.Branches", "BranchFose", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Branches", "BranchFose");
            DropColumn("dbo.Branches", "BranchTwentyFourHours");
        }
    }
}
