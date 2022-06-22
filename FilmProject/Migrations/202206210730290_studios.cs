namespace FilmProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class studios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Studios",
                c => new
                    {
                        StudioId = c.Int(nullable: false, identity: true),
                        StudioName = c.String(),
                        StudioDesc = c.String(),
                    })
                .PrimaryKey(t => t.StudioId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Studios");
        }
    }
}
