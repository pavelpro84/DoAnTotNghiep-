using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace courses_edu_be.Models
{
    public partial class QuestionDetail
    {
        public Guid QuestionDetailId { get; set; }
        public Guid LessonId { get; set; }
        public Guid? QuestionId { get; set; }

        public virtual Lesson Lesson { get; set; }
        public virtual Question Question { get; set; }
    }
}
