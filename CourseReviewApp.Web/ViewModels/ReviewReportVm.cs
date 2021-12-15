using CourseReviewApp.Model.DataModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class ReviewReportVm
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public int? ReportingUserId { get; set; }
        public ReviewVm Review { get; set; }

        [Display(Name = "Contents")]
        public string ReportContents { get; set; }

        [Display(Name = "Date added")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Reporting user")]
        public string ReportingUserName { get; set; }

        [Display(Name = "Reason")]
        public ReportReason ReportReason { get; set; }
    }
} 
