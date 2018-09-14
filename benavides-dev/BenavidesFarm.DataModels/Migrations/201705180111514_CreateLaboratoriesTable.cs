namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateLaboratoriesTable : DbMigration
    {
        public override void Up()
        {
            /*CreateTable(
                "dbo.Municipios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Lng = c.String(),
                        Lat = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Estados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Lng = c.String(),
                        Lat = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EstadosMunicipios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Estado_Id = c.Int(),
                        Municipio_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Estados", t => t.Estado_Id)
                .ForeignKey("dbo.Municipios", t => t.Municipio_Id)
                .Index(t => t.Estado_Id)
                .Index(t => t.Municipio_Id);*/
            
            CreateTable(
                "dbo.Laboratories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Active = c.Boolean(nullable: false),
                        City_Id = c.Int(),
                        State_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Municipios", t => t.City_Id)
                .ForeignKey("dbo.Estados", t => t.State_Id)
                .Index(t => t.City_Id)
                .Index(t => t.State_Id);
            
           /* AddColumn("dbo.Branches", "City_Id", c => c.Int());
            AddColumn("dbo.Branches", "State_Id", c => c.Int());
            CreateIndex("dbo.Branches", "City_Id");
            CreateIndex("dbo.Branches", "State_Id");
            AddForeignKey("dbo.Branches", "City_Id", "dbo.Municipios", "Id");
            AddForeignKey("dbo.Branches", "State_Id", "dbo.Estados", "Id");*/
        }
        
        public override void Down()
        {
           /* DropForeignKey("dbo.Laboratories", "State_Id", "dbo.Estados");
            DropForeignKey("dbo.Laboratories", "City_Id", "dbo.Municipios");
            DropForeignKey("dbo.EstadosMunicipios", "Municipio_Id", "dbo.Municipios");
            DropForeignKey("dbo.EstadosMunicipios", "Estado_Id", "dbo.Estados");
            DropForeignKey("dbo.Branches", "State_Id", "dbo.Estados");
            DropForeignKey("dbo.Branches", "City_Id", "dbo.Municipios");*/
            DropIndex("dbo.Laboratories", new[] { "State_Id" });
            DropIndex("dbo.Laboratories", new[] { "City_Id" });
           /* DropIndex("dbo.EstadosMunicipios", new[] { "Municipio_Id" });
            DropIndex("dbo.EstadosMunicipios", new[] { "Estado_Id" });
            DropIndex("dbo.Branches", new[] { "State_Id" });
            DropIndex("dbo.Branches", new[] { "City_Id" });
            DropColumn("dbo.Branches", "State_Id");
            DropColumn("dbo.Branches", "City_Id");*/
            DropTable("dbo.Laboratories");
            /*DropTable("dbo.EstadosMunicipios");
            DropTable("dbo.Estados");
            DropTable("dbo.Municipios");*/
        }
    }
}
