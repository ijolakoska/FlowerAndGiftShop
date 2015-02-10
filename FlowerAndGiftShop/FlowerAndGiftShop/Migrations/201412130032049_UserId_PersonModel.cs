namespace FlowerAndGiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserId_PersonModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "UserID", c => c.String());
            AddColumn("dbo.Employee", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "UserID");
            DropColumn("dbo.Customer", "UserID");
        }
    }
}
