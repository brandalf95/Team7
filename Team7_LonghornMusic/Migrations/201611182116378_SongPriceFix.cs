namespace Team7_LonghornMusic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SongPriceFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Songs", "SongPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Songs", "SongPrice", c => c.Single(nullable: false));
        }
    }
}
