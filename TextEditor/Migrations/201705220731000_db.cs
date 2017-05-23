namespace TextEditor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PageFormats", "MarginLeft", c => c.Single(nullable: false));
            AlterColumn("dbo.PageFormats", "MarginRight", c => c.Single(nullable: false));
            AlterColumn("dbo.PageFormats", "MarginTop", c => c.Single(nullable: false));
            AlterColumn("dbo.PageFormats", "MarginBottom", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PageFormats", "MarginBottom", c => c.Int(nullable: false));
            AlterColumn("dbo.PageFormats", "MarginTop", c => c.Int(nullable: false));
            AlterColumn("dbo.PageFormats", "MarginRight", c => c.Int(nullable: false));
            AlterColumn("dbo.PageFormats", "MarginLeft", c => c.Int(nullable: false));
        }
    }
}
