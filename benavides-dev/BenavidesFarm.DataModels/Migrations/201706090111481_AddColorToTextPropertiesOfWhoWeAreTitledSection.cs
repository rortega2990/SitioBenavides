namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColorToTextPropertiesOfWhoWeAreTitledSection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WhoWeAreTitledSections", "TileColor", c => c.String());
            AddColumn("dbo.WhoWeAreTitledSections", "SubTitleColor", c => c.String());
            AddColumn("dbo.WhoWeAreTitledSections", "TextColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WhoWeAreTitledSections", "TextColor");
            DropColumn("dbo.WhoWeAreTitledSections", "SubTitleColor");
            DropColumn("dbo.WhoWeAreTitledSections", "TileColor");
        }
    }
}
