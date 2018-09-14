namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateQuotesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quotes",
                c => new
                    {
                        QuoteId = c.Int(nullable: false, identity: true),
                        QuoteAuthor = c.String(),
                        QuoteAuthorSign = c.String(),
                        QuoteAuthorPhoto = c.String(),
                        QuoteText = c.String(),
                    })
                .PrimaryKey(t => t.QuoteId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Quotes");
        }
    }
}
