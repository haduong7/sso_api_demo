using RestSharp;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Utilities.Utils
{
    public class TokenJwt
    {
        public string token { get; set; }
        public string expireAt { get; set; }       
    }

    public class WebHelpers
    {
        private static IHttpContextAccessor _httpContextAccessor;
        private static IConfiguration _config;

        public static void Configure(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        public static readonly HttpClient _client = new HttpClient();

        public static HttpContext HttpContext
        {
            get { return _httpContextAccessor.HttpContext; }
        }
        
        

        

        public static string GetRemoteIP
        {
            get { return HttpContext.Connection.RemoteIpAddress.ToString(); }
        }

        public static string GetUserAgent
        {
            get { return HttpContext.Request.Headers["User-Agent"].ToString(); }
        }

        public static string GetScheme
        {
            get { return HttpContext.Request.Scheme; }
        }

        public static string ClientIP
        {
            get { return HttpContext.Connection.RemoteIpAddress.ToString(); }
        }

       

        
      

        public async Task<string> PostSAP(string urlApi, string jsonBody)
        {

            var basic = _config["Author_sap"];
            var prefix = _config["Url_prefix_sap"];
            var suffix = _config["Url_suffixes_sap"];

            var Authorization = "Basic " + basic;
            var urlRootApi = prefix;
            urlApi = urlApi + suffix;
            var options = new RestClientOptions(urlRootApi)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(urlApi, Method.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Authorization", Authorization);
            request.AddStringBody(jsonBody, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            return response.Content;
        }

       

        //public async Task<string> PostUpdateStatusPaymentToSap(string urlApi, string jsonBody)
        //{
        //    var Authorization = "Basic " + ConfigurationManager.AppSettings("Author_sap");
        //    var urlRootApi = ConfigurationManager.AppSettings("Url_prefix_sap");
        //   // var urlRootApi = ConfigurationManager.AppSettings("Url_prefix_sap");
        //    // urlApi = urlApi + ConfigurationManager.AppSettings("Url_suffixes_sap");
        //    var options = new RestClientOptions(urlRootApi)
        //    {
        //        MaxTimeout = -1,
        //    };
        //    var client = new RestClient(options);
        //    var request = new RestRequest(urlApi, Method.Post);
        //    request.AddHeader("Content-Type", "text/plain");
        //    request.AddHeader("Authorization", Authorization);
        //    request.AddStringBody(jsonBody, DataFormat.Json);
        //    RestResponse response = await client.ExecuteAsync(request);
        //    Console.WriteLine(response.Content);
        //    return response.Content;
        //}

        ////Hàm call API Đại lý từ SAP
        //public async Task<string> PostDaiLy(string urlApi, string jsonBody)
        //{
        //    var urlRootApi = ConfigurationManager.AppSettings("Url_prefix_sap");
        //    var urlCookie = ConfigurationManager.AppSettings("Cookie_sap");

        //    urlApi = urlRootApi + urlApi;
        //    var options = new RestClientOptions(urlRootApi)
        //    {
        //        MaxTimeout = -1,
        //    };
        //    var client = new RestClient(options);
        //    var request = new RestRequest(urlApi, Method.Post);
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddHeader("Cookie", urlCookie);
        //    request.AddStringBody(jsonBody, DataFormat.Json);
        //    RestResponse response = await client.ExecuteAsync(request);
        //    Console.WriteLine(response.Content);
        //    return response.Content;
        //}


        /// <summary>
        /// hàm Generate số OTP duy nhất
        /// </summary>
        /// <param name="iOTPLength"></param>
        /// <param name="saAllowedCharacters"></param>
        /// <returns></returns>
        public string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }

        ///// <summary>
        ///// Chức năng lấy file pdf từ sap
        ///// </summary>
        ///// <returns></returns>
        //public async Task<string> GetPdrFromSap(string iv_guiid = "", string iv_type = "")
        //{           
        //    var urlRootApi = ConfigurationManager.AppSettings("Url_prefix_sap");
        //    var urlCookie = ConfigurationManager.AppSettings("Cookie_sap");
        //    var Url_suffixes_sap = ConfigurationManager.AppSettings("Url_suffixes_sap");

        //    urlRootApi = "http://erp.austdoorgroup.vn:8000";
        //    Url_suffixes_sap = "?sap-client=900";
        //    string urlApi = "/adg/MAPPDF"+ Url_suffixes_sap + "&iv_guiid=" + iv_guiid + "&iv_type=" + iv_type + "";           
        //    var options = new RestClientOptions(urlRootApi)
        //    {
        //        MaxTimeout = -1,
        //    };
        //    var client = new RestClient(options);
        //    var request = new RestRequest(urlApi, Method.Get);
        //    request.AddHeader("Cookie", "sap-usercontext=sap-client=900");           
        //    //request.AddHeader("Cookie", urlCookie);
        //    RestResponse response = await client.ExecuteAsync(request);
        //    Console.WriteLine(response.Content);          
        //    return response.Content;
        //}


        /// <summary>
        /// Chức năng lấy list user từ sap
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetUserListFromSap(string? filter, int? fields, int? limit)
        {
            //var urlRootApi = ConfigurationManager.AppSettings("Url_prefix_sap");
            //var urlCookie = ConfigurationManager.AppSettings("Cookie_sap");
            //var Url_suffixes_sap = ConfigurationManager.AppSettings("Url_suffixes_sap");

            var urlRootApi = "http://103.21.148.147:8011/adg/users";
            var Url_suffixes_sap = "?sap-client=600";
            
            string urlApi = Url_suffixes_sap ;
            if (!string.IsNullOrEmpty(filter))
            {
                urlApi += "&filter=" + filter;
            }
            if (fields.HasValue)
            {
                urlApi += "&fields=" + fields.Value;
            }
            if (limit.HasValue)
            {
                urlApi += "&limit=" + limit.Value;
            }
            urlApi += "";
            var options = new RestClientOptions(urlRootApi)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(urlApi, Method.Get);
            //request.AddHeader("Cookie", "sap-usercontext=sap-client=900");
            //request.AddHeader("Cookie", urlCookie);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            return response.Content;
        }


    }
}
