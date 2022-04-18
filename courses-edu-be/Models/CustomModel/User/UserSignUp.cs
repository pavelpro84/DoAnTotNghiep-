using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace courses_edu_be.Models.CustomModel.User
{
    public class UserSignUp
    {
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string UserDob { get; set; }
        public string UserAvatar { get; set; }
        public int UserRoleType { get; set; }
    }
}
