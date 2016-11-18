namespace Team7_LonghornMusic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecimalFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Albums", "AlbumPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Albums", "AlbumPrice", c => c.Single(nullable: false));
        }
    }
}
