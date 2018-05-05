namespace StateTemplateV5Beta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answer2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        programName = c.String(),
                        Value = c.Boolean(nullable: false),
                        Question_Id = c.Int(),
                        User_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Question2", t => t.Question_Id)
                .ForeignKey("dbo.User2", t => t.User_ID)
                .Index(t => t.Question_Id)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.Question2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        reliesOn_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Question2", t => t.reliesOn_Id)
                .Index(t => t.reliesOn_Id);
            
            CreateTable(
                "dbo.User2",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Orginization = c.String(),
                        FName = c.String(),
                        LName = c.String(),
                        Passhash = c.String(),
                        PassSalt = c.String(),
                        Cookie = c.String(),
                        created = c.DateTime(nullable: false),
                        LastUsed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answer2", "User_ID", "dbo.User2");
            DropForeignKey("dbo.Answer2", "Question_Id", "dbo.Question2");
            DropForeignKey("dbo.Question2", "reliesOn_Id", "dbo.Question2");
            DropIndex("dbo.Question2", new[] { "reliesOn_Id" });
            DropIndex("dbo.Answer2", new[] { "User_ID" });
            DropIndex("dbo.Answer2", new[] { "Question_Id" });
            DropTable("dbo.User2");
            DropTable("dbo.Question2");
            DropTable("dbo.Answer2");
        }
    }
}
