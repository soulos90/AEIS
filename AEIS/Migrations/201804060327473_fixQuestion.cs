namespace StateTemplateV5Beta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question2", "yesPointValue", c => c.Int(nullable: false));
            AddColumn("dbo.Question2", "noPointValue", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Question2", "noPointValue");
            DropColumn("dbo.Question2", "yesPointValue");
        }
    }
}
