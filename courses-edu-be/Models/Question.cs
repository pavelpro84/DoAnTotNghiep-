using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace courses_edu_be.Models
{
    public partial class Question
    {
        public Guid QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string Answer { get; set; }
        public Guid LessonId { get; set; }

        public virtual Lesson Lesson { get; set; }
    }
}
