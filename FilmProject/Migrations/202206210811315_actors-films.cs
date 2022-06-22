namespace FilmProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actorsfilms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actors",
                c => new
                    {
                        ActorId = c.Int(nullable: false, identity: true),
                        ActorName = c.String(),
                        ActorFee = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActorId);
            
            CreateTable(
                "dbo.FilmActors",
                c => new
                    {
                        Film_FilmId = c.Int(nullable: false),
                        Actor_ActorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Film_FilmId, t.Actor_ActorId })
                .ForeignKey("dbo.Films", t => t.Film_FilmId, cascadeDelete: true)
                .ForeignKey("dbo.Actors", t => t.Actor_ActorId, cascadeDelete: true)
                .Index(t => t.Film_FilmId)
                .Index(t => t.Actor_ActorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilmActors", "Actor_ActorId", "dbo.Actors");
            DropForeignKey("dbo.FilmActors", "Film_FilmId", "dbo.Films");
            DropIndex("dbo.FilmActors", new[] { "Actor_ActorId" });
            DropIndex("dbo.FilmActors", new[] { "Film_FilmId" });
            DropTable("dbo.FilmActors");
            DropTable("dbo.Actors");
        }
    }
}
