using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace courses_edu_be.Models
{
    public partial class Courses
    {
        public Courses()
        {
            CoursesDetail = new HashSet<CoursesDetail>();
            LessonDetail = new HashSet<LessonDetail>();
        }

        public Guid CoursesId { get; set; }
        public string CoursesName { get; set; }
        public string CoursesNameSlug { get; set; }
        public string CoursesContent { get; set; }
        public int? CoursesNumberOfLesson { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<CoursesDetail> CoursesDetail { get; set; }
        public virtual ICollection<LessonDetail> LessonDetail { get; set; }
    }
}
