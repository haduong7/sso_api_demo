using Common.Utilities.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SSO_Api.DTO;
using SSO_Api.Helpers;
using SSO_Api.Repositories.MyAdgEF;
using SSO_Api.Repositories.RepositorySystemEF;

namespace SSO_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeXuatController : ControllerBase
    {

        private readonly IDeXuatManagement deXuatManagement;
        public DeXuatController(IDeXuatManagement deXuatManagement)
        {
            this.deXuatManagement = deXuatManagement;
        }
        public IActionResult Index()
        {
            return Ok(new { code = 1, data = "DeXuat" });
        }

        ////[Authorize]            
        //[HttpGet]
        //[Route("GetDeXuatPaging")]
        //public ActionResult GetDeXuatPaging(int pageNumber = 1, int pageSize = 10)
        //{
        //    try
        //    {
        //        var TotalRecord = 0;
        //        //var userId = _IUserService.AccountSession(HttpContext).UserId;
        //        var lstData = deXuatManagement.GetSanPhamPaging(ref TotalRecord, pageNumber, pageSize);

        //        var TotalPage = Convert.ToInt32(Math.Ceiling(TotalRecord / Convert.ToDouble(pageSize)));
        //        //kết quả API
        //        ValueData objValueData = new ValueData();
        //        objValueData.paging = new Pagingitem()
        //        {
        //            TotalPage = TotalPage,
        //            TotalItem = TotalRecord,
        //            CurrentPage = pageNumber
        //        };
        //        objValueData.items = lstData;

        //        MessageReturn objReturn = new MessageReturn(lstData.Count(), "Danh sách đề xuất", objValueData);

        //        return Ok(objReturn);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageReturn objReturn = new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Danh sách đề xuất", new ValueData());
        //        return Ok(objReturn);
        //    }
        //}

        //[Authorize]            
        [HttpGet]
        [Route("GetDeXuatPaging")]
        public ActionResult GetAllPoData(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var TotalRecord = 0;
                //var userId = _IUserService.AccountSession(HttpContext).UserId;
                var lstData = deXuatManagement.getAllPoData(ref TotalRecord, pageNumber, pageSize);

                var TotalPage = Convert.ToInt32(Math.Ceiling(TotalRecord / Convert.ToDouble(pageSize)));
                //kết quả API
                ValueData objValueData = new ValueData();
                objValueData.paging = new Pagingitem()
                {
                    TotalPage = TotalPage,
                    TotalItem = TotalRecord,
                    CurrentPage = pageNumber
                };
                objValueData.items = lstData;

                MessageReturn objReturn = new MessageReturn(lstData.Count(), "Danh sách đề xuất", objValueData);

                return Ok(objReturn);
            }
            catch (Exception ex)
            {
                MessageReturn objReturn = new MessageReturn(ErrorCode.SYSTEM_EXECEPTION, "Danh sách đề xuất", new ValueData());
                return Ok(objReturn);
            }
        }

        /// <summary>
        /// Lấy thông tin loại user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetQuyTrinhDeXuat")]
        public ActionResult GetPoFlowStagesByPoDataId(int poId)
        {
            try
            {
                var lstData = deXuatManagement.GetPoFlowStagesByPoDataId(poId);
                return Ok(new { code = lstData.Count, data = lstData });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = -99, data = ex.Message });
            }

        }

        /// <summary>
        /// xoa po ( lam ra cho tien khi test)
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [Route("deletePO")]
        [BasicAuth]
        public ActionResult Delete(int poId)
        {
            try
            {
                var lstData = deXuatManagement.DeletePO_PoProcess(poId);
                return Ok(new { code = poId, data = "Trang thai poId da xoa: " + lstData });
            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                return Ok(new { code = -99, data = ex.Message });
            }

        }
    }
}
