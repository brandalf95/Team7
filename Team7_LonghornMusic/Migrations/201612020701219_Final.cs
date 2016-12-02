namespace Team7_LonghornMusic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Final : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AlbumReviews", "Comment", c => c.String(maxLength: 100));
            AlterColumn("dbo.ArtistReviews", "Comment", c => c.String(maxLength: 100));
            AlterColumn("dbo.SongReviews", "Comment", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SongReviews", "Comment", c => c.String());
            AlterColumn("dbo.ArtistReviews", "Comment", c => c.String());
            AlterColumn("dbo.AlbumReviews", "Comment", c => c.String());
        }
    }
}
