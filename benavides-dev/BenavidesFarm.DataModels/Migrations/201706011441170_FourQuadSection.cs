namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourQuadSection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FourQuadSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quad1_Text = c.String(),
                        Quad1_Link = c.String(),
                        Quad1_ImageFileName = c.String(),
                        Quad1_BackgroundColor = c.String(),
                        Quad2_Text = c.String(),
                        Quad2_Link = c.String(),
                        Quad2_ImageFileName = c.String(),
                        Quad2_BackgroundColor = c.String(),
                        Quad3_Text = c.String(),
                        Quad3_Link = c.String(),
                        Quad3_ImageFileName = c.String(),
                        Quad3_BackgroundColor = c.String(),
                        Quad4_Text = c.String(),
                        Quad4_Link = c.String(),
                        Quad4_ImageFileName = c.String(),
                        Quad4_BackgroundColor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.HomePages", "FourQuadSectionId", c => c.Int(nullable: false));
            CreateIndex("dbo.HomePages", "FourQuadSectionId");
            AddForeignKey("dbo.HomePages", "FourQuadSectionId", "dbo.FourQuadSections", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HomePages", "FourQuadSectionId", "dbo.FourQuadSections");
            DropIndex("dbo.HomePages", new[] { "FourQuadSectionId" });
            DropColumn("dbo.HomePages", "FourQuadSectionId");
            DropTable("dbo.FourQuadSections");
        }
    }
}
