namespace ClassicGarageAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVisitorCounterToAds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdsModels", "visitCounter", c => c.Int(nullable: false, defaultValue: 0, defaultValueSql: "0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdsModels", "visitCounter");
        }
    }
}
