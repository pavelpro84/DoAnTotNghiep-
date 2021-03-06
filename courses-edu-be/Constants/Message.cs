using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace courses_edu_be.Constants
{
    public class Message
    {
        //General
        public const string SuccessMsg = "Thành công";
        public const string ErrorMsg = "Có lỗi xảy ra";
        public const string TitleError = "Tiêu đề không thể trống";
        public const string NotFound = "Không tìm thấy";
        public const string BadRequest = "Không hợp lệ";

        //Xử lý message tài khoản
        public const string NotAuthorize = "Bạn không có quyền này";
        public const string LoginIncorrect = "Thông tin đăng nhập không chính xác!";
        public const string AccountNotFound = "Không tìm thấy tài khoản này";
        public const string AccountUsernameExist = "Username đã tồn tại";

        public const string AccountLoginAgain = "Bạn vui lòng đăng nhập lại để thực hiện chức năng này";
        public const string AccountLogoutSuccess = "Đăng xuất thành công";
        public const string AccountLogLogin = "Login";
        public const string AccountLogDelete = "Xóa tài khoản";
        public const string AccountLogLogout = "Logout";
        public const string AccountLogChange = "Thay đổi thông tin tài khoản";
        public const string AccountLogPassword = "Thay đổi mật khẩu";

        //Xử lý message user account
        public const string UserFullNameEmpty = "Tên người dùng không thể trống";
        public const string UserLoginNameEmpty = "Tên đăng nhập không thể trống";
        public const string UserPasswordEmpty = "Mật khẩu không thể trống";
        public const string UserLoginNameExist = "Tên đăng nhập đã tồn tại";
        public const string RoleNotFound = "Không tìm thấy role này";

        //Xử lý message category
        public const string CategoryNameEmpty = "Tên category không thể trống";
        public const string CategoryExist = "Category đã tồn tại";
        public const string CategorySlugExist = "Slug category đã tồn tại";
        public const string CategorygNotFound = "Không tìm thấy category";

        //Xử lý message category
        public const string GradeNameEmpty = "Tên lớp không thể trống";
        public const string GradeExist = "Lớp đã tồn tại";
        public const string GradeSlugExist = "Slug lớp đã tồn tại";
        public const string GradegNotFound = "Không tìm thấy lớp";

        //Xử lý message file
        public const string CategoryNotFound = "Không tìm thấy phân loại này";
    }
}
