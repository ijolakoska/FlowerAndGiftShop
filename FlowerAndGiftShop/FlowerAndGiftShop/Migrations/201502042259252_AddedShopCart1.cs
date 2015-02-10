namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedShopCart1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShopCart", "CustomerID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ShopCart", "CustomerID", c => c.String());
        }
    }
}
