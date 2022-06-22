namespace FilmProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class films : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Films",
                c => new
                    {
                        FilmId = c.Int(nullable: false, identity: true),
                        FilmName = c.String(),
                        FilmYear = c.Int(nullable: false),
                        DirectorName = c.String(),
                        FilmPlot = c.String(),
                    })
                .PrimaryKey(t => t.FilmId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Films");
        }
    }
}
