namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrderPackages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderPackages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        DateOrdered = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrderPackages");
        }
    }
}
