namespace SSO_Api.Helpers
{
    using Common.Utilities.Security;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.EntityFrameworkCore;
    using SSO_Api.Data;
    using System.Text;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BasicAuthAttribute : Attribute, IAuthorizationFilter
    {
        private const string API_KEY = "MyADG2025!";
        private const string API_KEY_HEADER = "APIKey";

        //private readonly AppDbContext _dbContext;
        //public BasicAuthAttribute(AppDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var req = context.HttpContext.Request;
            var dbContext = context.HttpContext.RequestServices.GetService<AppDbContext>();
            // Check API Key
            if (!req.Headers.TryGetValue(API_KEY_HEADER, out var providedApiKey) || providedApiKey != API_KEY)
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "Missing or invalid API Key"
                };
                return;
            }

            // Check Basic Auth
            if (!req.Headers.TryGetValue("Authorization", out var authHeader) ||
                string.IsNullOrWhiteSpace(authHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!authHeader.ToString().StartsWith("Basic "))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var encodedCredentials = authHeader.ToString().Substring("Basic ".Length).Trim();
            try
            {
                var credentialBytes = Convert.FromBase64String(encodedCredentials);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

                if (credentials.Length != 2)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
                else
                {
                    //logic xu ly username and password
                    var objCheck = dbContext.TUsers.FirstOrDefault(x =>
                    x.IsDeleted == 0
                    && x.Status == 1
                    &&
                      (
                      (credentials[0].ToLower() == "admin" && x.Password == PasswordHash.MD5Encrypt(credentials[1]))
                       || (x.UserName.ToLower() == credentials[0].ToLower() && x.Password == PasswordHash.MD5Encrypt(credentials[1]))
                       //|| (x.Mobile.ToLower() == obj.UserName.ToLower() && x.Password == PasswordHash.MD5Encrypt(obj.Password))
                       || (x.Email.ToLower() == credentials[0].ToLower() && x.Password == PasswordHash.MD5Encrypt(credentials[1])))
                    );
                    if (objCheck == null)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                    // Set username vào HttpContext để dùng lại sau này
                    context.HttpContext.Items["AuthenticatedUserId"] = objCheck.UserId;
                }

                    
            }
            catch
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }

}
