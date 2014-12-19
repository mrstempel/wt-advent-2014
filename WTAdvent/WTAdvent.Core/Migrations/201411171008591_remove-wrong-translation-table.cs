namespace WTAdvent.Core.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removewrongtranslationtable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Translations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Translations",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        DE = c.String(),
                        EN = c.String(),
                        FR = c.String(),
                        ES = c.String(),
                        IT = c.String(),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
        }
    }
}
