using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace courses_edu_be.Models
{
    public partial class SchoolSubject
    {
        public SchoolSubject()
        {
            CoursesDetail = new HashSet<CoursesDetail>();
            GradeSubjectDetail = new HashSet<GradeSubjectDetail>();
        }

        public Guid SchoolSubjectId { get; set; }
        public string SchoolSubjectName { get; set; }
        public string SchoolSubjectSlug { get; set; }

        public virtual ICollection<CoursesDetail> CoursesDetail { get; set; }
        public virtual ICollection<GradeSubjectDetail> GradeSubjectDetail { get; set; }
    }
}
