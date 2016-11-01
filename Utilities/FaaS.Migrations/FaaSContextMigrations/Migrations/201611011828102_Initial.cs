namespace FaaS.Migrations.FaaSContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Elements",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        FormId = c.Guid(nullable: false),
                        Description = c.String(),
                        Options = c.String(),
                        Type = c.Int(nullable: false),
                        Required = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forms", t => t.FormId, cascadeDelete: true)
                .Index(t => t.FormId);
            
            CreateTable(
                "dbo.ElementValues",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ElementId = c.Guid(nullable: false),
                        SessionId = c.Guid(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sessions", t => t.SessionId, cascadeDelete: true)
                .ForeignKey("dbo.Elements", t => t.ElementId, cascadeDelete: true)
                .Index(t => t.ElementId)
                .Index(t => t.SessionId);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Filled = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Forms",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 254),
                        ProjectId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 254),
                        UserId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 254),
                        GoogleId = c.String(nullable: false),
                        Registered = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "UserId", "dbo.Users");
            DropForeignKey("dbo.Forms", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Elements", "FormId", "dbo.Forms");
            DropForeignKey("dbo.ElementValues", "ElementId", "dbo.Elements");
            DropForeignKey("dbo.ElementValues", "SessionId", "dbo.Sessions");
            DropIndex("dbo.Projects", new[] { "UserId" });
            DropIndex("dbo.Forms", new[] { "ProjectId" });
            DropIndex("dbo.ElementValues", new[] { "SessionId" });
            DropIndex("dbo.ElementValues", new[] { "ElementId" });
            DropIndex("dbo.Elements", new[] { "FormId" });
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.Forms");
            DropTable("dbo.Sessions");
            DropTable("dbo.ElementValues");
            DropTable("dbo.Elements");
        }
    }
}
