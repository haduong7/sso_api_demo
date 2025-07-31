using Common.Utilities.Security;
using Common.Utilities.Utils;
using Lib.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SSO_Api.Data;
using SSO_Api.DTO;
using SSO_Api.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SSO_Api.Repositories.RepositorySystemEF
{
    public class SystemManagement : ISystemManagement
    {

        private readonly AppDbContext _dbContext;

        public SystemManagement(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TUser> GetListSystemUser()
        {
            var q = _dbContext.TUsers.ToList();
            return q;
        }

        public TUser Login(ObjUser obj)
        {
            var objCheck = _dbContext.TUsers.FirstOrDefault(x =>
                x.IsDeleted == 0
                && x.Status == 1
                &&
                  (
                  (obj.UserName.ToLower() == "admin" && x.Password == PasswordHash.MD5Encrypt(obj.Password))
                   || (x.UserName.ToLower() == obj.UserName.ToLower() && x.Password == PasswordHash.MD5Encrypt(obj.Password))
                   //|| (x.Mobile.ToLower() == obj.UserName.ToLower() && x.Password == PasswordHash.MD5Encrypt(obj.Password))
                   || (x.Email.ToLower() == obj.UserName.ToLower() && x.Password == PasswordHash.MD5Encrypt(obj.Password)))
                );
            if (objCheck != null)
            {
                return objCheck;
            }
            return new TUser();
        }

        public int TUser_Add(TUserAddOrUpdate obj)
        {
            var objcheckExist = _dbContext.TUsers.Count(p => (p.UserName == obj.UserName || p.Email == obj.Email) && p.IsDeleted == 0);
            if (objcheckExist > 0)
            {
                return ErrorCode.USERNAME_OR_EMAIL_OR_MOBILE_EXISTED;
            }
            //obj.UserName = GenerateAccountName(obj);

            _dbContext.TUsers.Add(obj);
            _dbContext.SaveChanges();
            return obj.UserId;
        }
        public ObjUser AccountSession(HttpContext httpContext)
        {
            try
            {
                var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (!string.IsNullOrEmpty(token))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                    var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
                    return JsonConvert.DeserializeObject<ObjUser>(claimValue);
                }
                else
                {
                    return new ObjUser();
                }

            }
            catch (Exception ex)
            {

                return new ObjUser();
            }
        }

        public int Authenticate(ObjUser model, ref ObjUserResponse obj)
        {
            var objUserRes = Login(model);
            var objUser = new ObjUser()
            {
                UserId = objUserRes.UserId,
                FullName = objUserRes.FullName,
                UserName = objUserRes.UserName,
                UserType = objUserRes.UserType,
                Email = objUserRes.Email,
                Mobile = objUserRes.Mobile,
                Gender = objUserRes.Gender,
                Avatar = objUserRes.Avatar
            };
            var res = ErrorCode.SYSTEM_EXECEPTION;
            var token = string.Empty;
            if (objUser.UserId == 0)
            {
                res = ErrorCode.NOT_LOGIN;
            }
            else
            {
                res = objUser.UserId;
                token = generateJwtToken(objUser);
            }
            // authentication successful so generate jwt token
            obj = new ObjUserResponse(objUser, token);
            return res;
        }

        private string generateJwtToken(ObjUser user)
        {
            //var secret = _config["Author_sap"];JWTAppSettingKey
            //phai chinh thanh lay tu appsettings
            var secret = "ThisIsAVeryStrongSecretKeyADG!@#";
            var expiresDay = "1";

            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var exprires = Convert.ToDouble(expiresDay);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user)) }),
                //Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString())}),
                //Expires = DateTime.UtcNow.AddHours(exprires),
                Expires = DateTime.UtcNow.AddDays(exprires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Lấy danh sách nhóm quyền
        /// </summary>
        /// <param name="objInput"></param>
        /// <returns></returns>
        public List<TRoleGroup> GetListRoleGroup(TRoleGroup objInput, ref int TotalRecord, int pageNumber = 1, int pageSize = 40)
        {
            var q = _dbContext.TRoleGroups.Where(x =>
             (string.IsNullOrEmpty(objInput.RoleGroupName) || x.RoleGroupName.Contains(objInput.RoleGroupName))
            && (objInput.RoleGroupId == 0 || x.RoleGroupId == objInput.RoleGroupId)
            );

            TotalRecord = q.Count();
            return q.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        }

        public int TRoleGroup_Add(TRoleGroup obj)
        {
            _dbContext.TRoleGroups.Add(obj);
            _dbContext.SaveChanges();
            return obj.RoleGroupId;
        }

        public int TRoleGroup_Update(TRoleGroup objInput)
        {
            var objCheck = _dbContext.TRoleGroups.Find(objInput.RoleGroupId);
            if (objCheck != null)
            {
                objCheck.RoleGroupName = objInput.RoleGroupName;
                objCheck.Status = objInput.Status;
                objCheck.CreatedDate = objInput.CreatedDate;
                _dbContext.SaveChanges();
            }
            return objInput.RoleGroupId;
        }
        public int TRoleGroup_Delete(TRoleGroup objInput)
        {
            //Xóa 
            var objCheck = _dbContext.TRoleGroups.Find(objInput.RoleGroupId);
            if (objCheck != null)
            {
                _dbContext.TRoleGroups.Remove(objCheck);
                _dbContext.SaveChanges();
            }
            return objInput.RoleGroupId;
        }

        /// <summary>
        /// Lấy danh sách chức năng theo nhóm quyền
        /// </summary>
        /// <param name="objInput"></param>
        /// <returns></returns>
        public string GetListFunctionByRoleGroupId(int RoleGroupId)
        {
            var q = from t in _dbContext.TRefRoleGroupFunctions
                    where t.RoleGroupId == RoleGroupId
                    select t.FunctionId;
            var obj = q.ToList();
            return String.Join("|", obj);
        }
        /// <summary>
        /// Danh sách user theo nhóm quyền
        /// </summary>
        /// <param name="objInput"></param>
        /// <returns></returns>
        public string GetListUserByRoleGroupId(int RoleGroupId)
        {
            var q = from t in _dbContext.TRefUserRoleGroups
                    where t.RoleGroupId == RoleGroupId
                    select t.UserId;
            var obj = q.ToList();
            return String.Join("|", obj);
        }

        /// <summary>
        /// Lấy danh sách chức năng menu
        /// </summary>
        /// <param name="objInput"></param>
        /// <returns></returns>
        public List<TFunction> GetListFunction(TFunction objInput, ref int TotalRecord, int pageNumber = 1, int pageSize = 40)
        {
            var q = _dbContext.TFunctions.Where(x =>
             (string.IsNullOrEmpty(objInput.FunctionName) || x.FunctionName.Contains(objInput.FunctionName))
            && (objInput.FunctionId == 0 || x.FunctionId == objInput.FunctionId)
            );
            TotalRecord = q.Count();
            return q.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Lấy danh sách chức năng theo user id đã được phân quyền
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<TFunctionRole> GetListFunctionByUserIdPermission(int UserId, string userName)
        {
            if (userName.ToLower() == "admin")
            {
                var q = from t1 in _dbContext.TFunctions
                        where t1.Status == 1
                        select new TFunctionRole()
                        {
                            FunctionId = t1.FunctionId,
                            FunctionName = t1.FunctionName,
                            PageUrl = t1.PageUrl,
                            ParentId = t1.ParentId,
                            SortOrder = t1.SortOrder,
                            MenuId = t1.MenuId,
                            Icon = t1.Icon,
                            UserView = true,
                            UserNew = true,
                            UserEdit = true,
                            UserDelete = true,
                            UserApprove = true,
                            UserReject = true,
                        };
                return q.OrderBy(p => p.SortOrder).ToList();
            }
            else
            {
                var q = (from t1 in _dbContext.TRefUserRoleGroups
                         join t2 in _dbContext.TRefRoleGroupFunctions on t1.RoleGroupId equals t2.RoleGroupId
                         join t3 in _dbContext.TFunctions on t2.FunctionId equals t3.FunctionId
                         where t3.Status == 1 && t1.UserId == UserId
                         select new TFunctionRole()
                         {
                             FunctionId = t3.FunctionId,
                             FunctionName = t3.FunctionName,
                             PageUrl = t3.PageUrl,
                             ParentId = t3.ParentId,
                             SortOrder = t3.SortOrder,
                             Icon = t3.Icon,
                             MenuId = t3.MenuId,
                             UserView = t2.UserView,
                             UserNew = t2.UserNew,
                             UserEdit = t2.UserEdit,
                             UserDelete = t2.UserDelete,
                             UserApprove = t2.UserApprove,
                             UserReject = t2.UserReject,
                         }).Distinct();
                var lstParentId = q.Distinct().Select(p => p.ParentId);//Lấy ra list function cha
                                                                       // var lstDataParent = _dbContext.TFunctions.Where(p => lstParentId.Contains(p.FunctionId)).Distinct().ToList();
                var lst = q.Distinct().ToList();
                // lst.AddRange(lstDataParent);
                return lst.Distinct().OrderBy(p => p.SortOrder).ToList();
            }
        }

        /// <summary>
        /// Lấy thông tin phân quyền chức năng view, new, edit, delete theo UserID, ID function
        /// </summary>
        /// <param name="UserId">ID người sử dụng</param>
        /// <param name="FuntionName">tên Function Name </param>
        /// <returns></returns>
        public TRefRoleGroupFunction GetFunctionViewNewEditDeleteByUserIdPermission(int UserId, string FuntionName)
        {
            TRefRoleGroupFunction obj = new TRefRoleGroupFunction();
            int FuntionId = 0;
            //Lấy FunctionID theo FunctionName
            var queryFunction = (from p in _dbContext.TFunctions
                                 where p.FunctionName == FuntionName
                                 select p).FirstOrDefault();
            if (queryFunction != null)
            {
                FuntionId = queryFunction.FunctionId;
            }
            try
            {
                if (UserId == 1) //quyền admin
                {
                    var q = from t1 in _dbContext.TRefRoleGroupFunctions
                            where t1.FunctionId == FuntionId
                            select new TRefRoleGroupFunction()
                            {
                                RoleGroupFunctionId = t1.RoleGroupFunctionId,
                                FunctionId = t1.FunctionId,
                                RoleGroupId = t1.RoleGroupId,
                                UserView = true,
                                UserNew = true,
                                UserEdit = true,
                                UserDelete = true
                            };
                    obj = q.FirstOrDefault();
                }
                else
                {
                    obj.FunctionId = FuntionId;
                    obj.UserView = false;
                    obj.UserNew = false;
                    obj.UserEdit = false;
                    obj.UserDelete = false;
                    obj.UserApprove = false;
                    obj.UserReject = false;

                    var listRole = (from t1 in _dbContext.TRefUserRoleGroups
                                    join t2 in _dbContext.TRefRoleGroupFunctions on t1.RoleGroupId equals t2.RoleGroupId
                                    join t3 in _dbContext.TFunctions on t2.FunctionId equals t3.FunctionId
                                    where t3.Status == 1 && t1.UserId == UserId && t2.FunctionId == FuntionId
                                    select new TRefRoleGroupFunction()
                                    {
                                        RoleGroupFunctionId = t2.RoleGroupFunctionId,
                                        FunctionId = t2.FunctionId,
                                        RoleGroupId = t2.RoleGroupId,
                                        UserView = t2.UserView,
                                        UserNew = t2.UserNew,
                                        UserEdit = t2.UserEdit,
                                        UserDelete = t2.UserDelete,
                                        UserApprove = t2.UserApprove,
                                        UserReject = t2.UserReject,
                                    }).ToList();
                    if (listRole != null)
                    {
                        foreach (var item in listRole)
                        {
                            if (item.UserView == true)
                            {
                                obj.UserView = true;
                            }
                            if (item.UserNew == true)
                            {
                                obj.UserNew = true;
                            }
                            if (item.UserEdit == true)
                            {
                                obj.UserEdit = true;
                            }
                            if (item.UserDelete == true)
                            {
                                obj.UserDelete = true;
                            }
                            if (item.UserApprove == true)
                            {
                                obj.UserApprove = true;
                            }
                            if (item.UserReject == true)
                            {
                                obj.UserReject = true;
                            }
                        }
                    }

                }
                return obj;
            }
            catch (Exception)
            {
                obj.RoleGroupFunctionId = 0;
                obj.FunctionId = FuntionId;
                obj.RoleGroupId = 0;
                obj.UserView = false;
                obj.UserNew = false;
                obj.UserEdit = false;
                obj.UserDelete = false;
                return obj;
            }

        }

        /// <summary>
        /// Danh sách người dùng theo nhóm người dùng
        /// </summary>
        /// <param name="objinput"></param>
        /// <returns></returns>
        public dynamic GetUserByRoleGroupPaging(InputSearchRoleGroup objinput)
        {
            try
            {
                var query = (from p in _dbContext.TRefUserRoleGroups
                             join t1 in _dbContext.TUsers on p.UserId equals t1.UserId
                             where p.RoleGroupId == objinput.RoleGroupId
                             select t1
                             );
                var TotalRecord = query.Count();
                var listKhuon = query.Skip((objinput.CurrentPage - 1) * objinput.NumperPage).Take(objinput.NumperPage).ToList();
                //tổng số trang(pages)
                var TotalPage = Convert.ToInt32(Math.Ceiling(TotalRecord / Convert.ToDouble(objinput.NumperPage)));
                //kết quả API
                ValueData objValueData = new ValueData();
                objValueData.paging = new Pagingitem()
                {
                    TotalPage = TotalPage,
                    TotalItem = TotalRecord,
                    CurrentPage = objinput.CurrentPage
                };
                objValueData.items = listKhuon;
                return new MessageReturn(ErrorCode.SUCCESS_EXECEPTION, "Lấy dữ liệu danh sách người dùng theo nhóm người dùng", objValueData);
            }
            catch (Exception ex)
            {
                return new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi lấy dữ liệu danh sách người dùng theo nhóm người dùng': " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách người sử dụng
        /// </summary>
        /// <returns></returns>
        public dynamic GetUserAll()
        {
            try
            {
                var query = (from p in _dbContext.TUsers
                             where p.IsDeleted == 0
                             select p
                             );
                var TotalRecord = query.Count();
                var listUser = query.ToList();
                return new MessageAddUpdateReturn(ErrorCode.SUCCESS_EXECEPTION, "Lấy dữ liệu danh sách người dùng", listUser);
            }
            catch (Exception ex)
            {
                return new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi lấy dữ liệu danh sách người dùng': " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm User vào nhóm người dùng
        /// </summary>
        /// <param name="data"></param>       
        /// <returns></returns>
        public dynamic AddUserToGroup(InputAddRefRoleGroupUser inputdata)
        {
            try
            {
                List<TRefUserRoleGroup> listTRefUserRoleGroup = new List<TRefUserRoleGroup>();
                foreach (var iduser in inputdata.listIdUser)
                {
                    var first = _dbContext.TRefUserRoleGroups.FirstOrDefault(q => q.RoleGroupId == inputdata.RoleGroupId && q.UserId == iduser);
                    if (first == null)
                    {
                        TRefUserRoleGroup newitem = new TRefUserRoleGroup();
                        newitem.RoleGroupId = inputdata.RoleGroupId;
                        newitem.UserId = iduser;
                        listTRefUserRoleGroup.Add(newitem);
                    }
                }
                using var transaction = _dbContext.Database.BeginTransaction();
                {
                    try
                    {
                        _dbContext.TRefUserRoleGroups.AddRange(listTRefUserRoleGroup);
                        _dbContext.SaveChanges();
                        transaction.Commit();
                        return new MessageAddUpdateReturn(ErrorCode.SUCCESS_EXECEPTION, "Thêm người dùng vào nhóm thành công");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new MessageAddUpdateReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi thêm người dùng vào nhóm': " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return new MessageAddUpdateReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi thêm người dùng vào nhóm: " + ex.Message);
            }
        }

        /// <summary>
        /// Xóa User từ nhóm người dùng
        /// </summary>
        /// <param name="inputdata"></param>
        /// <returns></returns>
        public dynamic DeleteUserToGroup(InputAddRefRoleGroupUser inputdata)
        {
            try
            {
                List<TRefUserRoleGroup> listTRefUserRoleGroup = new List<TRefUserRoleGroup>();
                foreach (var iduser in inputdata.listIdUser)
                {
                    var first = _dbContext.TRefUserRoleGroups.FirstOrDefault(q => q.RoleGroupId == inputdata.RoleGroupId && q.UserId == iduser);
                    if (first != null)
                    {
                        _dbContext.TRefUserRoleGroups.Remove(first);
                    }
                }
                using var transaction = _dbContext.Database.BeginTransaction();
                {
                    try
                    {
                        _dbContext.SaveChanges();
                        transaction.Commit();
                        return new MessageAddUpdateReturn(ErrorCode.SUCCESS_EXECEPTION, "Xóa người dùng vào nhóm thành công");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new MessageAddUpdateReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi xóa người dùng vào nhóm': " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return new MessageAddUpdateReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi xóa người dùng vào nhóm: " + ex.Message);
            }
        }

        /// <summary>
        /// Danh sách chức năng(menu) theo nhóm người dùng
        /// </summary>
        /// <param name="inputRoleGroup"></param>
        /// <returns></returns>
        public dynamic GetRoleGroupFunctionAll(TRoleGroup inputRoleGroup)
        {
            try
            {
                var query = (from p in _dbContext.TFunctions
                             where p.Status == 1
                             orderby p.SortOrder ascending
                             select new TRefRoleGroupFunctionAll()
                             {
                                 RoleGroupFunctionId = 0,
                                 RoleGroupId = inputRoleGroup.RoleGroupId,
                                 FunctionId = p.FunctionId,
                                 DisplayName = p.DisplayName,
                                 ParentId = (int)p.ParentId,
                                 IsParent = p.Type == "MODULE" ? true : false,
                                 MenuId = (int)p.MenuId,
                                 Type = p.Type,
                                 UserView = false,
                                 UserNew = false,
                                 UserEdit = false,
                                 UserDelete = false,
                                 UserApprove = false,
                                 UserReject = false,
                             });
                var listFunctions = query.ToList();
                //update quyền chức năng
                foreach (var item in listFunctions)
                {
                    var first = _dbContext.TRefRoleGroupFunctions.FirstOrDefault(q => q.RoleGroupId == inputRoleGroup.RoleGroupId && q.FunctionId == item.FunctionId);
                    if (first != null)
                    {
                        item.RoleGroupFunctionId = first.RoleGroupFunctionId;
                        item.UserView = first.UserView;
                        item.UserNew = first.UserNew;
                        item.UserEdit = first.UserEdit;
                        item.UserDelete = first.UserDelete;
                        item.UserApprove = first.UserApprove;
                        item.UserReject = first.UserReject;
                        if (item.Type == "MODULE")
                        {
                            item.IsParent = true;
                        }
                    }
                }
                return new MessageAddUpdateReturn(ErrorCode.SUCCESS_EXECEPTION, "Lấy dữ liệu danh sách chức năng/menu", listFunctions);
            }
            catch (Exception ex)
            {
                return new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi lấy dữ liệu danh sách chức năng menu': " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm chức năng (menu) vào nhóm người dùng
        /// </summary>
        /// <param name="listItem"></param>
        /// <returns></returns>
        public dynamic AddFunctionToGroup(List<TRefRoleGroupFunctionAll> listItem)
        {
            try
            {
                List<TRefRoleGroupFunction> listTRefRoleGroupFunction = new List<TRefRoleGroupFunction>();
                foreach (var item in listItem)
                {
                    var first = _dbContext.TRefRoleGroupFunctions.FirstOrDefault(q => q.RoleGroupId == item.RoleGroupId && q.FunctionId == item.FunctionId);
                    if (first == null)
                    {
                        TRefRoleGroupFunction newitem = new TRefRoleGroupFunction();
                        newitem.RoleGroupFunctionId = 0;
                        newitem.RoleGroupId = item.RoleGroupId;
                        newitem.FunctionId = item.FunctionId;
                        newitem.UserView = item.UserView;
                        newitem.UserNew = item.UserNew;
                        newitem.UserEdit = item.UserEdit;
                        newitem.UserDelete = item.UserDelete;
                        newitem.UserApprove = item.UserApprove;
                        newitem.UserReject = item.UserReject;
                        _dbContext.TRefRoleGroupFunctions.Add(newitem);
                        //_dbContext.SaveChanges();
                    }
                    else
                    {
                        first.UserView = item.UserView;
                        first.UserNew = item.UserNew;
                        first.UserEdit = item.UserEdit;
                        first.UserDelete = item.UserDelete;
                        first.UserApprove = item.UserApprove;
                        first.UserReject = item.UserReject;
                        // _dbContext.SaveChanges();
                    }
                }
                using var transaction = _dbContext.Database.BeginTransaction();
                {
                    try
                    {
                        _dbContext.SaveChanges();
                        transaction.Commit();
                        return new MessageAddUpdateReturn(ErrorCode.SUCCESS_EXECEPTION, "Thêm chức năng vào nhóm người dùng thành công");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new MessageAddUpdateReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi thêm chức năng vào nhóm người dùng': " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return new MessageAddUpdateReturn(ErrorCode.SYSTEM_EXECEPTION, "Lỗi thêm chức năng vào nhóm người dùng : " + ex.Message);
            }
        }

        public ObjUserMemberShipGroup GetAccountInforV2(int userId)
        {
            var query = from t in _dbContext.TUsers
                        where t.UserId == userId
                        select new ObjUserMemberShipGroup()
                        {
                            UserId = t.UserId,
                            UserName = t.UserName,
                            UserType = t.UserType,
                            FullName = t.FullName,
                            Avatar = t.Avatar,
                            Email = t.Email,
                            Mobile = t.Mobile,
                            Gender = t.Gender,
                        };
            return query.FirstOrDefault();
        }

        public List<TUserAddOrUpdate> TUser_GetAll(ref int TotalRecord, TUser objInput, int DeptId = 0, int pageNumber = 1, int pageSize = 40)
        {
            var query = from p in _dbContext.TUsers
                        where (string.IsNullOrEmpty(objInput.FullName) || p.FullName.Contains(objInput.FullName))
                            && (string.IsNullOrEmpty(objInput.UserName) || p.UserName.Contains(objInput.UserName))
                            && (string.IsNullOrEmpty(objInput.Email) || p.Email.Contains(objInput.Email))
                            && (string.IsNullOrEmpty(objInput.Mobile) || p.Mobile.Contains(objInput.Mobile))
                            && (objInput.UserId == 0 || p.UserId == objInput.UserId)
                            && (objInput.UserType == -1 || p.UserType == objInput.UserType)
                            && (objInput.Gender == -1 || p.Gender == objInput.Gender)
                            && p.IsDeleted != 1
                            && p.UserName.ToLower() != "admin"
                        orderby p.FullName
                        select new TUserAddOrUpdate()
                        {
                            UserId = p.UserId,
                            UserName = p.UserName,
                            FullName = p.FullName,
                            Status = p.Status,
                            Email = p.Email,
                            Mobile = p.Mobile,
                            UserType = p.UserType,
                            Gender = p.Gender,
                            Birthday = p.Birthday,
                            IdOrPassport = p.IdOrPassport,
                            Address = p.Address,
                            JobDescription = p.JobDescription,
                            IsDeleted = p.IsDeleted,
                            Avatar = p.Avatar,
                        };
            TotalRecord = query.Count();
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<TUserType> GetListUserType()
        {
            var query = from p in _dbContext.TUserTypes
                        where p.Status == 1
                        select p;
            return query.ToList();
        }

        public int TUser_Delete(TUser objInput)
        {
            var objCheck = _dbContext.TUsers.Find(objInput.UserId);
            if (objCheck != null)
            {
                objCheck.IsDeleted = 1;
                _dbContext.SaveChanges();
                //LogAction(new TLogAction()
                //{
                //    User = objInput.UserName,//Gán userName người thực hiện xóa tạm vào biến UserName truyền vào
                //    Note = $"Delete user by {objCheck.UserName} ==> {JsonConvert.SerializeObject(objCheck)}",
                //    ActionName = LogActionType.DeleteUser
                //});
            }
            return objInput.UserId;
        }

        public int TUser_Update(TUserAddOrUpdate objInput)
        {
            var objCheck = _dbContext.TUsers.Find(objInput.UserId);
            if (objCheck != null)
            {
                var objcheckExist = _dbContext.TUsers.Count(p => (p.UserName == objInput.UserName || p.Email == objInput.Email) && p.IsDeleted == 0 && p.UserId != objInput.UserId);
                if (objcheckExist > 0)
                {
                    return ErrorCode.USERNAME_OR_EMAIL_OR_MOBILE_EXISTED;
                }
                objCheck.UserName = objInput.UserName;// objInput.UserName;
                if (objInput.Password.Length > 0)
                {
                    objCheck.Password = objInput.Password;
                }
                objCheck.FullName = objInput.FullName;
                objCheck.UserType = objInput.UserType;
                objCheck.Status = objInput.Status;
                objCheck.Email = objInput.Email;
                objCheck.Mobile = objInput.Mobile;
                objCheck.Gender = objInput.Gender;
                objCheck.Birthday = objInput.Birthday;
                objCheck.Address = objInput.Address;
                objCheck.JobDescription = objInput.JobDescription;
                _dbContext.SaveChanges();
            }
            return objInput.UserId;
        }

        public async Task<dynamic> getSapUserList(string? filter, int? fields, int? limit)
        {
            try
            {
                var webHelpers = new WebHelpers();
                var responsesap = await webHelpers.GetUserListFromSap(filter, fields, limit);
                if (!string.IsNullOrEmpty(responsesap))
                {
                    var sapUserList = JsonConvert.DeserializeObject<SAPMessageReturnJson>(responsesap.ToString()??"");
                    return new MessageAddUpdateReturn(ErrorCode.SUCCESS_EXECEPTION, "Lấy dữ liệu danh sách người dùng từ SAP thành công", sapUserList);
                }
            }
            catch (Exception ex)
            {

                // Log the exception
                Console.WriteLine($"Error in getSapUserList: {ex.Message}");
            }
            throw new NotImplementedException();
        }

        public async Task<ResultCompareUserSys> getSapUserList()
        {
            try
            {
                ResultCompareUserSys returnList = new ResultCompareUserSys();

                // Lấy danh sách người dùng từ SAP
                var webHelpers = new WebHelpers();
                string? filter = null;
                int? fields = null;
                int? limit = null;
                var responsesap = await webHelpers.GetUserListFromSap(filter, fields, limit);
                if (!string.IsNullOrEmpty(responsesap))
                {
                    var sapUserList = JsonConvert.DeserializeObject<SAPMessageReturnJson>(responsesap.ToString() ?? "");
                    if (sapUserList != null && sapUserList.ES_USER.USERS.Count > 0)
                    {
                        foreach (var user in sapUserList.ES_USER.USERS)
                        {
                            // Chuyển đổi dữ liệu từ SAP sang định dạng mong muốn
                            returnList.listUserSysCompare.Add(new UserSysCompare()
                            {

                                UserName = user.USERNAME,
                                FullName = user.FULLNAME,
                                Email = user.EMAIL,
                                PhoneNumber = user.PHONE,
                                Dob = user.DATEOFBIRTH,
                                statusSap = user.STATUS,
                                onSap = true,
                                onMyADG = false,
                            });
                        }
                    }
                    //return returnList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();   
                }


                // Lấy danh sách người dùng từ hệ thống MyADG
                var listUserADG = _dbContext.TUsers.Where(p => p.IsDeleted == 0 && p.UserName.ToLower() != "admin").ToList();
                if (listUserADG != null && listUserADG.Count > 0)
                {
                    foreach (var user in listUserADG)
                    {
                        // Chuyển đổi dữ liệu từ MyADG sang định dạng mong muốn
                        // Kiểm tra xem người dùng đã có trong danh sách SAP chưa
                        var existingUser = returnList.listUserSysCompare.FirstOrDefault(u => u.UserName.ToLower() == user.UserName.ToLower());
                        if (existingUser != null)
                        {
                            // Nếu đã có, cập nhật thông tin
                            if (existingUser.FullName != user.FullName
                                || existingUser.Email != user.Email 
                                || existingUser.PhoneNumber != user.Mobile 
                                || existingUser.Dob != user.Birthday) {
                                existingUser.infoCorrect = false; // Đánh dấu thông tin cần cập nhật
                            }
                            existingUser.UserId = user.UserId;
                            existingUser.onMyADG = true;
                        }
                        else
                        {
                            // Nếu chưa có, thêm mới
                            returnList.listUserSysCompare.Add(new UserSysCompare()
                            {
                                UserName = user.UserName,
                                FullName = user.FullName,
                                Email = user.Email,
                                PhoneNumber = user.Mobile,
                                Dob = user.Birthday,
                                statusSap = -1, // có trên MyADG nhưng không có trên SAP
                                onSap = false,
                                onMyADG = true,
                            });
                        }
                        
                    }
                }
                returnList.soTaiKhoanMyADG= returnList.listUserSysCompare.Count(p => p.onMyADG == true);
                returnList.soTaiKhoanSAP = returnList.listUserSysCompare.Count(p => p.onSap == true);
                return returnList;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in getSapUserList: {ex.Message}");
            }
            throw new NotImplementedException();
        }

        
    }
}

