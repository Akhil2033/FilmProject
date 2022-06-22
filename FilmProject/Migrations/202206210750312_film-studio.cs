namespace FilmProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class filmstudio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Films", "StudioId", c => c.Int(nullable: false));
            CreateIndex("dbo.Films", "StudioId");
            AddForeignKey("dbo.Films", "StudioId", "dbo.Studios", "StudioId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Films", "StudioId", "dbo.Studios");
            DropIndex("dbo.Films", new[] { "StudioId" });
            DropColumn("dbo.Films", "StudioId");
        }
    }
}
