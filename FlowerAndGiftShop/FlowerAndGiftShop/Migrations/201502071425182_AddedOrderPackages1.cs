namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrderPackages1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderPackages", "CustomerID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderPackages", "CustomerID", c => c.Int(nullable: false));
        }
    }
}
