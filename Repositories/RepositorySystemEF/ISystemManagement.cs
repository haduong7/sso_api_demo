using Common.Utilities.Utils;
using Lib.DataAccess.Models;
using SSO_Api.DTO;
using SSO_Api.Models;

namespace SSO_Api.Repositories.RepositorySystemEF
{
    public interface ISystemManagement
    {
        TUser Login(ObjUser obj);

        List<TUser> GetListSystemUser();

        List<TUserType> GetListUserType();

        ObjUserMemberShipGroup GetAccountInforV2(int userId);

        List<TUserAddOrUpdate> TUser_GetAll(ref int TotalRecord, TUser objInput, int DeptId = 0, int pageNumber = 1, int pageSize = 40);

        int TUser_Add(TUserAddOrUpdate objInput);

        int TUser_Delete(TUser objInput);

        int TUser_Update(TUserAddOrUpdate objInput);

        ObjUser AccountSession(HttpContext httpContext);

        int Authenticate(ObjUser model, ref ObjUserResponse obj);

        List<TRoleGroup> GetListRoleGroup(TRoleGroup objInput, ref int TotalRecord, int pageNumber = 1, int pageSize = 40);

        string GetListFunctionByRoleGroupId(int RoleGroupId);
        string GetListUserByRoleGroupId(int RoleGroupId);

        int TRoleGroup_Add(TRoleGroup obj);
        int TRoleGroup_Update(TRoleGroup objInput);
        int TRoleGroup_Delete(TRoleGroup objInput);

        List<TFunctionRole> GetListFunctionByUserIdPermission(int UserId, string userName);

        public TRefRoleGroupFunction GetFunctionViewNewEditDeleteByUserIdPermission(int UserId, string FuntionName);

        List<TFunction> GetListFunction(TFunction objInput, ref int TotalRecord, int pageNumber = 1, int pageSize = 40);

        /// <summary>
        /// Danh sách người dùng theo nhóm người dùng
        /// </summary>
        /// <param name="objinput"></param>
        /// <returns></returns>

        public dynamic GetUserByRoleGroupPaging(InputSearchRoleGroup objinput);

        /// <summary>
        /// Lấy danh sách người sử dụng
        /// </summary>
        /// <returns></returns>
        public dynamic GetUserAll();

        /// <summary>
        /// Thêm User vào nhóm người dùng
        /// </summary>
        /// <param name="data"></param>       
        /// <returns></returns>
        public dynamic AddUserToGroup(InputAddRefRoleGroupUser inputdata);

        /// <summary>
        /// Xóa User từ nhóm người dùng
        /// </summary>
        /// <param name="inputdata"></param>
        /// <returns></returns>
        public dynamic DeleteUserToGroup(InputAddRefRoleGroupUser inputdata);

        /// <summary>
        /// Danh sách chức năng(menu) theo nhóm người dùng
        /// </summary>
        /// <param name="inputRoleGroup"></param>
        /// <returns></returns>
        public dynamic GetRoleGroupFunctionAll(TRoleGroup inputRoleGroup);

        /// <summary>
        /// Thêm chức năng (menu) vào nhóm người dùng
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public dynamic AddFunctionToGroup(List<TRefRoleGroupFunctionAll> listItem);

        //List<TFunctionAction> GetAllFunctionAction();

        Task<dynamic> getSapUserList(string? filter, int? fields, int? limit);

        
        Task<ResultCompareUserSys> getSapUserList();
    }

}
