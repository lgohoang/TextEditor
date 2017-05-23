namespace TextEditor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Word : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageFormats", "isCentimeter", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PageFormats", "isCentimeter");
        }
    }
}
