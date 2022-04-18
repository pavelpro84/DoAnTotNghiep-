using courses_edu_be.Constants;
using courses_edu_be.Model.CustomModel;
using courses_edu_be.Models;
using courses_edu_be.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace courses_edu_be.Controllers
{
    [Route("api/grade")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly CoursesEduContext _db;
        string sql_get_grade = "select * from Grade where CHARINDEX(@txtSeach, GradeName) > 0 or CHARINDEX(@txtSeach, GradeSlug) > 0";

        public GradeController(CoursesEduContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Lấy danh sách lớp có phân trang và cho phép tìm kiếm
        /// </summary>
        /// <returns></returns>
        /// https://localhost:44335/api/grade?page=2&record=10&search=admin
        [HttpGet]
        public async Task<ServiceResponse> GetAccountsByPagingAndSearch([FromQuery] string search, [FromQuery] int? page = 1, [FromQuery] int? record = 10)
        {
            ServiceResponse res = new ServiceResponse();
            try
            {
                List<Grade> records = new List<Grade>();

                if (search != null && search.Trim() != "")
                {
                    var param = new SqlParameter("@txtSeach", search);
                    records = _db.Grade.FromSqlRaw(sql_get_grade, param).OrderByDescending(x => x.GradeName).ToList();
                }
                else
                {
                    records = await _db.Grade.OrderByDescending(x => x.GradeName).ToListAsync();
                }

                res.Success = true;
                res.Data = new PagingData()
                {
                    TotalRecord = records.Count(),
                    TotalPage = Convert.ToInt32(Math.Ceiling((decimal)records.Count() / (decimal)record.Value)),
                    Data = records.Skip((page.Value - 1) * record.Value).Take(record.Value).ToList(),
                };
                res.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                res = ErrorHandler.ErrorCatchResponse(e);
            }
            return res;
        }

        /// <summary>
        /// Lấy chi tiết thông tin lớp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<ServiceResponse> GetCategoryDetail(Guid? id)
        {
            ServiceResponse res = new ServiceResponse();
            var grade = await _db.Grade.FindAsync(id);
            if (grade == null)
            {
                return ErrorHandler.NotFoundResponse(Message.GradegNotFound);
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("grade", grade);

            res.Data = result;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }

        /// <summary>
        /// Create lớp
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ServiceResponse> CreateGrade(Grade grade)
        {
            ServiceResponse res = new ServiceResponse();
            if (!Helper.CheckPermission(HttpContext, "admin"))
            {
                return ErrorHandler.UnauthorizeCatchResponse();
            }

            if (string.IsNullOrEmpty(grade.GradeName))
            {
                return ErrorHandler.BadRequestResponse(Message.GradeNameEmpty);
            }

            grade.GradeId = Guid.NewGuid();
            grade.GradeName = grade.GradeName.Trim();
            if (string.IsNullOrEmpty(grade.GradeSlug))
            {
                grade.GradeSlug = StringUtils.Slugify(grade.GradeName);
            }
            else
            {
                var grade_check_slug = await _db.Grade.Where(item =>
                item.GradeSlug.Equals(grade.GradeSlug))
                    .FirstOrDefaultAsync();
                if (grade_check_slug != null)
                {
                    return ErrorHandler.BadRequestResponse(Message.GradeSlugExist);
                }
            }

            _db.Grade.Add(grade);
            await _db.SaveChangesAsync();

            res.Data = grade;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }
        
        /// <summary>
        /// Edit lớp
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut("edit/{id}")]
        public async Task<ServiceResponse> EditGrade(Guid id, Grade grade)
        {
            ServiceResponse res = new ServiceResponse();
            if (!Helper.CheckPermission(HttpContext, "admin"))
            {
                return ErrorHandler.UnauthorizeCatchResponse();
            }
            var grade_result = await _db.Grade.FindAsync(id);
            if (grade_result == null)
            {
                return ErrorHandler.NotFoundResponse(Message.GradegNotFound);
            }

            if (string.IsNullOrEmpty(grade.GradeName))
            {
                return ErrorHandler.BadRequestResponse(Message.CategoryNameEmpty);
            }

            grade_result.GradeName = grade.GradeName.Trim();
            if (string.IsNullOrEmpty(grade.GradeSlug))
            {
                grade_result.GradeSlug = StringUtils.Slugify(grade_result.GradeName.Trim());
            }
            else
            {
                var category_check_slug = await _db.Grade.Where(item =>
                item.GradeSlug.Equals(grade.GradeSlug.Trim()))
                    .FirstOrDefaultAsync();
                if (category_check_slug != null)
                {
                    return ErrorHandler.BadRequestResponse(Message.GradeSlugExist);
                }
                else
                {
                    grade_result.GradeSlug = grade.GradeSlug.Trim();
                }
            }

            await _db.SaveChangesAsync();

            res.Data = grade_result;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }


        /// <summary>
        /// Xóa lớp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ServiceResponse> DeleteGrade(Guid id)
        {
            ServiceResponse res = new ServiceResponse();
            if (!Helper.CheckPermission(HttpContext, "admin"))
            {
                return ErrorHandler.UnauthorizeCatchResponse();
            }
            var grade = await _db.Grade.FindAsync(id);
            if (grade == null)
            {
                return ErrorHandler.BadRequestResponse(Message.GradegNotFound);
            }

            _db.Grade.Remove(grade);
            await _db.SaveChangesAsync();

            res.Data = true;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }

    }
}
