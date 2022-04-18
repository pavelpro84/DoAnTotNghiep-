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
                return ErrorHandler.NotFoundResponse(Message.CategoryNotFound);
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
                return ErrorHandler.NotFoundResponse(Message.CategoryNotFound);
            }

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("category", category);

            res.Data = result;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ServiceResponse> CreateCategory(Category category)
        {
            ServiceResponse res = new ServiceResponse();
            if (!Helper.CheckPermission(HttpContext, "admin"))
            {
                return ErrorHandler.UnauthorizeCatchResponse();
            }

            if (string.IsNullOrEmpty(category.CategoryName))
            {
                return ErrorHandler.BadRequestResponse(Message.CategoryNameEmpty);
            }

            category.CategoryId = Guid.NewGuid();
            category.CategoryName = category.CategoryName.Trim();
            if (string.IsNullOrEmpty(category.CategorySlug))
            {
                category.CategorySlug = StringUtils.Slugify(category.CategoryName
                    + "-"
                    + category.CategoryId);
            }
            else
            {
                var category_check_slug = await _db.Category.Where(item =>
                item.CategorySlug.Equals(category.CategorySlug.Trim()))
                    .FirstOrDefaultAsync();
                if (category_check_slug != null)
                {
                    return ErrorHandler.BadRequestResponse(Message.CategorySlugExist);
                }
            }

            _db.Category.Add(category);
            await _db.SaveChangesAsync();

            res.Data = category;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }

        /// <summary>
        /// Edit category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut("edit/{id}")]
        public async Task<ServiceResponse> EditCategory(Guid id, Category category)
        {
            ServiceResponse res = new ServiceResponse();
            if (!Helper.CheckPermission(HttpContext, "admin"))
            {
                return ErrorHandler.UnauthorizeCatchResponse();
            }
            var category_result = await _db.Category.FindAsync(id);
            if (category_result == null)
            {
                return ErrorHandler.BadRequestResponse(Message.CategoryNotFound);
            }

            if (string.IsNullOrEmpty(category.CategoryName))
            {
                return ErrorHandler.BadRequestResponse(Message.CategoryNameEmpty);
            }

            category_result.CategoryName = category.CategoryName.Trim();
            if (string.IsNullOrEmpty(category.CategorySlug))
            {
                category_result.CategorySlug = StringUtils.Slugify(category.CategoryName.Trim()
                    + "-"
                    + category_result.CategoryId);
            }
            else
            {
                var category_check_slug = await _db.Category.Where(item =>
                item.CategorySlug.Equals(category.CategorySlug.Trim()))
                    .FirstOrDefaultAsync();
                if (category_check_slug != null)
                {
                    return ErrorHandler.BadRequestResponse(Message.CategorySlugExist);
                } else
                {
                    category_result.CategorySlug = category.CategorySlug.Trim();
                }
            }

            await _db.SaveChangesAsync();

            res.Data = category_result;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ServiceResponse> DeleteCategory(Guid id)
        {
            ServiceResponse res = new ServiceResponse();
            if (!Helper.CheckPermission(HttpContext, "admin"))
            {
                return ErrorHandler.UnauthorizeCatchResponse();
            }
            var category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                return ErrorHandler.NotFoundResponse(Message.CategoryNotFound);
            }

            _db.Category.Remove(category);
            await _db.SaveChangesAsync();

            res.Data = true;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }

    }
}
