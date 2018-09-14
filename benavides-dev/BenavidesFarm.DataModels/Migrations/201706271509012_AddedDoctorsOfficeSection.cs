namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDoctorsOfficeSection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DoctorsOfficePages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DoctorsOfficePageSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageFileName = c.String(),
                        Title = c.String(),
                        TitleColor = c.String(),
                        DoctorsOfficePage_Id = c.Int(),
                        DoctorsOfficePage_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DoctorsOfficePages", t => t.DoctorsOfficePage_Id)
                .ForeignKey("dbo.DoctorsOfficePages", t => t.DoctorsOfficePage_Id1)
                .Index(t => t.DoctorsOfficePage_Id)
                .Index(t => t.DoctorsOfficePage_Id1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DoctorsOfficePageSections", "DoctorsOfficePage_Id1", "dbo.DoctorsOfficePages");
            DropForeignKey("dbo.DoctorsOfficePageSections", "DoctorsOfficePage_Id", "dbo.DoctorsOfficePages");
            DropIndex("dbo.DoctorsOfficePageSections", new[] { "DoctorsOfficePage_Id1" });
            DropIndex("dbo.DoctorsOfficePageSections", new[] { "DoctorsOfficePage_Id" });
            DropTable("dbo.DoctorsOfficePageSections");
            DropTable("dbo.DoctorsOfficePages");
        }
    }
}
