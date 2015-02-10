namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriceAndPicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flower", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Flower", "Picture", c => c.Binary());
            AddColumn("dbo.Product", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Service", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Service", "Price");
            DropColumn("dbo.Product", "Price");
            DropColumn("dbo.Flower", "Picture");
            DropColumn("dbo.Flower", "Price");
        }
    }
}
