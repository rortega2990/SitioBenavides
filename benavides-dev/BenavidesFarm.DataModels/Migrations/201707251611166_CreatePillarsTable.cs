namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePillarsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pillars",
                c => new
                    {
                        PillarId = c.Int(nullable: false, identity: true),
                        PillarName = c.String(),
                        PillarActive = c.Boolean(nullable: false),
                        PillarDescription = c.String(),
                        PillarLink = c.String(),
                        PillarImage = c.String(),
                    })
                .PrimaryKey(t => t.PillarId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pillars");
        }
    }
}
