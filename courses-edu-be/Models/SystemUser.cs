using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace courses_edu_be.Models
{
    public partial class SystemUser
    {
        public Guid SystemUserId { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string UserDob { get; set; }
        public string UserAvatar { get; set; }
    }
}
