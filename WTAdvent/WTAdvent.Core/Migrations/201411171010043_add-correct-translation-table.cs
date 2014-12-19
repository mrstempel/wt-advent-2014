namespace WTAdvent.Core.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcorrecttranslationtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Translations",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        DE = c.String(),
                        EN = c.String(),
                        ES = c.String(),
                        FR = c.String(),
                        IT = c.String(),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Translations");
        }
    }
}
