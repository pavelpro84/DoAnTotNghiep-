using courses_edu_be.Constants;
using courses_edu_be.Model.CustomModel;
using courses_edu_be.Models;
using courses_edu_be.Models.CustomModel.User;
using courses_edu_be.Utils;
using JWT.courses_edu_be;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly CoursesEduContext _db;

        public UserController(CoursesEduContext context, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _db = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        /// <summary>
        /// Lấy danh sách tài khoản có phân trang và cho phép tìm kiếm
        /// </summary>
        /// <returns></returns>
        /// https://localhost:44335/api/user?page=2&record=10&search=admin
        [HttpGet]
        public async Task<ServiceResponse> GetAccountsByPagingAndSearch([FromQuery] string search, [FromQuery] int? page = 1, [FromQuery] int? record = 10)
        {
            ServiceResponse res = new ServiceResponse();
            try
            {
                List<SystemUser> records = new List<SystemUser>();

                if (search != null && search.Trim() != "")
                {
                    //CHARINDEX tìm không phân biệt hoa thường trả về vị trí đầu tiên xuất hiện của chuỗi con
                    string sql_get_account = "select * from SystemUser where CHARINDEX(@txtSeach, Username) > 0 or CHARINDEX(@txtSeach, UserUsername) > 0";
                    var param = new SqlParameter("@txtSeach", search);
                    records = _db.SystemUser.FromSqlRaw(sql_get_account, param).OrderByDescending(x => x.UserName).ToList();
                }
                else
                {
                    records = await _db.SystemUser.OrderByDescending(x => x.UserName).ToListAsync();
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
        /// Lấy chi tiết thông tin tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<ServiceResponse> GetAccount(Guid? id)
        {
            ServiceResponse res = new ServiceResponse();
            var account = await _db.SystemUser.FindAsync(id);
            if (account == null)
            {
                res.Data = null;
                res.Message = Message.AccountNotFound;
                res.ErrorCode = 404;
                res.StatusCode = HttpStatusCode.NotFound;
                return res;
            }
            account.UserPassword = null;
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("account", account);

            res.Data = result;
            res.Success = true;
            res.StatusCode = HttpStatusCode.OK;
            return res;
        }

        /// <summary>
        /// Lấy chi tiết thông tin tài khoản đang dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail")]
        public async Task<ServiceResponse> GetAccount()
        {
            ServiceResponse res = new ServiceResponse();
            try
            {
                var userId = Helper.getUserId(HttpContext);
                var account = await _db.SystemUser.FindAsync(userId);
                if (account == null)
                {
                    return ErrorHandler.NotFoundResponse(Message.AccountNotFound);
                }
                account.UserPassword = null;
                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("account", account);

                res.Data = result;
                res.Success = true;
                res.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                res = ErrorHandler.ErrorCatchResponse(e);
            }

            return res;
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param username="" password=""></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ServiceResponse> UserLogin(UserLogin userLogin)
        {
            return await _jwtAuthenticationManager.LoginAuthenticate(_db, userLogin.username, userLogin.password);
        }

        /// <summary>
        /// Tạo mới admin
        /// </summary>
        /// <param SystemUser="SystemUser"></param>
        /// <returns></returns>
        [HttpPost("create-user")]
        public async Task<ServiceResponse> CreateUser(UserSignUp userSignUp)
        {
            try
            {
                if (string.IsNullOrEmpty(userSignUp.UserFullName))
                {
                    return ErrorHandler.BadRequestResponse(Message.UserFullNameEmpty);
                }
                if (string.IsNullOrEmpty(userSignUp.UserPassword))
                {
                    return ErrorHandler.BadRequestResponse(Message.UserPasswordEmpty);
                }
                var user_result = _db.SystemUser
                    .Where(item => item.UserName.Equals(userSignUp.UserName.Trim()))
                    .FirstOrDefault();
                if (user_result != null)
                {
                    return ErrorHandler.BadRequestResponse(Message.UserLoginNameExist);
                }

                Dictionary<string, object> result = new Dictionary<string, object>();

                SystemUser sysUser = new SystemUser()
                {
                    SystemUserId = Guid.NewGuid(),
                    UserName = userSignUp.UserName.Trim(),
                    UserFullName = userSignUp.UserFullName?.Trim(),
                    UserPassword = StringUtils.EncodeMD5(userSignUp.UserPassword.Trim()),
                    UserAvatar = userSignUp.UserAvatar?.Trim(),
                    UserEmail = userSignUp.UserEmail?.Trim(),
                    UserDob = userSignUp.UserDob?.Trim(),
                    UserPhone = userSignUp.UserPhone?.Trim(),
                };
                //_db.SystemUser.Add(sysUser);
                result.Add("user", sysUser);

                var role = await _db.SystemRole.FindAsync(userSignUp.UserRoleType);
                if (role == null)
                {
                    return ErrorHandler.BadRequestResponse(Message.RoleNotFound);
                }
                result.Add("role", role);

                UserDetail sysUserDetail = new UserDetail
                {
                    UserDetailId = Guid.NewGuid(),
                    SystemRoleId = userSignUp.UserRoleType,
                    SystemUserId = sysUser.SystemUserId,
                };
                //_db.UserDetail.Add(sysUserDetail);

                //await _db.SaveChangesAsync();

                return new ServiceResponse()
                {
                    Data = sysUser,
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };
            }
            catch (Exception e)
            {
                return ErrorHandler.ErrorCatchResponse(e);
            }
        }
    }
}
