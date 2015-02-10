namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrderStatus1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "OrderStatusID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "OrderStatusID");
        }
    }
}
