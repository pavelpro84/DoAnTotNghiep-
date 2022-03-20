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
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CoursesEduContext _db;
        string sql_get_category = "select * from Category where CHARINDEX(@txtSeach, CategoryName) > 0 or CHARINDEX(@txtSeach, CategoryId) > 0";

        public CategoryController(CoursesEduContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Lấy danh sách category có phân trang và cho phép tìm kiếm
        /// </summary>
        /// <returns></returns>
        /// https://localhost:44335/api/category?page=2&record=10&search=admin
        [HttpGet]
        public async Task<ServiceResponse> GetAccountsByPagingAndSearch([FromQuery] string search, [FromQuery] int? page = 1, [FromQuery] int? record = 10)
        {
            ServiceResponse res = new ServiceResponse();
            try
            {
                List<Category> records = new List<Category>();

                if (search != null && search.Trim() != "")
                {
                    var param = new SqlParameter("@txtSeach", search);
                    records = _db.Category.FromSqlRaw(sql_get_category, param).OrderByDescending(x => x.CategoryName).ToList();
                }
                else
                {
                    records = await _db.Category.OrderByDescending(x => x.CategoryName).ToListAsync();
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
        /// Lấy chi tiết thông tin category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<ServiceResponse> GetCategoryDetail(Guid? id)
        {
            ServiceResponse res = new ServiceResponse();
            var category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                res.Data = null;
                res.Message = Message.CategoryNotFound;
                res.ErrorCode = 404;
                res.StatusCode = HttpStatusCode.NotFound;
                return res;
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("category", category);

            res.Data = result;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }

        /// <summary>
        /// Lấy chi tiết thông tin category by slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        [HttpGet("detail-slug/{id}")]
        public async Task<ServiceResponse> GetCategoryDetail(string slug)
        {
            ServiceResponse res = new ServiceResponse();
            var category = await _db.Category.Where(item => slug.Equals(slug)).FirstOrDefaultAsync();
            if (category == null)
            {
                res.Data = null;
                res.Message = Message.CategoryNotFound;
                res.ErrorCode = 404;
                res.StatusCode = HttpStatusCode.NotFound;
                return res;
            }

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("category", category);

            res.Data = result;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }
    }
}
