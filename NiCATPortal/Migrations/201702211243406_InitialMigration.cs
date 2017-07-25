//namespace NiCATPortal.Migrations
//{
//    using System;
//    using System.Data.Entity.Migrations;
    
//    public partial class InitialMigration : DbMigration
//    {
//        public override void Up()
//        {
//            CreateTable(
//                "dbo.Courses",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        Name = c.String(nullable: false, maxLength: 50),
//                        Year = c.Int(nullable: false),
//                    })
//                .PrimaryKey(t => t.Id);
            
//            CreateTable(
//                "dbo.Evaluations",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        Mark = c.Int(nullable: false),
//                        Course_Id = c.Int(),
//                        Student_Id = c.String(maxLength: 128),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.Courses", t => t.Course_Id)
//                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id)
//                .Index(t => t.Course_Id)
//                .Index(t => t.Student_Id);
            
//            CreateTable(
//                "dbo.AspNetUsers",
//                c => new
//                    {
//                        Id = c.String(nullable: false, maxLength: 128),
//                        Name = c.String(),
//                        LastName = c.String(),
//                        Email = c.String(maxLength: 256),
//                        EmailConfirmed = c.Boolean(nullable: false),
//                        PasswordHash = c.String(),
//                        SecurityStamp = c.String(),
//                        PhoneNumber = c.String(),
//                        PhoneNumberConfirmed = c.Boolean(nullable: false),
//                        TwoFactorEnabled = c.Boolean(nullable: false),
//                        LockoutEndDateUtc = c.DateTime(),
//                        LockoutEnabled = c.Boolean(nullable: false),
//                        AccessFailedCount = c.Int(nullable: false),
//                        UserName = c.String(nullable: false, maxLength: 256),
//                        Discriminator = c.String(nullable: false, maxLength: 128),
//                    })
//                .PrimaryKey(t => t.Id)
//                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
//            CreateTable(
//                "dbo.AspNetUserClaims",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        UserId = c.String(nullable: false, maxLength: 128),
//                        ClaimType = c.String(),
//                        ClaimValue = c.String(),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
//                .Index(t => t.UserId);
            
//            CreateTable(
//                "dbo.Homework",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        FileName = c.String(nullable: false, maxLength: 100),
//                        Course_Id = c.Int(),
//                        Student_Id = c.String(maxLength: 128),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.Courses", t => t.Course_Id)
//                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id)
//                .Index(t => t.Course_Id)
//                .Index(t => t.Student_Id);
            
//            CreateTable(
//                "dbo.AspNetUserLogins",
//                c => new
//                    {
//                        LoginProvider = c.String(nullable: false, maxLength: 128),
//                        ProviderKey = c.String(nullable: false, maxLength: 128),
//                        UserId = c.String(nullable: false, maxLength: 128),
//                    })
//                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
//                .Index(t => t.UserId);
            
//            CreateTable(
//                "dbo.AspNetUserRoles",
//                c => new
//                    {
//                        UserId = c.String(nullable: false, maxLength: 128),
//                        RoleId = c.String(nullable: false, maxLength: 128),
//                    })
//                .PrimaryKey(t => new { t.UserId, t.RoleId })
//                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
//                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
//                .Index(t => t.UserId)
//                .Index(t => t.RoleId);
            
//            CreateTable(
//                "dbo.Literatures",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        FileName = c.String(nullable: false, maxLength: 100),
//                        Course_Id = c.Int(),
//                        Teacher_Id = c.String(maxLength: 128),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.Courses", t => t.Course_Id)
//                .ForeignKey("dbo.AspNetUsers", t => t.Teacher_Id)
//                .Index(t => t.Course_Id)
//                .Index(t => t.Teacher_Id);
            
//            CreateTable(
//                "dbo.CVs",
//                c => new
//                    {
//                        Id = c.Int(nullable: false, identity: true),
//                        FileName = c.String(nullable: false, maxLength: 100),
//                        Student_Id = c.String(maxLength: 128),
//                    })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id)
//                .Index(t => t.Student_Id);
            
