using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CourseReviewApp.Model.DataModels;

namespace CourseReviewApp.DAL.EntityFramework
{
    public class AppDbContext : IdentityDbContext<AppUser, Role, int>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<OwnerComment> OwnerComments { get; set; } 
        public DbSet<ReviewReport> ReviewReports { get; set; }
        public DbSet<HelpfullReview> HelpfullReviews { get; set; }
        public DbSet<LearningSkill> LearningSkills { get; set; }
        public DbSet<ObservedCourse> ObservedCourses { get; set; } 

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        public static readonly LoggerFactory _myLoggerFactory =
            new LoggerFactory(new[] {
                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
            });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseLoggerFactory(_myLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //---------------------------------------------- AppUser, CourseOwner, CourseClient
            modelBuilder.Entity<AppUser>()
            .ToTable("AspNetUsers")
            .HasDiscriminator<int>("UserType")
            .HasValue<AppUser>((int)RoleValue.App_user)
            .HasValue<CourseClient>((int)RoleValue.Course_client)
            .HasValue<CourseOwner>((int)RoleValue.Course_owner);

            modelBuilder.Entity<AppUser>()
                .Ignore(au => au.Roles);

            modelBuilder.Entity<AppUser>()
                .Property(au => au.LockoutMessageSent)
                .IsRequired()
                .HasDefaultValue(false);

            modelBuilder.Entity<AppUser>()
                .Property(au => au.UserName)
                .HasMaxLength(20);

            modelBuilder.Entity<AppUser>()
                .Property(au => au.NormalizedUserName)
                .HasMaxLength(20);

            modelBuilder.Entity<AppUser>()
                .Property(au => au.FirstName)
                .HasColumnType("nvarchar(35)");

            modelBuilder.Entity<AppUser>()
                .Property(au => au.LastName)
                .HasColumnType("nvarchar(35)");

            modelBuilder.Entity<AppUser>()
                .Property(au => au.PhoneNumber)
                .HasColumnType("nvarchar(16)");

            modelBuilder.Entity<CourseOwner>()
                .Property(co => co.CourseInfoEmailsEnabled)
                .IsRequired()
                .HasDefaultValue(true);

            modelBuilder.Entity<CourseOwner>()
                .Property(co => co.BrandName)
                .HasColumnType("nvarchar(100)");

            modelBuilder.Entity<CourseClient>()
                .Property(cc => cc.ReviewInfoEmailsEnabled)
                .IsRequired()
                .HasDefaultValue(true);
            //*---------------------------------------------- AppUser, CourseOwner, CourseClient

            //---------------------------------------------- Course
            modelBuilder.Entity<Course>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Owner)
                .WithMany(co => co.Courses)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Reviews)
                .WithOne(r => r.Course)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.LearningSkills)
                .WithOne(ls => ls.Course)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .Property(c => c.DateEdited)
                .IsRequired(false);

            modelBuilder.Entity<Course>()
                .Property(c => c.Name)
                .HasColumnType("nvarchar(70)");

            modelBuilder.Entity<Course>()
                .Property(c => c.ShortDescription)
                .HasColumnType("nvarchar(100)");

            modelBuilder.Entity<Course>()
                .Property(c => c.LongDescription)
                .HasColumnType("nvarchar(4000)");

            modelBuilder.Entity<Course>()
                .Property(c => c.CourseWebsiteUrl)
                .HasColumnType("nvarchar(2048)");

            modelBuilder.Entity<Course>()
                .Property(c => c.Language)
                .HasColumnType("nvarchar(50)");

            modelBuilder.Entity<Course>()
                .Ignore(c => c.AvgRating);
            //*---------------------------------------------- Course

            //----------------------------------------------- Review
            modelBuilder.Entity<Review>()
                .ToTable("Reviews");

            modelBuilder.Entity<Review>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Author)
                .WithMany(cc => cc.Reviews)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.OwnerComment)
                .WithOne(ow => ow.Review)
                .HasForeignKey<OwnerComment>(ow => ow.ReviewId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Review>()
                .Property(r => r.DateEdited)
                .IsRequired(false);

            modelBuilder.Entity<Review>()
                .Property(r => r.Contents)
                .HasColumnType("nvarchar(1000)");
            //*----------------------------------------------- Review

            //----------------------------------------------- OwnerComment
            modelBuilder.Entity<OwnerComment>()
                .ToTable("OwnerComments");

            modelBuilder.Entity<OwnerComment>()
                .Property(oc => oc.DateEdited)
                .IsRequired(false);

            modelBuilder.Entity<OwnerComment>()
                .Property(oc => oc.Contents)
                .HasColumnType("nvarchar(500)");
            //*----------------------------------------------- OwnerComment

            //---------------------------------------------- ReviewReport
            modelBuilder.Entity<ReviewReport>()
                .HasKey(rr => rr.Id);

            modelBuilder.Entity<ReviewReport>()
                .HasOne(rr => rr.Review)
                .WithMany(r => r.ReviewReports)
                .HasForeignKey(rr => rr.ReviewId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<ReviewReport>()
                .HasOne(rr => rr.ReportingUser)
                .WithMany(ru => ru.ReviewReports)
                .HasForeignKey(rr => rr.ReportingUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ReviewReport>()
                .Property(rr => rr.ReportContents)
                .HasColumnType("nvarchar(400)");
            //*--------------------------------------------- ReviewReport

            //---------------------------------------------- CourseOwner
            modelBuilder.Entity<CourseOwner>()
                .HasMany(co => co.OwnerComments)
                .WithOne(ow => ow.Author)
                .HasForeignKey(ow => ow.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
            //*---------------------------------------------- CourseOwner

            //---------------------------------------------- Category
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(p => p.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .HasColumnType("nvarchar(50)");
            //*---------------------------------------------- Category

            //---------------------------------------------- HelpfullReview
            modelBuilder.Entity<HelpfullReview>()
                .HasKey(hr => new { hr.UserId, hr.ReviewId });

            modelBuilder.Entity<HelpfullReview>()
                .HasOne(hr => hr.User)
                .WithMany(c => c.HelpfullReviews)
                .HasForeignKey(hr => hr.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<HelpfullReview>()
                .HasOne(hr => hr.Review)
                .WithMany(r => r.HelpfullReviews)
                .HasForeignKey(hr => hr.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);
            //*---------------------------------------------- HelpfullReview

            //---------------------------------------------- ObservedCourse
            modelBuilder.Entity<ObservedCourse>()
                .HasKey(oc => new { oc.UserId, oc.CourseId });

            modelBuilder.Entity<ObservedCourse>()
                .HasOne(oc => oc.User)
                .WithMany(au => au.ObservedCourses)
                .HasForeignKey(oc => oc.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<ObservedCourse>()
                .HasOne(oc => oc.Course)
                .WithMany(c => c.ObservingUsers)
                .HasForeignKey(oc => oc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            //*---------------------------------------------- ObservedCourse

            //---------------------------------------------- LearningSkill
            modelBuilder.Entity<LearningSkill>()
                .Property(ls => ls.Name)
                .HasColumnType("nvarchar(50)");
            //*---------------------------------------------- LearningSkill
        }
    }
}
