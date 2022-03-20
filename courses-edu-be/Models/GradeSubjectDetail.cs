using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace courses_edu_be.Models
{
    public partial class GradeSubjectDetail
    {
        public Guid GradeSubjectDetailId { get; set; }
        public Guid SchoolSubjectId { get; set; }
        public Guid GradeId { get; set; }

        public virtual Grade Grade { get; set; }
        public virtual SchoolSubject SchoolSubject { get; set; }
    }
}
