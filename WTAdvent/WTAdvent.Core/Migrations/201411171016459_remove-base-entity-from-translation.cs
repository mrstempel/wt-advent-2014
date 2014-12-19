namespace WTAdvent.Core.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removebaseentityfromtranslation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Translations", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Translations", "Id", c => c.Int(nullable: false));
        }
    }
}
