namespace Team7_LonghornMusic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "CreditCardOne", c => c.String());
            AlterColumn("dbo.AspNetUsers", "CreditCardTwo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "CreditCardTwo", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "CreditCardOne", c => c.Int(nullable: false));
        }
    }
}
