namespace FaaS.Migrations.FaaSContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Options", "ElementId", "dbo.Elements");
            DropIndex("dbo.Options", new[] { "ElementId" });
            AddColumn("dbo.Elements", "Options", c => c.String());
            AddColumn("dbo.Elements", "DisplayName", c => c.String(nullable: false));
            AddColumn("dbo.Elements", "CodeName", c => c.String(nullable: false, maxLength: 254));
            AddColumn("dbo.ElementValues", "DisplayName", c => c.String(nullable: false));
            AddColumn("dbo.ElementValues", "CodeName", c => c.String(nullable: false, maxLength: 254));
            AddColumn("dbo.Sessions", "DisplayName", c => c.String(nullable: false));
            AddColumn("dbo.Sessions", "CodeName", c => c.String(nullable: false, maxLength: 254));
            AddColumn("dbo.Forms", "DisplayName", c => c.String(nullable: false));
            AddColumn("dbo.Forms", "CodeName", c => c.String(nullable: false, maxLength: 254));
            AddColumn("dbo.Projects", "DisplayName", c => c.String(nullable: false));
            AddColumn("dbo.Projects", "CodeName", c => c.String(nullable: false, maxLength: 254));
            AddColumn("dbo.Users", "DisplayName", c => c.String(nullable: false));
            AddColumn("dbo.Users", "CodeName", c => c.String(nullable: false, maxLength: 254));
            CreateIndex("dbo.Elements", "CodeName", unique: true);
            CreateIndex("dbo.ElementValues", "CodeName", unique: true);
            CreateIndex("dbo.Sessions", "CodeName", unique: true);
            CreateIndex("dbo.Forms", "CodeName", unique: true);
            CreateIndex("dbo.Projects", "CodeName", unique: true);
            CreateIndex("dbo.Users", "CodeName", unique: true);
            DropColumn("dbo.Elements", "Name");
            DropColumn("dbo.Forms", "Name");
            DropColumn("dbo.Projects", "Name");
            DropTable("dbo.Options");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ElementId = c.Guid(nullable: false),
                        Label = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Projects", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Forms", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Elements", "Name", c => c.String(nullable: false));
            DropIndex("dbo.Users", new[] { "CodeName" });
            DropIndex("dbo.Projects", new[] { "CodeName" });
            DropIndex("dbo.Forms", new[] { "CodeName" });
            DropIndex("dbo.Sessions", new[] { "CodeName" });
            DropIndex("dbo.ElementValues", new[] { "CodeName" });
            DropIndex("dbo.Elements", new[] { "CodeName" });
            DropColumn("dbo.Users", "CodeName");
            DropColumn("dbo.Users", "DisplayName");
            DropColumn("dbo.Projects", "CodeName");
            DropColumn("dbo.Projects", "DisplayName");
            DropColumn("dbo.Forms", "CodeName");
            DropColumn("dbo.Forms", "DisplayName");
            DropColumn("dbo.Sessions", "CodeName");
            DropColumn("dbo.Sessions", "DisplayName");
            DropColumn("dbo.ElementValues", "CodeName");
            DropColumn("dbo.ElementValues", "DisplayName");
            DropColumn("dbo.Elements", "CodeName");
            DropColumn("dbo.Elements", "DisplayName");
            DropColumn("dbo.Elements", "Options");
            CreateIndex("dbo.Options", "ElementId");
            AddForeignKey("dbo.Options", "ElementId", "dbo.Elements", "Id", cascadeDelete: true);
        }
    }
}
