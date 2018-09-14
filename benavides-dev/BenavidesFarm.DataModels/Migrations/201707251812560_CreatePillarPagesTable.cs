namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePillarPagesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PillarPages",
                c => new
                    {
                        PillarPageId = c.Int(nullable: false, identity: true),
                        PillarPageTitle = c.String(nullable: false),
                        PillarPageActive = c.Boolean(nullable: false),
                        PillarPageCreatedDate = c.DateTime(nullable: false),
                        PillarPageCustomValue = c.String(nullable: false),
                        PillarPageColorText1 = c.String(),
                        PillarPageText1 = c.String(),
                        PillarPageColorText2 = c.String(),
                        PillarPageText2 = c.String(),
                        PillarPageImage = c.String(),
                    })
                .PrimaryKey(t => t.PillarPageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PillarPages");
        }
    }
}
