using Common.Utilities.Utils;
using Microsoft.AspNetCore.Mvc;
using SSO_Api.DTO;
using SSO_Api.Helpers;
using SSO_Api.Repositories.MyAdgEF;

namespace SSO_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SapController : ControllerBase
    {

        private readonly IDeXuatManagement deXuatManagement;
        public SapController(IDeXuatManagement deXuatManagement)
        {
            this.deXuatManagement = deXuatManagement;
        }

        ///// <summary>
        ///// Lấy thông tin quy trình 
        ///// </summary>
        ///// <returns></returns>
        //[Authorize]
        //[HttpGet]
        //[Route("po-data")]
        //public ActionResult GetPoFlowStagesByPoNumber(string poNumber, DateTime poDate)
        //{
        //    try
        //    {
        //        var lstData = deXuatManagement.GetPoFlowStagesByPoDataId(poId);
        //        return Ok(new { code = lstData.Count, data = lstData });
        //    }
        //    catch (Exception ex)
        //    {
        //        //NLogManager.PublishException(ex);
        //        return Ok(new { code = -99, data = ex.Message });
        //    }

        //}

        /// <summary>
        /// Them thông tin PO + quy trình 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [BasicAuth]
        [Route("po-data")]
        public ActionResult CreateUpdatePO_PoProcess([FromBody] OutPutAPICreateUpdatePO_PoProcess input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var userId = (int)(HttpContext.Items["AuthenticatedUserId"] ?? -1);
                var lstData = deXuatManagement.CreateUpdatePO_PoProcess(input, userId);
                return Ok(new { code = ErrorCode.ADD_PO_PO_PROCESS_SUCCESS, data = lstData.Result });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = ErrorCode.ADD_PO_PO_PROCESS_ERROR, data = ex.Message });
            }
        }
        /// <summary>
        /// Them thông tin PO + quy trình 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BasicAuth]
        [Route("po-data")]
        public ActionResult GetPoProcess([FromBody] InputAPIGetRoadMap input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {

                var lstData = deXuatManagement.GetPoProcess(input);
                return Ok(new { code = ErrorCode.ADD_PO_PO_PROCESS_SUCCESS, data = lstData });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = ErrorCode.ADD_PO_PO_PROCESS_ERROR, data = ex.Message });
            }
        } 
    }
    
}
