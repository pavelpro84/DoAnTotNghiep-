using courses_edu_be.Model.CustomModel;
using courses_edu_be.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.courses_edu_be
{
    public interface IJwtAuthenticationManager
    {
        Task<ServiceResponse> LoginAuthenticate(CoursesEduContext _db, string username, string password);
    }
}
