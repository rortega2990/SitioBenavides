namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDoctorsOfficeSectionToHome : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DoctorsOfficeSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        SectionMessageText = c.String(),
                        ImageFileName = c.String(),
                        BackgroundColor = c.String(),
                        LogoImageFileName = c.String(),
                        Link = c.String(),
                        TitleColor = c.String(),
                        SectionMessageTextColor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.HomePages", "DoctorsOfficeSectionId", c => c.Int(nullable: false));
            CreateIndex("dbo.HomePages", "DoctorsOfficeSectionId");
            AddForeignKey("dbo.HomePages", "DoctorsOfficeSectionId", "dbo.DoctorsOfficeSections", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HomePages", "DoctorsOfficeSectionId", "dbo.DoctorsOfficeSections");
            DropIndex("dbo.HomePages", new[] { "DoctorsOfficeSectionId" });
            DropColumn("dbo.HomePages", "DoctorsOfficeSectionId");
            DropTable("dbo.DoctorsOfficeSections");
        }
    }
}
