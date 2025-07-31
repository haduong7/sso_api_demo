using Common.Utilities.Security;
using Common.Utilities.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSO_Api.Data;
using SSO_Api.DTO;
using SSO_Api.Models;
using SSO_Api.Repositories.MyAdgEF;
using SSO_Api.Repositories.RepositorySystemEF;

namespace SSO_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ISystemManagement systemManagement;
        public UsersController(ISystemManagement systemManagement)
        {
            this.systemManagement = systemManagement;
        }
        public IActionResult Index()
        {
            //return View();
            try
            {
                return Ok(new { code = 1, data = "API Connected" });
            }
            catch (System.Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = -99, data = ex.Message });
            }
        }

        // GET: api/users
        [HttpGet]
        [Route("all")] // => cụ thể hóa endpoint: GET api/users/all
        public IActionResult GetAllUsers()
        {
            try
            {
                // Danh sách user mẫu
                var users = new List<object>
                {
                    new { Id = 1, Name = "Nguyen Van A", Email = "a@example.com" },
                    new { Id = 2, Name = "Tran Thi B", Email = "b@example.com" },
                    new { Id = 3, Name = "Le Van C", Email = "c@example.com" }
                };

                return Ok(new { code = 1, data = users });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { code = -99, message = ex.Message });
            }
        }
        /// <summary>
        /// Thêm mới user
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost]
        [Route("add")]
        public ActionResult UserAdd([FromBody] TUserAddOrUpdate obj)
        {
            var res = -1;
            try
            {
                obj.IsDeleted = 0;
                
                if (obj.PasswordInsertOrUpDate != null && obj.PasswordInsertOrUpDate.Length > 0)
                {
                    obj.Password = PasswordHash.MD5Encrypt(obj.PasswordInsertOrUpDate);
                }
                res = systemManagement.TUser_Add(obj);

                var UserName = systemManagement.AccountSession(HttpContext).UserName;
                return Ok(new { code = res });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = -99, mess = ex.Message });
            }

        }

        /// <summary>
        /// Kiểm tra đăng nhập hệ thống
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        public IActionResult Authenticate(ObjUser obj)
        {
            try
            {
                var objResponse = new ObjUserResponse();
                var response = systemManagement.Authenticate(obj, ref objResponse);

                if (response < 0)
                    return Ok(new { code = ErrorCode.USER_NOT_EXISTS, data = "response" });
                return Ok(new { code = response, data = objResponse });
            }
            catch (System.Exception ex)
            {
                return Ok(new { code = -99, data = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin account info từ database
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("acc-info/v2")]
        public IActionResult GetAccountLoginV2()
        {
            try
            {
                var userId = systemManagement.AccountSession(HttpContext).UserId;
                var response = systemManagement.GetAccountInforV2(userId);

                if (response.UserId < 0)
                    return Ok(new { code = ErrorCode.USER_NOT_EXISTS, message = "Tài khoản không tồn tại" });
                return Ok(new { code = response.UserId, data = response });
            }
            catch (System.Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = -99, data = ex.Message });
            }
        }

        /// <summary>
        /// Tìm kiếm user
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="gender"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("get")]
        public ActionResult UserSearch(string fullName = "", string userName = "", int userId = 0, int type = -1, string mobile = "", string email = "", int pageNumber = 1, int pageSize = 40, int gender = -1, int deptId = 0)
        {
            try
            {
                int TotalRecord = 0;
                var lstData = systemManagement.TUser_GetAll(ref TotalRecord, new TUser()
                {
                    UserId = userId,
                    FullName = fullName,
                    UserName = userName,
                    UserType = type,
                    Gender = gender,
                    Mobile = mobile,
                    Email = email
                }, deptId, pageNumber, pageSize);
                var TotalPage = Convert.ToInt32(System.Math.Ceiling(TotalRecord / System.Convert.ToDouble(pageSize)));
                return Ok(new { code = lstData.Count, data = lstData, totalRecord = TotalRecord, totalPage = TotalPage });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = -99, data = ex.Message });
            }

        }

        /// <summary>
        /// Lấy thông tin loại user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("user-type/get")]
        public ActionResult GetListUserType()
        {
            try
            {
                var lstData = systemManagement.GetListUserType();
                return Ok(new { code = lstData.Count, data = lstData });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = -99, data = ex.Message });
            }

        }

        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("delete")]
        public ActionResult UserDelete([FromBody] TUser obj)
        {
            var res = -1;
            if (!ModelState.IsValid)
            {
                try
                {
                    var UserName = systemManagement.AccountSession(HttpContext).UserName;
                    obj.UserName = UserName;
                    res = systemManagement.TUser_Delete(obj);
                }
                catch (Exception ex)
                {
                    res = -99;
                    //NLogManager.PublishException(ex);
                }
            }
            
            
            return Ok(new { code = res });
        }

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("update")]
        public ActionResult UserUpdate([FromBody] TUserAddOrUpdate obj)
        {
            var res = -1;
            try
            {
                if (obj.PasswordInsertOrUpDate != null && obj.PasswordInsertOrUpDate.Length > 0)
                {
                    obj.Password = PasswordHash.MD5Encrypt(obj.PasswordInsertOrUpDate);
                }
                else
                {
                    obj.Password = "";
                }
                res = systemManagement.TUser_Update(obj);
                var UserName = systemManagement.AccountSession(HttpContext).UserName;
                //_repository.LogAction(new TLogAction()
                //{
                //    User = UserName,
                //    Note = $"Update User by {UserName} ==> {JsonConvert.SerializeObject(obj)}",
                //    ActionName = LogActionType.UpdateUser
                //});
                return Ok(new { code = res });
            }
            catch (Exception ex)
            {
                res = -99;
                //NLogManager.PublishException(ex);
                return Ok(new { code = -99, mess = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetDanhSachNguoiDungSap")]
        public async Task<ActionResult> GetDanhSachNguoiDungSap(string? filter, int? fields, int? limit)
        {
            try
            {
                //InputApiDongBoVatTu input = new InputApiDongBoVatTu();
                //input.MAVATTU = inputApi.MaVatTu;
                //input.TUNGAY = inputApi.fromDate.ToString("yyyyMMdd");
                //input.DENNGAY = inputApi.toDate.ToString("yyyyMMdd");
                var res = await systemManagement.getSapUserList(filter, fields, limit);
                return Ok(res);
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                MessageReturn objReturn = new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi': " + ex.Message);
                return Ok(objReturn);
            }
        }
        /// <summary>
        /// Tìm kiếm user trong bảng so sánh
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="gender"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserSysComparePaging")]
        public async Task<IActionResult> GetUserSysComparePaging()
        {
            try
            {
                var TotalRecord = 0;
                //var userId = _IUserService.AccountSession(HttpContext).UserId;
                
                //var lstData = systemManagement.getSapUserList();

                //var TotalPage = Convert.ToInt32(Math.Ceiling(TotalRecord / Convert.ToDouble(pageSize)));
                ////kết quả API
                //ValueData objValueData = new ValueData();
                //objValueData.paging = new Pagingitem()
                //{
                //    TotalPage = TotalPage,
                //    TotalItem = TotalRecord,
                //    CurrentPage = pageNumber
                //};
                //objValueData.items = lstData;

                //MessageReturn objReturn = new MessageReturn(lstData.Count(), "Danh sách đề xuất", objValueData);
                var result = await systemManagement.getSapUserList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                MessageReturn objReturn = new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Danh sách tài khoản", new ValueData());
                return Ok(objReturn);
            }
        }


    }
}
