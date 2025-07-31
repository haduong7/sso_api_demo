using Lib.DataAccess.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SSO_Api.Models;

namespace SSO_Api.DTO
{
    public class ObjDTO
    {
    }
    public class ObjUser
    {
        public ObjUser()
        {

        }
        public ObjUser(HttpContext context)
        {
            if (context == null)
            {
                return;
            }
            var currentUser = (ObjUser)context.Items["User"];

            if (currentUser == null)
                return;

            this.UserId = currentUser.UserId;
            this.UserName = currentUser.UserName;
            //this.PASSWORD = currentUser.PASSWORD;
            this.FullName = currentUser.FullName;
            this.UserType = currentUser.UserType;
        }
        public int UserId { get; set; }
        //public int ChildId { get; set; }
        public string UserName { get; set; }
        public int? Gender { get; set; }
        public string? Otp { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string Password { get; set; }
        public string? NewPassword { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Token { get; set; }
        public int? UserType { get; set; }
        public string? FromSystem { get; set; }
    }

    public class ObjUserResponse : ObjUser
    {
        public ObjUserResponse()
        {
        }
        public ObjUserResponse(ObjUser user, string token)
        {
            UserId = user.UserId;
            UserName = user.UserName;
            FullName = user.FullName;
            UserType = user.UserType;
            Token = token;
        }
    }
    public class ResultCompareUserSys
    {
        public int soTaiKhoanSAP { get; set; }
        public int soTaiKhoanMyADG { get; set; }
        public List<UserSysCompare> listUserSysCompare { get; set; } = new List<UserSysCompare>();
        
    }
    public class UserSysCompare()
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }

        public string? FullName { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public DateTime? Dob { get; set; }
        public int statusSap { get; set; }
        public bool onSap { get; set; } 
        public bool onMyADG { get; set; }
        public bool infoCorrect { get; set; }
    }

