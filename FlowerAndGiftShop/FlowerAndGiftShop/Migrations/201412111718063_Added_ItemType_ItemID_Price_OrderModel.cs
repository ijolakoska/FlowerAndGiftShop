namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_ItemType_ItemID_Price_OrderModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "ItemType", c => c.String());
            AddColumn("dbo.Order", "ItemID", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Order", "TypeOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "TypeOrder", c => c.String());
            DropColumn("dbo.Order", "Price");
            DropColumn("dbo.Order", "ItemID");
            DropColumn("dbo.Order", "ItemType");
        }
    }
}
