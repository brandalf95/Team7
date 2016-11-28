namespace Team7_LonghornMusic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCartAddition : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Discounts", "DiscountAmt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Discounts", "DiscountAmt", c => c.Single(nullable: false));
        }
    }
}
