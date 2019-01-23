namespace ClassicGarageAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevertChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdsModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        title = c.String(nullable: false),
                        price = c.Double(nullable: false),
                        CarModelsID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CarModels", t => t.CarModelsID, cascadeDelete: true)
                .Index(t => t.CarModelsID);
            
            CreateTable(
                "dbo.CarModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Make = c.String(nullable: false),
                        Model = c.String(nullable: false),
                        Year = c.Int(nullable: false),
                        VIN = c.String(nullable: false),
                        Picture = c.String(),
                        PurchaseDate = c.DateTime(nullable: false),
                        PurchaseAmount = c.Double(nullable: false),
                        SalesDate = c.DateTime(nullable: false),
                        SalesAmount = c.Double(nullable: false),
                        OwnerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OwnerModels", t => t.OwnerID, cascadeDelete: true)
                .Index(t => t.OwnerID);
            
            CreateTable(
                "dbo.OwnerModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 45),
                        LastName = c.String(nullable: false, maxLength: 45),
                        PhoneNumber = c.String(maxLength: 9),
                        Email = c.String(nullable: false),
                        UserID = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PartModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PartName = c.String(nullable: false),
                        CatalogNumber = c.String(),
                        PurchaseAmount = c.Double(nullable: false),
                        SalesAmount = c.Double(nullable: false),
                        SalesDate = c.DateTime(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                        CarModelsID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CarModels", t => t.CarModelsID, cascadeDelete: true)
                .Index(t => t.CarModelsID);
            
            CreateTable(
                "dbo.RepairModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        CostAmount = c.Double(nullable: false),
                        CarModelID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CarModels", t => t.CarModelID, cascadeDelete: true)
                .Index(t => t.CarModelID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdsModels", "CarModelsID", "dbo.CarModels");
            DropForeignKey("dbo.RepairModels", "CarModelID", "dbo.CarModels");
            DropForeignKey("dbo.PartModels", "CarModelsID", "dbo.CarModels");
            DropForeignKey("dbo.CarModels", "OwnerID", "dbo.OwnerModels");
            DropIndex("dbo.RepairModels", new[] { "CarModelID" });
            DropIndex("dbo.PartModels", new[] { "CarModelsID" });
            DropIndex("dbo.CarModels", new[] { "OwnerID" });
            DropIndex("dbo.AdsModels", new[] { "CarModelsID" });
            DropTable("dbo.RepairModels");
            DropTable("dbo.PartModels");
            DropTable("dbo.OwnerModels");
            DropTable("dbo.CarModels");
            DropTable("dbo.AdsModels");
        }
    }
}
