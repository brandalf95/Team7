namespace Team7_LonghornMusic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewDiscountModelStuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "DiscountPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Albums", "DisplayPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Songs", "DiscountPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Songs", "DisplayPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "DisplayPrice");
            DropColumn("dbo.Songs", "DiscountPrice");
            DropColumn("dbo.Albums", "DisplayPrice");
            DropColumn("dbo.Albums", "DiscountPrice");
        }
    }
}