//            CreateTable(
//                "dbo.AspNetRoles",
//                c => new
//                    {
//                        Id = c.String(nullable: false, maxLength: 128),
//                        Name = c.String(nullable: false, maxLength: 256),
//                    })
//                .PrimaryKey(t => t.Id)
//                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
//            CreateTable(
//                "dbo.StudentCourses",
//                c => new
//                    {
//                        Student_Id = c.String(nullable: false, maxLength: 128),
//                        Course_Id = c.Int(nullable: false),
//                    })
//                .PrimaryKey(t => new { t.Student_Id, t.Course_Id })
//                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id, cascadeDelete: true)
//                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
//                .Index(t => t.Student_Id)
//                .Index(t => t.Course_Id);
            
//            CreateTable(
//                "dbo.TeacherCourses",
//                c => new
//                    {
//                        Teacher_Id = c.String(nullable: false, maxLength: 128),
//                        Course_Id = c.Int(nullable: false),
//                    })
//                .PrimaryKey(t => new { t.Teacher_Id, t.Course_Id })
//                .ForeignKey("dbo.AspNetUsers", t => t.Teacher_Id, cascadeDelete: true)
//                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
//                .Index(t => t.Teacher_Id)
//                .Index(t => t.Course_Id);
            
//        }
        
//        public override void Down()
//        {
//            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
//            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
//            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
//            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
//            DropForeignKey("dbo.CVs", "Student_Id", "dbo.AspNetUsers");
//            DropForeignKey("dbo.Literatures", "Teacher_Id", "dbo.AspNetUsers");
//            DropForeignKey("dbo.TeacherCourses", "Course_Id", "dbo.Courses");
//            DropForeignKey("dbo.TeacherCourses", "Teacher_Id", "dbo.AspNetUsers");
//            DropForeignKey("dbo.Literatures", "Course_Id", "dbo.Courses");
//            DropForeignKey("dbo.Homework", "Student_Id", "dbo.AspNetUsers");
//            DropForeignKey("dbo.Homework", "Course_Id", "dbo.Courses");
//            DropForeignKey("dbo.Evaluations", "Student_Id", "dbo.AspNetUsers");
//            DropForeignKey("dbo.StudentCourses", "Course_Id", "dbo.Courses");
//            DropForeignKey("dbo.StudentCourses", "Student_Id", "dbo.AspNetUsers");
//            DropForeignKey("dbo.Evaluations", "Course_Id", "dbo.Courses");
//            DropIndex("dbo.TeacherCourses", new[] { "Course_Id" });
//            DropIndex("dbo.TeacherCourses", new[] { "Teacher_Id" });
//            DropIndex("dbo.StudentCourses", new[] { "Course_Id" });
//            DropIndex("dbo.StudentCourses", new[] { "Student_Id" });
//            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
//            DropIndex("dbo.CVs", new[] { "Student_Id" });
//            DropIndex("dbo.Literatures", new[] { "Teacher_Id" });
//            DropIndex("dbo.Literatures", new[] { "Course_Id" });
//            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
//            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
//            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
//            DropIndex("dbo.Homework", new[] { "Student_Id" });
//            DropIndex("dbo.Homework", new[] { "Course_Id" });
//            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
//            DropIndex("dbo.AspNetUsers", "UserNameIndex");
//            DropIndex("dbo.Evaluations", new[] { "Student_Id" });
//            DropIndex("dbo.Evaluations", new[] { "Course_Id" });
//            DropTable("dbo.TeacherCourses");
//            DropTable("dbo.StudentCourses");
//            DropTable("dbo.AspNetRoles");
//            DropTable("dbo.CVs");
//            DropTable("dbo.Literatures");
//            DropTable("dbo.AspNetUserRoles");
//            DropTable("dbo.AspNetUserLogins");
//            DropTable("dbo.Homework");
//            DropTable("dbo.AspNetUserClaims");
//            DropTable("dbo.AspNetUsers");
//            DropTable("dbo.Evaluations");
//            DropTable("dbo.Courses");
//        }
//    }
//}
