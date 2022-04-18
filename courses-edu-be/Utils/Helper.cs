using courses_edu_be.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace courses_edu_be.Utils
{
    public static class Helper
    {
        private static readonly CoursesEduContext _db = new CoursesEduContext(); 
        private static readonly string sql_get_role = "SELECT * FROM SystemRole WHERE RoleId IN (SELECT DISTINCT SystemRoleId FROM UserDetail WHERE SystemUserId = @SystemUserId)";

        public static bool CheckPermission(HttpContext httpContext, string role_code)
        {
            Dictionary<string, object> account_login = JsonConvert.DeserializeObject<Dictionary<string, object>>(httpContext.User.Identity.Name);
            if (account_login != null && account_login.ContainsKey("account"))
            {
                JObject jAccount = account_login["account"] as JObject;
                SystemUser account = jAccount.ToObject<SystemUser>();

                var roles = _db.SystemRole
                    .FromSqlRaw(sql_get_role, new SqlParameter("@SystemUserId", account.SystemUserId))
                    .ToList();

                for (int i = 0; i < roles.Count(); i++)
                {
                    if (roles[i].RoleName == role_code)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static Guid? getUserId(HttpContext httpContext)
        {
            Dictionary<string, object> account_login = JsonConvert.DeserializeObject<Dictionary<string, object>>(httpContext.User.Identity.Name);
            if (account_login != null && account_login.ContainsKey("account"))
            {
                JObject jAccount = account_login["account"] as JObject;
                SystemUser account = jAccount.ToObject<SystemUser>();

                return account.SystemUserId;
            }
            return null;
        }
        
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> input, string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
                return input;

            int i = 0;
            foreach (string propname in queryString.Split(','))
            {
                var subContent = propname.Split(' ');
                if (subContent[1].Trim().ToLower() == "asc")
                {
                    if (i == 0)
                        input = input.OrderBy(x => GetPropertyValue(x, subContent[0].Trim()));
                    else
                        input = ((IOrderedEnumerable<T>)input).ThenBy(x => GetPropertyValue(x, subContent[0].Trim()));
                }
                else
                {
                    if (i == 0)
                        input = input.OrderByDescending(x => GetPropertyValue(x, subContent[0].Trim()));
                    else
                        input = ((IOrderedEnumerable<T>)input).ThenByDescending(x => GetPropertyValue(x, subContent[0].Trim()));
                }
                i++;
            }

            return input;
        }

        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }
    }
}
