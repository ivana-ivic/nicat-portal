//namespace NiCATPortal.Migrations
//{
//    using System;
//    using System.Data.Entity.Migrations;
    
//    public partial class CVInStudent : DbMigration
//    {
//        public override void Up()
//        {
//            DropForeignKey("dbo.CVs", "Student_Id", "dbo.AspNetUsers");
//            DropIndex("dbo.CVs", new[] { "Student_Id" });
//            AddColumn("dbo.AspNetUsers", "CV_Id", c => c.Int());
//            CreateIndex("dbo.AspNetUsers", "CV_Id");
//            AddForeignKey("dbo.AspNetUsers", "CV_Id", "dbo.CVs", "Id");
//            DropColumn("dbo.CVs", "Student_Id");
//        }
        
//        public override void Down()
//        {
//            AddColumn("dbo.CVs", "Student_Id", c => c.String(maxLength: 128));
//            DropForeignKey("dbo.AspNetUsers", "CV_Id", "dbo.CVs");
//            DropIndex("dbo.AspNetUsers", new[] { "CV_Id" });
//            DropColumn("dbo.AspNetUsers", "CV_Id");
//            CreateIndex("dbo.CVs", "Student_Id");
//            AddForeignKey("dbo.CVs", "Student_Id", "dbo.AspNetUsers", "Id");
//        }
//    }
//}
