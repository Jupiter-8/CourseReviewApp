using AutoMapper;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Web.ViewModels;
using System;
using System.Linq;

namespace CourseReviewApp.Web.Configuration.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            //--- Course
            CreateMap<Course, CourseVm>()
                .ForMember(dest => dest.CategoryName, x => x.MapFrom(src => src.Category.Name));

            CreateMap<AddOrEditCourseVm, Course>();
            CreateMap<Course, AddOrEditCourseVm>();
            CreateMap<LearningSkill, LearningSkillVm>();
            CreateMap<LearningSkillVm, LearningSkill>();

            CreateMap<Course, ChangeCourseStatusVm>()
                .ForMember(dest => dest.OwnerId, x => x.MapFrom(src => src.Owner.Id))
                .ForMember(dest => dest.OwnerEmail, x => x.MapFrom(src => src.Owner.Email))
                .ForMember(dest => dest.OwnerHasCourseInfoEmailsEnabled, x => x.MapFrom(src => src.Owner.CourseInfoEmailsEnabled));

            CreateMap<Course, DeleteCourseVm>()
                .ForMember(dest => dest.OwnerEmail, x => x.MapFrom(src => src.Owner.Email))
                .ForMember(dest => dest.OwnerName, x => x.MapFrom(src => $"{src.Owner.FirstName} {src.Owner.LastName}"))
                .ForMember(dest => dest.OwnerId, x => x.MapFrom(src => src.Owner.Id))
                .ForMember(dest => dest.CategoryName, x => x.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.OwnerHasCourseInfoEmailsEnabled, x => x.MapFrom(src => src.Owner.CourseInfoEmailsEnabled));
            //--- Course

            //--- Category
            CreateMap<Category, CategoryVm>()
                .ForMember(dest => dest.ParentCategoryName, x => x.MapFrom(src => src.ParentCategory.Name))
                .ForMember(dest => dest.CoursesCount, x => x.MapFrom(
                    src => src.ParentCategoryId.HasValue ? src.Courses.Count : src.SubCategories.Sum(sc => sc.Courses.Count)))
                .ForMember(dest => dest.ActiveCoursesCount, x => x.MapFrom(src => src.ParentCategoryId.HasValue ? 
                    src.Courses.Where(c => c.Status == CourseStatus.Active).Count() 
                    : src.SubCategories.Sum(sc => sc.Courses.Where(c => c.Status == CourseStatus.Active).Count())));

            CreateMap<Category, AddOrEditCategoryVm>();
            CreateMap<AddOrEditCategoryVm, Category>();
            //--- Category

            //--- Review and report
            CreateMap<Review, AddOrEditReviewVm>();
            CreateMap<AddOrEditReviewVm, Review>();
            CreateMap<ReviewVm, Review>();
            CreateMap<ReportReviewVm, ReviewReport>();
            CreateMap<OwnerComment, AddOrEditOwnerCommentVm>();
            CreateMap<AddOrEditOwnerCommentVm, OwnerComment>();
            CreateMap<HelpfullReview, HelpfullReviewVm>();

            CreateMap<Review, ReviewVm>()
                .ForMember(dest => dest.WasHelpfullCount, x => x.MapFrom(src => src.HelpfullReviews.Count))
                .ForMember(dest => dest.CourseOwnerId, x => x.MapFrom(src => src.Course.OwnerId))
                .ForMember(dest => dest.CourseName, x => x.MapFrom(src => src.Course.Name));

            CreateMap<ReviewReport, ReviewReportVm>()
                .ForMember(dest => dest.ReportingUserName, x => x.MapFrom(src => $"{src.ReportingUser.FirstName} {src.ReportingUser.LastName}"));

            CreateMap<OwnerComment, OwnerCommentVm>()
                .ForMember(dest => dest.AuthorName, x => x.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"));
            //--- Review and report

            CreateMap<AppUser, UserVm>()
                .ForMember(dest => dest.FullName, x => x.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            //--- User
            CreateMap<CourseOwner, UserVm>()
                .ForMember(dest => dest.AvgCoursesRating, x => x.MapFrom(src => Math.Round(src.Courses.Average(c => c.AvgRating), 2)))
                .ForMember(dest => dest.CoursesReviewsCount, x => x.MapFrom(src => src.Courses.Sum(c => c.Reviews.Count)))
                .ForMember(dest => dest.FullName, x => x.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<CourseClient, UserVm>()
                .ForMember(dest => dest.FullName, x => x.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.ReviewsCount, x => x.MapFrom(src => src.Reviews.Count))
                .ForMember(dest => dest.UserWasHelpfullCount, x => x.MapFrom(src => src.Reviews.Where(r => r.HelpfullReviews.Count != 0).Count()));

            CreateMap<AppUser, EditUserVm>()
                .ForMember(dest => dest.FullName, x => x.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            //--- User
        }
    }
}
