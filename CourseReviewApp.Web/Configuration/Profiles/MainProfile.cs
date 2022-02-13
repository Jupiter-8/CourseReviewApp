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
            CreateMap<Course, BaseCourseVm>()
                .IncludeAllDerived()
                .ForMember(dest => dest.OwnerFullName, x => x.MapFrom(src => $"{src.Owner.FirstName} {src.Owner.LastName}"));

            CreateMap<Course, CourseLessDetailsVm>()
                .IncludeAllDerived();

            CreateMap<Course, CourseFullDetailsVm>();
            CreateMap<AddOrEditCourseVm, Course>();
            CreateMap<Course, AddOrEditCourseVm>();
            CreateMap<LearningSkill, LearningSkillVm>();
            CreateMap<LearningSkillVm, LearningSkill>();

            CreateMap<Course, ChangeCourseStatusVm>()
                .ForMember(dest => dest.OwnerName, x => x.MapFrom(src => $"{src.Owner.FirstName} {src.Owner.LastName}"))
                .ForMember(dest => dest.OwnerHasCourseInfoEmailsEnabled, x => x.MapFrom(src => src.Owner.CourseInfoEmailsEnabled));

            CreateMap<Course, DeleteCourseVm>()
                .ForMember(dest => dest.OwnerName, x => x.MapFrom(src => $"{src.Owner.FirstName} {src.Owner.LastName}"))
                .ForMember(dest => dest.OwnerHasCourseInfoEmailsEnabled, x => x.MapFrom(src => src.Owner.CourseInfoEmailsEnabled));

            CreateMap<ObservedCourse, ObservedCourseVm>()
                .ForMember(dest => dest.CourseCategoryName, x => x.MapFrom(src => src.Course.Category != null ? src.Course.Category.Name : string.Empty))
                .ForMember(dest => dest.IsCourseActive, x => x.MapFrom(src => src.Course.Status == CourseStatus.Active ? true : false));
            //--- Course

            //--- Category
            CreateMap<Category, CategoryVm>()
                .ForMember(dest => dest.CoursesCount, x => x.MapFrom(
                    src => src.ParentCategoryId.HasValue ? src.Courses.Count : src.SubCategories.Sum(sc => sc.Courses.Count)))
                .ForMember(dest => dest.ActiveCoursesCount, x => x.MapFrom(src => src.ParentCategoryId.HasValue ?
                    src.Courses.Where(c => c.Status == CourseStatus.Active).Count()
                    : src.SubCategories.Sum(sc => sc.Courses.Where(c => c.Status == CourseStatus.Active).Count())));

            CreateMap<Category, AddOrEditCategoryVm>();
            CreateMap<AddOrEditCategoryVm, Category>();
            //--- Category

            //--- Review and report
            CreateMap<AddOrEditReviewVm, Review>();
            CreateMap<ReviewVm, Review>();
            CreateMap<ReportReviewVm, ReviewReport>();
            CreateMap<OwnerComment, AddOrEditOwnerCommentVm>();
            CreateMap<AddOrEditOwnerCommentVm, OwnerComment>();
            CreateMap<HelpfullReview, HelpfullReviewVm>();
            CreateMap<Review, AddOrEditReviewVm>();
            CreateMap<Review, DeleteReviewVm>();

            CreateMap<Review, ReviewVm>()
                .IncludeAllDerived()
                .ForMember(dest => dest.WasHelpfullCount, x => x.MapFrom(src => src.HelpfullReviews.Count));

            CreateMap<ReviewReport, ReviewReportVm>()
                .ForMember(dest => dest.ReportingUserName, x => x.MapFrom(src => $"{src.ReportingUser.FirstName} {src.ReportingUser.LastName}"));

            CreateMap<OwnerComment, OwnerCommentVm>()
                .IncludeAllDerived()
                .ForMember(dest => dest.AuthorName, x => x.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"));

            CreateMap<OwnerComment, DeleteOwnerCommentVm>();
            //--- Review and report

            //--- User
            CreateMap<AppUser, UserVm>()
                .IncludeAllDerived()
                .ForMember(dest => dest.FullName, x => x.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<CourseOwner, UserVm>()
                .ForMember(dest => dest.AvgCoursesRating, x => x.MapFrom(src => Math.Round(src.Courses.Average(c => c.AvgRating), 2)))
                .ForMember(dest => dest.CoursesReviewsCount, x => x.MapFrom(src => src.Courses.Sum(c => c.Reviews.Count)));

            CreateMap<CourseClient, UserVm>()
                .ForMember(dest => dest.UserWasHelpfullCount, x => x.MapFrom(src => src.Reviews.Where(r => r.HelpfullReviews.Count != 0).Count()));

            CreateMap<AppUser, EditUserVm>()
                .ForMember(dest => dest.FullName, x => x.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            //--- User
        }
    }
}
