namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinkURLToImageSection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageSections", "Link", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageSections", "Link");
        }
    }
}
