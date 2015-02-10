namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerId_String : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "CustomerID", c => c.String());
            DropColumn("dbo.Order", "CutomerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "CutomerID", c => c.Int(nullable: false));
            DropColumn("dbo.Order", "CustomerID");
        }
    }
}
