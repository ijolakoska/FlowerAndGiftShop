namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedQuantityInServiceAndShopcart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Service", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.ShopCart", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopCart", "Quantity");
            DropColumn("dbo.Service", "Quantity");
        }
    }
}
