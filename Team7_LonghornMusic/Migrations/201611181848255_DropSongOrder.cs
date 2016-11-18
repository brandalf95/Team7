namespace Team7_LonghornMusic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropSongOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SongOrders", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.SongOrders", "Song_SongID", "dbo.Songs");
            DropIndex("dbo.SongOrders", new[] { "Album_AlbumID" });
            DropIndex("dbo.SongOrders", new[] { "Song_SongID" });
            CreateTable(
                "dbo.SongAlbums",
                c => new
                    {
                        Song_SongID = c.Int(nullable: false),
                        Album_AlbumID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_SongID, t.Album_AlbumID })
                .ForeignKey("dbo.Songs", t => t.Song_SongID, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumID, cascadeDelete: true)
                .Index(t => t.Song_SongID)
                .Index(t => t.Album_AlbumID);
            
            DropTable("dbo.SongOrders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SongOrders",
                c => new
                    {
                        SongOrderID = c.Int(nullable: false, identity: true),
                        TrackNumber = c.Int(nullable: false),
                        Album_AlbumID = c.Int(),
                        Song_SongID = c.Int(),
                    })
                .PrimaryKey(t => t.SongOrderID);
            
            DropForeignKey("dbo.SongAlbums", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.SongAlbums", "Song_SongID", "dbo.Songs");
            DropIndex("dbo.SongAlbums", new[] { "Album_AlbumID" });
            DropIndex("dbo.SongAlbums", new[] { "Song_SongID" });
            DropTable("dbo.SongAlbums");
            CreateIndex("dbo.SongOrders", "Song_SongID");
            CreateIndex("dbo.SongOrders", "Album_AlbumID");
            AddForeignKey("dbo.SongOrders", "Song_SongID", "dbo.Songs", "SongID");
            AddForeignKey("dbo.SongOrders", "Album_AlbumID", "dbo.Albums", "AlbumID");
        }
    }
}
