using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using courses_edu_be.Model.CustomModel;
using courses_edu_be.Models;
using courses_edu_be.Utils;
using courses_edu_be.Constants;

namespace JWT.courses_edu_be
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string key;
        public JwtAuthenticationManager(string key)
        {
            this.key = key;
        }

        public async Task<ServiceResponse> LoginAuthenticate(CoursesEduContext _db, string username, string password)
        {
            ServiceResponse res = new ServiceResponse();
            try
            {
                string passwordMD5 = StringUtils.EncodeMD5(password);
                var accountResult = await _db.SystemUser
                    .Where(_ => _.UserName == username && _.UserPassword == passwordMD5)
                    .FirstOrDefaultAsync();
                if (accountResult == null)
                {
                    return ErrorHandler.BadRequestResponse(Message.LoginIncorrect);
                }
                accountResult.UserPassword = null;
                string sql_get_role = $"select * from SystemRole where RoleId in (select distinct SystemRoleId from UserDetail where SystemUserId = @SystemUserId)";
                var roles = await _db.SystemRole
                    .FromSqlRaw(sql_get_role, new SqlParameter("@SystemUserId", accountResult.SystemUserId))
                    .ToListAsync();

                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("account", accountResult);
                result.Add("roles", roles);
                result.Add("token", EncodeJWTToken(result));
                res.Success = true;
                res.Data = result;
                res.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                res = ErrorHandler.ErrorCatchResponse(e);
            }
            return res;
        }

        private string EncodeJWTToken(object result)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDesciptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,JsonConvert.SerializeObject(result))
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDesciptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
