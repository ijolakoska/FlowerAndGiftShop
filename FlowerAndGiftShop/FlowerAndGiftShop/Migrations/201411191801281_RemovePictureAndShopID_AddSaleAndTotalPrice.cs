namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePictureAndShopID_AddSaleAndTotalPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "Sale", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Flower", "Picture");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Flower", "Picture", c => c.Binary());
            DropColumn("dbo.Order", "TotalPrice");
            DropColumn("dbo.Order", "Sale");
        }
    }
}
