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

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

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

            //---------------------------------------------- AppUser
            modelBuilder.Entity<AppUser>()
            .ToTable("AspNetUsers")
            .HasDiscriminator<int>("UserType")
            .HasValue<AppUser>((int)RoleValue.App_user)
            .HasValue<CourseClient>((int)RoleValue.Course_client)
            .HasValue<CourseOwner>((int)RoleValue.Course_owner);

            modelBuilder.Entity<AppUser>()
                .Ignore(au => au.Roles);

            modelBuilder.Entity<CourseOwner>()
                .Property(co => co.CourseInfoEmailsEnabled)
                .IsRequired()
                .HasDefaultValue(true);

            modelBuilder.Entity<CourseClient>()
                .Property(cc => cc.ReviewInfoEmailsEnabled)
                .IsRequired()
                .HasDefaultValue(true);
            //*---------------------------------------------- AppUser

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
                .HasMany(co => co.LearningSkills)
                .WithOne(ls => ls.Course)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .Property(r => r.DateEdited)
                .IsRequired(false);

            modelBuilder.Entity<Course>()
                .Ignore(s => s.AvgRating);
            //*---------------------------------------------- Course

            //----------------------------------------------- Review
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
            //*----------------------------------------------- Review

            modelBuilder.Entity<OwnerComment>()
                .Property(r => r.DateEdited)
                .IsRequired(false);

            //---------------------------------------------- Review Report
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
            //*--------------------------------------------- Review Report

            //---------------------------------------------- Course Owner
            modelBuilder.Entity<CourseOwner>()
                .HasMany(co => co.OwnerComments)
                .WithOne(ow => ow.Author)
                .HasForeignKey(ow => ow.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
            //*---------------------------------------------- Course Owner

            //---------------------------------------------- Category
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(p => p.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.ClientCascade);
            //*---------------------------------------------- Category

            //---------------------------------------------- Helpfull Review
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
            //*---------------------------------------------- Helpfull Review

            //---------------------------------------------- TPT
            modelBuilder.Entity<Review>()
                .ToTable("Reviews");

            modelBuilder.Entity<OwnerComment>()
                .ToTable("OwnerComments");
            //*---------------------------------------------- TPT
        }
    }
}
