namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMailConfigurationToJoinTeamSection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MailConfigurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Port = c.Int(nullable: false),
                        Server = c.String(nullable: false),
                        EnableSSL = c.Boolean(nullable: false),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MailConfigurations");
        }
    }
}
