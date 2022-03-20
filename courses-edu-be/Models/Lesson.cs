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
            LessonDetail = new HashSet<LessonDetail>();
            QuestionDetail = new HashSet<QuestionDetail>();
        }

        public Guid LessonId { get; set; }
        public string LessonName { get; set; }
        public string LessonSlug { get; set; }
        public string LessonContent { get; set; }
        public long? LessonDuration { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<LessonDetail> LessonDetail { get; set; }
        public virtual ICollection<QuestionDetail> QuestionDetail { get; set; }
    }
}
