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
    [Route("api/lesson")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly CoursesEduContext _db;
        string sql_get_lesson = "select * from Lesson where CHARINDEX(@txtSeach, LessonName) > 0 or CHARINDEX(@txtSeach, LessonSlug) > 0 or CHARINDEX(@txtSeach, LessonDuration) > 0";

        public LessonController(CoursesEduContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Lấy danh sách bài học có phân trang và cho phép tìm kiếm
        /// </summary>
        /// <returns></returns>
        /// https://localhost:44335/api/lesson?page=2&record=10&search=admin
        [HttpGet]
        public async Task<ServiceResponse> GetAccountsByPagingAndSearch([FromQuery] string search, [FromQuery] int? page = 1, [FromQuery] int? record = 10)
        {
            ServiceResponse res = new ServiceResponse();
            try
            {
                List<Lesson> records = new List<Lesson>();

                if (search != null && search.Trim() != "")
                {
                    var param = new SqlParameter("@txtSeach", search);
                    records = _db.Lesson.FromSqlRaw(sql_get_lesson, param).OrderByDescending(x => x.LessonName).ToList();
                }
                else
                {
                    records = await _db.Lesson.OrderByDescending(x => x.LessonName).ToListAsync();
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
        /// Lấy chi tiết thông tin bài học
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<ServiceResponse> GetCategoryDetail(Guid? id)
        {
            ServiceResponse res = new ServiceResponse();
            var lesson = await _db.Lesson.FindAsync(id);
            if (lesson == null)
            {
                res.Data = null;
                res.Message = Message.CategoryNotFound;
                res.ErrorCode = 404;
                res.StatusCode = HttpStatusCode.NotFound;
                return res;
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("lesson", lesson);

            res.Data = result;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }
    }
}
