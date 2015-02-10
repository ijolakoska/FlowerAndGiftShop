namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedShopCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShopCart",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.String(),
                        ItemID = c.Int(nullable: false),
                        ItemType = c.String(),
                        ItemName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sale = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShopCart");
        }
    }
}