    public class ObjPoDataShow
    {
        public int poDataId { get; set; }
        public string? DocType { get; set; }
        public string Number { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public int AuthorId { get; set; }
        public string Author { get; set; }
        public int? ResponsibilityId { get; set; }
        public string? Responsibility { get; set; }
        public string Company { get; set; }
        public string? Bu { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string? Comment { get; set; }
        public string HtmlPresentation { get; set; }
        public List<TFile> Files { get; set; } = new List<TFile>();

        //public string? previousDraftID { get; set; }
    }
    public class ObjUserMemberShipGroup
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int? UserType { get; set; }
        public int? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string IdOrPassport { get; set; }
        public string Address { get; set; }
        public string JobDescription { get; set; }
        public int WalletId { get; set; }
        public string WalletName { get; set; }
        public int? ClassId { get; set; }
        public int? Grade { get; set; }
        public int? GradeLevel { get; set; }
        public int? BuildingId { get; set; }

        public int? SchoolId { get; set; }
        public string ClassName { get; set; }
        public string Avatar { get; set; }
        public int? Coin { get; set; }
        public int? Point { get; set; }
        public int? TotalCoin { get; set; }
        public int? TotalPoint { get; set; }
        public int? Balance { get; set; }
        public int? Status { get; set; }
        public int IsHomeroom { get; set; }
        public int IsDeputyHomeroom { get; set; }
        public int IsNormal { get; set; }
        public int? IsLeader { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<ObjUserMemberShipGroup> ListChildrent { get; set; }
        public string BackgroundColor { get; set; }
        public int IsTopupPoint { get; set; }
        public int IsDeductPoint { get; set; }
        public int IsTopupCoin { get; set; }
    }

    /// <summary>
    /// properties(thuộc tính phân trang)
    /// </summary>
    public class ValueData
    {
        public Pagingitem paging { get; set; }
        public dynamic items { get; set; }
    }

    public class Pagingitem
    {
        public int TotalPage { get; set; }
        public int TotalItem { get; set; }
        public int CurrentPage { get; set; }
    }
    /// <summary>
    /// Thuộc tính(Properties) kết quả API
    /// </summary>
    public class MessageReturn
    {
        public int Error { get; set; }
        public string sError { get; set; }
        public string Detail { get; set; }
        public ValueData Value { get; set; }

        public MessageReturn(int Error, string Detail)
        {
            this.Error = Error;
            this.Detail = Detail;
        }

        public MessageReturn(int Error, string Detail, dynamic Value)
        {
            this.Error = Error;
            this.Detail = Detail;
            this.Value = Value;
        }


    }

    /// <summary>
    /// Thuộc tính(Properties) kết quả API của SAP
    /// </summary>
    /// 

    public class SAPMessageReturnJson()
    {
        public SAPMessageReturnUser ES_RETURN { get; set; }
        public UsersSap ES_USER { get; set; }
    }
    public class SAPMessageReturnUser()
    {
        public string TYPE { get; set; }
        public string ID { get; set; }
        public string NUMBER { get; set; }
        public string MESSAGE { get; set; }
        public string LOG_NO { get; set; }
        public string LOG_MSG_NO { get; set; }
        public string MESSAGE_V1 { get; set; }
        public string MESSAGE_V2 { get; set; }
        public string MESSAGE_V3 { get; set; }
        public string MESSAGE_V4 { get; set; }
        public string PARAMETER { get; set; }
        public int ROW { get; set; }
        public string FIELD { get; set; }
        public string SYSTEM { get; set; }
    }
    public class UsersSap()
    {
        public int COUNT { get; set; }
        public List<UserSap> USERS { get; set; }
    }
    public class UserSap()
    {
        public int STATUS { get; set; }
        public string USERNAME { get; set; }
        public string FULLNAME { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public DateTime? DATEOFBIRTH { get; set; }
    }
    public class InputAPIGetRoadMap()
    {
        public string poNumber { get; set; }
        public DateTime poDate { get; set; }
    }

    /// <summary>
    /// Thuộc tính(Properties) INPUT for SAP into MyADG
    /// </summary>
    /// 

    public class OutPutAPICreateUpdatePO_PoProcess
    {
        public PoData poData { get; set; }
        public List<POFlowStage> poFlow { get; set; }

    }
    public class PoData
    {
        public string? docType { get; set; }
        public string number { get; set; }
        public DateTime? date { get; set; }
        public string status { get; set; }
        public string author { get; set; }
        public string? responsibility { get; set; }
        public string company { get; set; }
        public string? bu { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string? comment { get; set; }
        public string htmlPresentation { get; set; }
        public List<TFile> files { get; set; } = new List<TFile>();

        public string? previousDraftID { get; set; }

    }
    public class Files
    {
        public string? service { get; set; }
        public string? name { get; set; }
        public string link { get; set; }
    }

    public class POFlowStage
    {
        public int step { get; set; }
        public string performer { get; set; }
        public int? deadlineDays { get; set; }
        public string? description { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? completionDate { get; set; }
        public int result { get; set; }
        public string? performComment { get; set; }
        public string sapapprovelink { get; set; }
        public string? saprejectlink { get; set; }
    }
    public class SAPMessageReturn
    {
        public int errorCode { get; set; }
        public string errorDescription { get; set; }
        public string dataType { get; set; }
        public string? draftID { get; set; }
        public string appName { get; set; }
    }


    public class User
    {
        public int status { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class Users
    {
        List<User> users { get; set; }
    }

    //public class objOutPutPO
    //{
    //    public objErrorSap ES_RESULT { get; set; }
    //    //can edit fix lai
    //    public List<POFlow> ET_DATA { get; set; }
    //}
    public class objErrorSap
    {
        public string TYPE { get; set; }
        public string MESSAGE { get; set; }
    }
    /// <summary>
    /// kết quả tích hợp từ SAP
    /// </summary>
    public class ObjResIntegrated
    {
        public string Code { get; set; }
        public int RowIntegrated { get; set; }
        public string Message { get; set; }
    }

    public class TFunctionRole : TFunction
    {
        public bool? UserView { get; set; }
        public bool? UserNew { get; set; }
        public bool? UserEdit { get; set; }
        public bool? UserDelete { get; set; }
        public bool? UserApprove { get; set; }
        public bool? UserReject { get; set; }
    }

    public class InputSearchRoleGroup
    {
        public int RoleGroupId { get; set; }
        public int CurrentPage { get; set; }
        public int NumperPage { get; set; }
    }

    /// <summary>
    /// tham số add người dùng vào nhóm người dùng
    /// </summary>
    public class InputAddRefRoleGroupUser
    {
        public int RoleGroupId { get; set; }
        public List<int> listIdUser { get; set; }
    }

    /// <summary>
    /// Đối tượng chức năng theo nhóm người dùng
    /// </summary>
    public class TRefRoleGroupFunctionAll : TRefRoleGroupFunction
    {
        public string DisplayName { get; set; }
        public int ParentId { get; set; }
        public int MenuId { get; set; }
        public string Type { get; set; }
        public bool IsParent { get; set; }
    }
    public class MessageAddUpdateReturn
    {
        public int Error { get; set; }
        public string sError { get; set; }
        public string Detail { get; set; }
        public dynamic Value { get; set; }

        public MessageAddUpdateReturn(int Error, string Detail)
        {
            this.Error = Error;
            this.Detail = Detail;
        }

        public MessageAddUpdateReturn(int Error, string Detail, dynamic Value)
        {
            this.Error = Error;
            this.Detail = Detail;
            this.Value = Value;
        }
    }
}