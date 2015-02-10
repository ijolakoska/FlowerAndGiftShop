namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CutomerID = c.Int(nullable: false),
                        DateOrdered = c.DateTime(nullable: false),
                        Delivery = c.Boolean(nullable: false),
                        PlaceDelivery = c.String(),
                        HasCompleted = c.Boolean(nullable: false),
                        DateCompleted = c.DateTime(nullable: false),
                        TypeOrder = c.String(),
                        Quantity = c.Int(nullable: false),
                        Description = c.String(),
                        Customer_ID = c.Int(),
                        Shop_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customer", t => t.Customer_ID)
                .ForeignKey("dbo.Shop", t => t.Shop_ID)
                .Index(t => t.Customer_ID)
                .Index(t => t.Shop_ID);
            
            CreateTable(
                "dbo.Shop",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        Logo = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Shop_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Shop", t => t.Shop_ID)
                .Index(t => t.Shop_ID);
            
            CreateTable(
                "dbo.Flower",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Quantity = c.Int(nullable: false),
                        Type = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Image = c.String(),
                        Shop_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Shop", t => t.Shop_ID)
                .Index(t => t.Shop_ID);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Quantity = c.Int(nullable: false),
                        Type = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Image = c.String(),
                        Shop_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Shop", t => t.Shop_ID)
                .Index(t => t.Shop_ID);
            
            CreateTable(
                "dbo.Service",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Type = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Image = c.String(),
                        Shop_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Shop", t => t.Shop_ID)
                .Index(t => t.Shop_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Service", "Shop_ID", "dbo.Shop");
            DropForeignKey("dbo.Product", "Shop_ID", "dbo.Shop");
            DropForeignKey("dbo.Order", "Shop_ID", "dbo.Shop");
            DropForeignKey("dbo.Flower", "Shop_ID", "dbo.Shop");
            DropForeignKey("dbo.Employee", "Shop_ID", "dbo.Shop");
            DropForeignKey("dbo.Order", "Customer_ID", "dbo.Customer");
            DropIndex("dbo.Service", new[] { "Shop_ID" });
            DropIndex("dbo.Product", new[] { "Shop_ID" });
            DropIndex("dbo.Flower", new[] { "Shop_ID" });
            DropIndex("dbo.Employee", new[] { "Shop_ID" });
            DropIndex("dbo.Order", new[] { "Shop_ID" });
            DropIndex("dbo.Order", new[] { "Customer_ID" });
            DropTable("dbo.Service");
            DropTable("dbo.Product");
            DropTable("dbo.Flower");
            DropTable("dbo.Employee");
            DropTable("dbo.Shop");
            DropTable("dbo.Order");
            DropTable("dbo.Customer");
        }
    }
}
