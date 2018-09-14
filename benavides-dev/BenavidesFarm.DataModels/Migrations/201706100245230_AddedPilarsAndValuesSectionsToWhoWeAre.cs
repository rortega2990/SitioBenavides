namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPilarsAndValuesSectionsToWhoWeAre : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id2", c => c.Int());
            AddColumn("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id3", c => c.Int());
            CreateIndex("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id2");
            CreateIndex("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id3");
            AddForeignKey("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id2", "dbo.WhoWeArePages", "Id");
            AddForeignKey("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id3", "dbo.WhoWeArePages", "Id");
            DropColumn("dbo.WhoWeArePages", "WhoWeAreSection_ImageFileName");
            DropColumn("dbo.WhoWeArePages", "VisionSection_ImageFileName");
            DropColumn("dbo.WhoWeArePages", "MisionSection_ImageFileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WhoWeArePages", "MisionSection_ImageFileName", c => c.String());
            AddColumn("dbo.WhoWeArePages", "VisionSection_ImageFileName", c => c.String());
            AddColumn("dbo.WhoWeArePages", "WhoWeAreSection_ImageFileName", c => c.String());
            DropForeignKey("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id3", "dbo.WhoWeArePages");
            DropForeignKey("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id2", "dbo.WhoWeArePages");
            DropIndex("dbo.WhoWeAreTitledSections", new[] { "WhoWeArePage_Id3" });
            DropIndex("dbo.WhoWeAreTitledSections", new[] { "WhoWeArePage_Id2" });
            DropColumn("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id3");
            DropColumn("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id2");
        }
    }
}
