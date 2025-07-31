using SSO_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SSO_Api.Repositories.RepositorySystemEF;

namespace SSO_Api.Controllers;

//[Authorize]
public class HomeController : ControllerBase
{

    private readonly ISystemManagement _repository;

    public HomeController(ISystemManagement repository)
    {
        _repository = repository;
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

    public IActionResult Test()
    {
        //return View();
        try
        {
            var res = _repository.GetListSystemUser();
            return Ok(new { code = res.Count(), data = res });
        }
        catch (Exception ex)
        {
            
            return Ok(new { code = -99, data = ex.Message });
        }
    }

    //public IActionResult Privacy()
    //{
    //    return View();
    //}

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}
    /// <summary>
    /// Kiểm tra đăng nhập hệ thống
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
}