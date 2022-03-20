using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace courses_edu_be.Models
{
    public partial class SubCategory
    {
        public Guid SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategorySlug { get; set; }
    }
}
