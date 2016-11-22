namespace Team7_LonghornMusic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShoppingCartStuff : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Discounts", "ShoppingCart_ShoppingCartID", "dbo.ShoppingCarts");
            DropForeignKey("dbo.OrderDetails", "ShoppingCart_ShoppingCartID", "dbo.ShoppingCarts");
            DropForeignKey("dbo.AppUserAlbums", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppUserAlbums", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.ShoppingCarts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppUserSongs", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppUserSongs", "Song_SongID", "dbo.Songs");
            DropIndex("dbo.Discounts", new[] { "ShoppingCart_ShoppingCartID" });
            DropIndex("dbo.ShoppingCarts", new[] { "User_Id" });
            DropIndex("dbo.OrderDetails", new[] { "ShoppingCart_ShoppingCartID" });
            DropIndex("dbo.AppUserAlbums", new[] { "AppUser_Id" });
            DropIndex("dbo.AppUserAlbums", new[] { "Album_AlbumID" });
            DropIndex("dbo.AppUserSongs", new[] { "AppUser_Id" });
            DropIndex("dbo.AppUserSongs", new[] { "Song_SongID" });
            AddColumn("dbo.Discounts", "OrderDetail_OrderDetailID", c => c.Int());
            AddColumn("dbo.OrderDetails", "GifterEmail", c => c.String());
            AddColumn("dbo.OrderDetails", "GifteeEmail", c => c.String());
            AddColumn("dbo.OrderDetails", "CreditCardType", c => c.String());
            AddColumn("dbo.OrderDetails", "CreditCardNumber", c => c.String());
            AddColumn("dbo.OrderDetails", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Discounts", "OrderDetail_OrderDetailID");
            CreateIndex("dbo.OrderDetails", "User_Id");
            AddForeignKey("dbo.Discounts", "OrderDetail_OrderDetailID", "dbo.OrderDetails", "OrderDetailID");
            AddForeignKey("dbo.OrderDetails", "User_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Discounts", "ShoppingCart_ShoppingCartID");
            DropColumn("dbo.OrderDetails", "IsGift");
            DropColumn("dbo.OrderDetails", "ShoppingCart_ShoppingCartID");
            DropTable("dbo.ShoppingCarts");
            DropTable("dbo.AppUserAlbums");
            DropTable("dbo.AppUserSongs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AppUserSongs",
                c => new
                    {
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                        Song_SongID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_Id, t.Song_SongID });
            
            CreateTable(
                "dbo.AppUserAlbums",
                c => new
                    {
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                        Album_AlbumID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_Id, t.Album_AlbumID });
            
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        ShoppingCartID = c.Int(nullable: false, identity: true),
                        SubTotal = c.Single(nullable: false),
                        CartTax = c.Single(nullable: false),
                        CreditCardType = c.Int(nullable: false),
                        CreditCardNumber = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ShoppingCartID);
            
            AddColumn("dbo.OrderDetails", "ShoppingCart_ShoppingCartID", c => c.Int());
            AddColumn("dbo.OrderDetails", "IsGift", c => c.Boolean(nullable: false));
            AddColumn("dbo.Discounts", "ShoppingCart_ShoppingCartID", c => c.Int());
            DropForeignKey("dbo.OrderDetails", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Discounts", "OrderDetail_OrderDetailID", "dbo.OrderDetails");
            DropIndex("dbo.OrderDetails", new[] { "User_Id" });
            DropIndex("dbo.Discounts", new[] { "OrderDetail_OrderDetailID" });
            DropColumn("dbo.OrderDetails", "User_Id");
            DropColumn("dbo.OrderDetails", "CreditCardNumber");
            DropColumn("dbo.OrderDetails", "CreditCardType");
            DropColumn("dbo.OrderDetails", "GifteeEmail");
            DropColumn("dbo.OrderDetails", "GifterEmail");
            DropColumn("dbo.Discounts", "OrderDetail_OrderDetailID");
            CreateIndex("dbo.AppUserSongs", "Song_SongID");
            CreateIndex("dbo.AppUserSongs", "AppUser_Id");
            CreateIndex("dbo.AppUserAlbums", "Album_AlbumID");
            CreateIndex("dbo.AppUserAlbums", "AppUser_Id");
            CreateIndex("dbo.OrderDetails", "ShoppingCart_ShoppingCartID");
            CreateIndex("dbo.ShoppingCarts", "User_Id");
            CreateIndex("dbo.Discounts", "ShoppingCart_ShoppingCartID");
            AddForeignKey("dbo.AppUserSongs", "Song_SongID", "dbo.Songs", "SongID", cascadeDelete: true);
            AddForeignKey("dbo.AppUserSongs", "AppUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ShoppingCarts", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AppUserAlbums", "Album_AlbumID", "dbo.Albums", "AlbumID", cascadeDelete: true);
            AddForeignKey("dbo.AppUserAlbums", "AppUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OrderDetails", "ShoppingCart_ShoppingCartID", "dbo.ShoppingCarts", "ShoppingCartID");
            AddForeignKey("dbo.Discounts", "ShoppingCart_ShoppingCartID", "dbo.ShoppingCarts", "ShoppingCartID");
        }
    }
}
