using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace courses_edu_be.Models
{
    public partial class LessonDetail
    {
        public Guid LessonDetailId { get; set; }
        public Guid CoursesId { get; set; }
        public Guid LessonId { get; set; }

        public virtual Courses Courses { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}
