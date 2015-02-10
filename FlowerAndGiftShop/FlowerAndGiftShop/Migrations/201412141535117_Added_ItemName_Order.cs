namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_ItemName_Order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "ItemName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "ItemName");
        }
    }
}
