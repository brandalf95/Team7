namespace Team7_LonghornMusic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumReviews",
                c => new
                    {
                        AlbumReviewID = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(),
                        Album_AlbumID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AlbumReviewID)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Album_AlbumID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        AlbumID = c.Int(nullable: false, identity: true),
                        AlbumTitle = c.String(nullable: false),
                        IsFeatured = c.Boolean(nullable: false),
                        AlbumPrice = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.AlbumID);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ArtistID = c.Int(nullable: false, identity: true),
                        ArtistName = c.String(nullable: false),
                        IsFeatured = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ArtistID);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreID = c.Int(nullable: false, identity: true),
                        GenreName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GenreID);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        SongID = c.Int(nullable: false, identity: true),
                        SongTitle = c.String(nullable: false),
                        IsFeatured = c.Boolean(nullable: false),
                        SongPrice = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.SongID);
            
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        DiscountID = c.Int(nullable: false, identity: true),
                        DiscountAmt = c.Single(nullable: false),
                        Album_AlbumID = c.Int(),
                        ShoppingCart_ShoppingCartID = c.Int(),
                        Song_SongID = c.Int(),
                    })
                .PrimaryKey(t => t.DiscountID)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumID)
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCart_ShoppingCartID)
                .ForeignKey("dbo.Songs", t => t.Song_SongID)
                .Index(t => t.Album_AlbumID)
                .Index(t => t.ShoppingCart_ShoppingCartID)
                .Index(t => t.Song_SongID);
            
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
                .PrimaryKey(t => t.ShoppingCartID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailID = c.Int(nullable: false, identity: true),
                        IsGift = c.Boolean(nullable: false),
                        IsConfirmed = c.Boolean(nullable: false),
                        IsRefunded = c.Boolean(nullable: false),
                        ShoppingCart_ShoppingCartID = c.Int(),
                    })
                .PrimaryKey(t => t.OrderDetailID)
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCart_ShoppingCartID)
                .Index(t => t.ShoppingCart_ShoppingCartID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FName = c.String(nullable: false),
                        LName = c.String(nullable: false),
                        MidInitial = c.String(),
                        IsDisabled = c.Boolean(nullable: false),
                        Address = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.Int(nullable: false),
                        ZipCode = c.String(nullable: false),
                        CreditCardOne = c.Int(nullable: false),
                        CreditCardTwo = c.Int(nullable: false),
                        CreditCardTypeOne = c.Int(nullable: false),
                        CreditCardTypeTwo = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SongOrders",
                c => new
                    {
                        SongOrderID = c.Int(nullable: false, identity: true),
                        TrackNumber = c.Int(nullable: false),
                        Album_AlbumID = c.Int(),
                        Song_SongID = c.Int(),
                    })
                .PrimaryKey(t => t.SongOrderID)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumID)
                .ForeignKey("dbo.Songs", t => t.Song_SongID)
                .Index(t => t.Album_AlbumID)
                .Index(t => t.Song_SongID);
            
            CreateTable(
                "dbo.SongReviews",
                c => new
                    {
                        SongReviewID = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(),
                        Song_SongID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SongReviewID)
                .ForeignKey("dbo.Songs", t => t.Song_SongID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Song_SongID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ArtistReviews",
                c => new
                    {
                        ArtistReviewID = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(),
                        Artist_ArtistID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ArtistReviewID)
                .ForeignKey("dbo.Artists", t => t.Artist_ArtistID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Artist_ArtistID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ArtistAlbums",
                c => new
                    {
                        Artist_ArtistID = c.Int(nullable: false),
                        Album_AlbumID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Artist_ArtistID, t.Album_AlbumID })
                .ForeignKey("dbo.Artists", t => t.Artist_ArtistID, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumID, cascadeDelete: true)
                .Index(t => t.Artist_ArtistID)
                .Index(t => t.Album_AlbumID);
            
            CreateTable(
                "dbo.GenreAlbums",
                c => new
                    {
                        Genre_GenreID = c.Int(nullable: false),
                        Album_AlbumID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_GenreID, t.Album_AlbumID })
                .ForeignKey("dbo.Genres", t => t.Genre_GenreID, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumID, cascadeDelete: true)
                .Index(t => t.Genre_GenreID)
                .Index(t => t.Album_AlbumID);
            
            CreateTable(
                "dbo.GenreArtists",
                c => new
                    {
                        Genre_GenreID = c.Int(nullable: false),
                        Artist_ArtistID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_GenreID, t.Artist_ArtistID })
                .ForeignKey("dbo.Genres", t => t.Genre_GenreID, cascadeDelete: true)
                .ForeignKey("dbo.Artists", t => t.Artist_ArtistID, cascadeDelete: true)
                .Index(t => t.Genre_GenreID)
                .Index(t => t.Artist_ArtistID);
            
            CreateTable(
                "dbo.SongArtists",
                c => new
                    {
                        Song_SongID = c.Int(nullable: false),
                        Artist_ArtistID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_SongID, t.Artist_ArtistID })
                .ForeignKey("dbo.Songs", t => t.Song_SongID, cascadeDelete: true)
                .ForeignKey("dbo.Artists", t => t.Artist_ArtistID, cascadeDelete: true)
                .Index(t => t.Song_SongID)
                .Index(t => t.Artist_ArtistID);
            
            CreateTable(
                "dbo.AppUserAlbums",
                c => new
                    {
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                        Album_AlbumID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_Id, t.Album_AlbumID })
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumID, cascadeDelete: true)
                .Index(t => t.AppUser_Id)
                .Index(t => t.Album_AlbumID);
            
            CreateTable(
                "dbo.AppUserSongs",
                c => new
                    {
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                        Song_SongID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_Id, t.Song_SongID })
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Songs", t => t.Song_SongID, cascadeDelete: true)
                .Index(t => t.AppUser_Id)
                .Index(t => t.Song_SongID);
            
            CreateTable(
                "dbo.SongGenres",
                c => new
                    {
                        Song_SongID = c.Int(nullable: false),
                        Genre_GenreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_SongID, t.Genre_GenreID })
                .ForeignKey("dbo.Songs", t => t.Song_SongID, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.Genre_GenreID, cascadeDelete: true)
                .Index(t => t.Song_SongID)
                .Index(t => t.Genre_GenreID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AlbumReviews", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AlbumReviews", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.ArtistReviews", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ArtistReviews", "Artist_ArtistID", "dbo.Artists");
            DropForeignKey("dbo.SongReviews", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SongReviews", "Song_SongID", "dbo.Songs");
            DropForeignKey("dbo.SongOrders", "Song_SongID", "dbo.Songs");
            DropForeignKey("dbo.SongOrders", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.SongGenres", "Genre_GenreID", "dbo.Genres");
            DropForeignKey("dbo.SongGenres", "Song_SongID", "dbo.Songs");
            DropForeignKey("dbo.Discounts", "Song_SongID", "dbo.Songs");
            DropForeignKey("dbo.AppUserSongs", "Song_SongID", "dbo.Songs");
            DropForeignKey("dbo.AppUserSongs", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ShoppingCarts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AppUserAlbums", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.AppUserAlbums", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderDetails", "ShoppingCart_ShoppingCartID", "dbo.ShoppingCarts");
            DropForeignKey("dbo.Discounts", "ShoppingCart_ShoppingCartID", "dbo.ShoppingCarts");
            DropForeignKey("dbo.Discounts", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.SongArtists", "Artist_ArtistID", "dbo.Artists");
            DropForeignKey("dbo.SongArtists", "Song_SongID", "dbo.Songs");
            DropForeignKey("dbo.GenreArtists", "Artist_ArtistID", "dbo.Artists");
            DropForeignKey("dbo.GenreArtists", "Genre_GenreID", "dbo.Genres");
            DropForeignKey("dbo.GenreAlbums", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.GenreAlbums", "Genre_GenreID", "dbo.Genres");
            DropForeignKey("dbo.ArtistAlbums", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.ArtistAlbums", "Artist_ArtistID", "dbo.Artists");
            DropIndex("dbo.SongGenres", new[] { "Genre_GenreID" });
            DropIndex("dbo.SongGenres", new[] { "Song_SongID" });
            DropIndex("dbo.AppUserSongs", new[] { "Song_SongID" });
            DropIndex("dbo.AppUserSongs", new[] { "AppUser_Id" });
            DropIndex("dbo.AppUserAlbums", new[] { "Album_AlbumID" });
            DropIndex("dbo.AppUserAlbums", new[] { "AppUser_Id" });
            DropIndex("dbo.SongArtists", new[] { "Artist_ArtistID" });
            DropIndex("dbo.SongArtists", new[] { "Song_SongID" });
            DropIndex("dbo.GenreArtists", new[] { "Artist_ArtistID" });
            DropIndex("dbo.GenreArtists", new[] { "Genre_GenreID" });
            DropIndex("dbo.GenreAlbums", new[] { "Album_AlbumID" });
            DropIndex("dbo.GenreAlbums", new[] { "Genre_GenreID" });
            DropIndex("dbo.ArtistAlbums", new[] { "Album_AlbumID" });
            DropIndex("dbo.ArtistAlbums", new[] { "Artist_ArtistID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ArtistReviews", new[] { "User_Id" });
            DropIndex("dbo.ArtistReviews", new[] { "Artist_ArtistID" });
            DropIndex("dbo.SongReviews", new[] { "User_Id" });
            DropIndex("dbo.SongReviews", new[] { "Song_SongID" });
            DropIndex("dbo.SongOrders", new[] { "Song_SongID" });
            DropIndex("dbo.SongOrders", new[] { "Album_AlbumID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.OrderDetails", new[] { "ShoppingCart_ShoppingCartID" });
            DropIndex("dbo.ShoppingCarts", new[] { "User_Id" });
            DropIndex("dbo.Discounts", new[] { "Song_SongID" });
            DropIndex("dbo.Discounts", new[] { "ShoppingCart_ShoppingCartID" });
            DropIndex("dbo.Discounts", new[] { "Album_AlbumID" });
            DropIndex("dbo.AlbumReviews", new[] { "User_Id" });
            DropIndex("dbo.AlbumReviews", new[] { "Album_AlbumID" });
            DropTable("dbo.SongGenres");
            DropTable("dbo.AppUserSongs");
            DropTable("dbo.AppUserAlbums");
            DropTable("dbo.SongArtists");
            DropTable("dbo.GenreArtists");
            DropTable("dbo.GenreAlbums");
            DropTable("dbo.ArtistAlbums");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ArtistReviews");
            DropTable("dbo.SongReviews");
            DropTable("dbo.SongOrders");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.ShoppingCarts");
            DropTable("dbo.Discounts");
            DropTable("dbo.Songs");
            DropTable("dbo.Genres");
            DropTable("dbo.Artists");
            DropTable("dbo.Albums");
            DropTable("dbo.AlbumReviews");
        }
    }
}
