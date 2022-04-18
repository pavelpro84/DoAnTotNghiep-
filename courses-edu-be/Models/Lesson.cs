using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace courses_edu_be.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            Question = new HashSet<Question>();
        }

        public Guid LessonId { get; set; }
        public string LessonName { get; set; }
        public string LessonSlug { get; set; }
        public string LessonContent { get; set; }
        public long? LessonDuration { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid CoursesId { get; set; }

        public virtual Courses Courses { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
