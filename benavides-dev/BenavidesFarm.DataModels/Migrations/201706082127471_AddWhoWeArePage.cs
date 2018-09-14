namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWhoWeArePage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WhoWeArePages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        WhoWeAreSection_Title = c.String(),
                        WhoWeAreSection_TitleTextColor = c.String(),
                        WhoWeAreSection_BackgroundColor = c.String(),
                        WhoWeAreSection_Message = c.String(),
                        WhoWeAreSection_MessageTextColor = c.String(),
                        WhoWeAreSection_ImageFileName = c.String(),
                        VisionSection_Title = c.String(),
                        VisionSection_TitleTextColor = c.String(),
                        VisionSection_BackgroundColor = c.String(),
                        VisionSection_Message = c.String(),
                        VisionSection_MessageTextColor = c.String(),
                        VisionSection_ImageFileName = c.String(),
                        MisionSection_Title = c.String(),
                        MisionSection_TitleTextColor = c.String(),
                        MisionSection_BackgroundColor = c.String(),
                        MisionSection_Message = c.String(),
                        MisionSection_MessageTextColor = c.String(),
                        MisionSection_ImageFileName = c.String(),
                        AdSection_Title = c.String(),
                        AdSection_TitleTextColor = c.String(),
                        AdSection_BackgroundColor = c.String(),
                        AdSection_Message = c.String(),
                        AdSection_MessageTextColor = c.String(),
                        AdSection_ImageFileName = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.WhoWeAreTitledSections",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    SubTitle = c.String(),
                    Text = c.String(),
                    ImageFileName = c.String(),
                    WhoWeArePage_Id = c.Int(),
                    WhoWeArePage_Id1 = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WhoWeArePages", t => t.WhoWeArePage_Id, cascadeDelete: false)
                .ForeignKey("dbo.WhoWeArePages", t => t.WhoWeArePage_Id1)
                .Index(t => t.WhoWeArePage_Id)
                .Index(t => t.WhoWeArePage_Id1);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id1", "dbo.WhoWeArePages");
            DropForeignKey("dbo.WhoWeAreTitledSections", "WhoWeArePage_Id", "dbo.WhoWeArePages");
            DropIndex("dbo.WhoWeAreTitledSections", new[] { "WhoWeArePage_Id1" });
            DropIndex("dbo.WhoWeAreTitledSections", new[] { "WhoWeArePage_Id" });
            DropTable("dbo.WhoWeAreTitledSections");
            DropTable("dbo.WhoWeArePages");
        }
    }
}
