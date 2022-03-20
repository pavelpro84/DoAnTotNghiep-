using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace courses_edu_be.Models
{
    public partial class Grade
    {
        public Grade()
        {
            CoursesDetail = new HashSet<CoursesDetail>();
            GradeSubjectDetail = new HashSet<GradeSubjectDetail>();
        }

        public Guid GradeId { get; set; }
        public string GradeName { get; set; }
        public string GradeSlug { get; set; }

        public virtual ICollection<CoursesDetail> CoursesDetail { get; set; }
        public virtual ICollection<GradeSubjectDetail> GradeSubjectDetail { get; set; }
    }
}
