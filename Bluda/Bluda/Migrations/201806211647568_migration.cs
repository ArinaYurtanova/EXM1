namespace ClassLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bludas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameBluda = c.String(),
                        TypeBluda = c.String(),
                        DateCreate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Produckts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdBluda = c.Int(nullable: false),
                        ProductName = c.String(),
                        Count = c.Int(nullable: false),
                        DatePostavki = c.DateTime(nullable: false),
                        PlaceProizvod = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bludas", t => t.IdBluda, cascadeDelete: true)
                .Index(t => t.IdBluda);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Produckts", "IdBluda", "dbo.Bludas");
            DropIndex("dbo.Produckts", new[] { "IdBluda" });
            DropTable("dbo.Produckts");
            DropTable("dbo.Bludas");
        }
    }
}
