namespace TextEditor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PageFormats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MarginLeft = c.Int(nullable: false),
                        MarginRight = c.Int(nullable: false),
                        MarginTop = c.Int(nullable: false),
                        MarginBottom = c.Int(nullable: false),
                        PaperType = c.String(),
                        FontFamily = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PagePropertiesFormats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PageId = c.Int(nullable: false),
                        Row = c.Int(nullable: false),
                        Name = c.String(),
                        Size = c.Int(nullable: false),
                        Bold = c.Boolean(nullable: false),
                        Italic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.FileTables", "PageId", c => c.Int(nullable: false));
            AlterColumn("dbo.FileTables", "Time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FileTables", "Time", c => c.String());
            DropColumn("dbo.FileTables", "PageId");
            DropTable("dbo.PagePropertiesFormats");
            DropTable("dbo.PageFormats");
        }
    }
}
