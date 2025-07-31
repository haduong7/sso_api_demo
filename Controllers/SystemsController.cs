using Common.Utilities.Utils;
using Lib.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using SSO_Api.DTO;
using SSO_Api.Models;
using SSO_Api.Repositories.RepositorySystemEF;

namespace SSO_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemsController : ControllerBase
    {
        private readonly ISystemManagement systemManagement;
        public SystemsController(ISystemManagement systemManagement)
        {
            this.systemManagement = systemManagement;
        }
        /// <summary>
        /// Danh sách nhóm quyền
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [Route("role-group/get")]
        public ActionResult GetListRoleGroup(int RoleGroupId = 0, string RoleGroupName = "", int pageNumber = 1, int pageSize = 40)
        {
            try
            {
                var TotalRecord = 0;
                var lstData = systemManagement.GetListRoleGroup(new TRoleGroup()
                {
                    RoleGroupId = RoleGroupId,
                    RoleGroupName = RoleGroupName,
                }, ref TotalRecord, pageNumber, pageSize);
                var TotalPage = Convert.ToInt32(System.Math.Ceiling(TotalRecord / System.Convert.ToDouble(pageSize)));
                return Ok(new { code = lstData.Count(), data = lstData, totalRecord = TotalRecord, totalPage = TotalPage });
            }
            catch (Exception ex)
            {
                return Ok(new { code = ErrorCode.SYSTEM_EXECEPTION, data = ex.Message });
            }
        }

        //[Authorize]
        [HttpPost]
        [Route("role-group/add")]
        public ActionResult RoleGroupAdd([FromBody] TRoleGroup obj)
        {
            var res = -1;
            try
            {
                res = systemManagement.TRoleGroup_Add(obj);
            }
            catch (Exception ex)
            {
                res = ErrorCode.SYSTEM_EXECEPTION;
                //NLogManager.PublishException(ex);
            }
            return Ok(new { code = res });
        }

        /// <summary>
        /// Lấy danh sách chức năng
        /// </summary>
        /// <param name="FunctionId"></param>
        /// <param name="FunctionName"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [Route("function/get")]
        public ActionResult GetListFunction(int FunctionId = 0, string FunctionName = "", int pageNumber = 1, int pageSize = 40)
        {
            try
            {
                var TotalRecord = 0;
                var lstData = systemManagement.GetListFunction(new TFunction()
                {
                    FunctionId = FunctionId,
                    FunctionName = FunctionName,
                }, ref TotalRecord, pageNumber, pageSize);
                var TotalPage = Convert.ToInt32(System.Math.Ceiling(TotalRecord / System.Convert.ToDouble(pageSize)));
                return Ok(new { code = lstData.Count(), data = lstData, totalRecord = TotalRecord, totalPage = TotalPage });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = ErrorCode.SYSTEM_EXECEPTION, data = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách chức năng phân quyền theo người đăng nhập
        /// </summary>
        /// <param name="FunctionId"></param>
        /// <param name="FunctionName"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [Route("function/get-by-permission")]
        public ActionResult GetListFunctionByUserIdPermission()
        {
            try
            {
                var userId = systemManagement.AccountSession(HttpContext).UserId;
                var userName = systemManagement.AccountSession(HttpContext).UserName;
                var lstData = systemManagement.GetListFunctionByUserIdPermission(userId, userName);
                return Ok(new { code = lstData.Count(), data = lstData });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = ErrorCode.SYSTEM_EXECEPTION, data = ex.Message });
            }
        }

        /// <summary>
        /// Danh sách Function theo Role Group
        /// </summary>
        /// <param name="RoleGroupId"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [Route("function-by-role-group/get")]
        public ActionResult GetListFunctionByRoleGroupId(int RoleGroupId = 0)
        {
            try
            {
                var strListFunctionId = systemManagement.GetListFunctionByRoleGroupId(RoleGroupId);
                return Ok(new { code = 1, data = strListFunctionId });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = ErrorCode.SYSTEM_EXECEPTION, data = ex.Message });
            }
        }

        /// <summary>
        /// Danh sách User theo Role Group
        /// </summary>
        /// <param name="RoleGroupId"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [Route("user-by-role-group/get")]
        public ActionResult GetListUserByRoleGroupId(int RoleGroupId = 0)
        {
            try
            {
                var strListId = systemManagement.GetListUserByRoleGroupId(RoleGroupId);
                return Ok(new { code = 1, data = strListId });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = ErrorCode.SYSTEM_EXECEPTION, data = ex.Message });
            }
        }
        
        //[Authorize]
        [HttpPost]
        [Route("role-group/update")]
        public ActionResult RoleGroupUpdate([FromBody] TRoleGroup obj)
        {
            var res = -1;
            try
            {
                res = systemManagement.TRoleGroup_Update(obj);
            }
            catch (Exception ex)
            {
                res = ErrorCode.SYSTEM_EXECEPTION;
                //NLogManager.PublishException(ex);
            }
            return Ok(new { code = res });
        }
        //[Authorize]
        [HttpPost]
        [Route("role-group/delete")]
        public ActionResult DeleteRoleGroup([FromBody] TRoleGroup obj)
        {
            var res = -1;
            try
            {
                res = systemManagement.TRoleGroup_Delete(obj);
            }
            catch (Exception ex)
            {
                res = ErrorCode.SYSTEM_EXECEPTION;
                //NLogManager.PublishException(ex);
            }
            return Ok(new { code = res });
        }

        //[Authorize]
        [HttpPost]
        [Route("GetUserByRoleGroupPaging")]
        public ActionResult GetUserByRoleGroupPaging([FromBody] InputSearchRoleGroup dyn)
        {
            try
            {
                var userId = systemManagement.AccountSession(HttpContext).UserId;
                var res = systemManagement.GetUserByRoleGroupPaging(dyn);
                return Ok(res);
            }
            catch (Exception ex)
            {
                MessageReturn objReturn = new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi': " + ex.Message);
                return Ok(objReturn);
            }
        }

        //[Authorize]
        [HttpPost]
        [Route("GetUserAll")]
        public ActionResult GetUserAll()
        {
            try
            {
                var res = systemManagement.GetUserAll();
                return Ok(res);
            }
            catch (Exception ex)
            {
                MessageReturn objReturn = new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi': " + ex.Message);
                return Ok(objReturn);
            }
        }

        //[Authorize]
        [HttpPost]
        [Route("AddUserToGroup")]
        public ActionResult AddUserToGroup([FromBody] InputAddRefRoleGroupUser inputdata)
        {
            try
            {
                // public dynamic AddUserToGroup(InputAddRefRoleGroupUser inputdata);
                var res = systemManagement.AddUserToGroup(inputdata);
                return Ok(res);
            }
            catch (Exception ex)
            {
                MessageReturn objReturn = new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi': " + ex.Message);
                return Ok(objReturn);
            }

        }

        //[Authorize]
        [HttpPost]
        [Route("DeleteUserToGroup")]
        public ActionResult DeleteUserToGroup([FromBody] InputAddRefRoleGroupUser inputdata)
        {
            try
            {
                // public dynamic AddUserToGroup(InputAddRefRoleGroupUser inputdata);
                var res = systemManagement.DeleteUserToGroup(inputdata);
                return Ok(res);
            }
            catch (Exception ex)
            {
                MessageReturn objReturn = new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi': " + ex.Message);
                return Ok(objReturn);
            }

        }

        //[Authorize]
        [HttpPost]
        [Route("GetRoleGroupFunctionAll")]
        public ActionResult GetRoleGroupFunctionAll([FromBody] TRoleGroup inputdata)
        {
            try
            {
                // public dynamic AddUserToGroup(InputAddRefRoleGroupUser inputdata);
                var res = systemManagement.GetRoleGroupFunctionAll(inputdata);
                return Ok(res);
            }
            catch (Exception ex)
            {
                MessageReturn objReturn = new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi': " + ex.Message);
                return Ok(objReturn);
            }
        }

        //[Authorize]
        [HttpPost]
        [Route("AddFunctionToGroup")]
        public ActionResult AddFunctionToGroup([FromBody] List<TRefRoleGroupFunctionAll> listItem)
        {
            try
            {
                // public dynamic AddUserToGroup(InputAddRefRoleGroupUser inputdata);
                var res = systemManagement.AddFunctionToGroup(listItem);
                return Ok(res);
            }
            catch (Exception ex)
            {
                MessageReturn objReturn = new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi': " + ex.Message);
                return Ok(objReturn);
            }

        }

        /// <summary>
        /// phân quyền chức năng thêm mơ, sửa, xóa
        /// </summary>
        /// <param name="FunctionId"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [Route("function/get-by-permission-action")]
        public ActionResult GetListFunctionByUserIdPermissionAction(string FunctionName = "")
        {
            try
            {
                var userId = systemManagement.AccountSession(HttpContext).UserId;
                var TRefRoleGroupFunction = systemManagement.GetFunctionViewNewEditDeleteByUserIdPermission(userId, FunctionName);
                return Ok(new { code = 100, data = TRefRoleGroupFunction });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = ErrorCode.SYSTEM_EXECEPTION, data = ex.Message });
            }
        }
    }
}
