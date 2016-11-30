namespace Team7_LonghornMusic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderDetailChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "OrderDetail_OrderDetailID", c => c.Int());
            AddColumn("dbo.Songs", "OrderDetail_OrderDetailID", c => c.Int());
            CreateIndex("dbo.Albums", "OrderDetail_OrderDetailID");
            CreateIndex("dbo.Songs", "OrderDetail_OrderDetailID");
            AddForeignKey("dbo.Albums", "OrderDetail_OrderDetailID", "dbo.OrderDetails", "OrderDetailID");
            AddForeignKey("dbo.Songs", "OrderDetail_OrderDetailID", "dbo.OrderDetails", "OrderDetailID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Songs", "OrderDetail_OrderDetailID", "dbo.OrderDetails");
            DropForeignKey("dbo.Albums", "OrderDetail_OrderDetailID", "dbo.OrderDetails");
            DropIndex("dbo.Songs", new[] { "OrderDetail_OrderDetailID" });
            DropIndex("dbo.Albums", new[] { "OrderDetail_OrderDetailID" });
            DropColumn("dbo.Songs", "OrderDetail_OrderDetailID");
            DropColumn("dbo.Albums", "OrderDetail_OrderDetailID");
        }
    }
}
