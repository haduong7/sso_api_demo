using SSO_Api.DTO;
using SSO_Api.Models;

namespace SSO_Api.Repositories.MyAdgEF
{
    public interface IDeXuatManagement
    {

        //List<TDeXuat> GetSanPhamPaging(ref int TotalRecord, int pageNumber = 1, int pageSize = 10);

        List<ObjPoDataShow> getAllPoData(ref int TotalRecord, int pageNumber = 1, int pageSize = 10);
        List<TPoFlowStage> GetPoFlowStagesByPoDataId(int poDataId);

        Task<SAPMessageReturn> CreateUpdatePO_PoProcess(OutPutAPICreateUpdatePO_PoProcess input, int? userId);

        OutPutAPICreateUpdatePO_PoProcess GetPoProcess(InputAPIGetRoadMap input);

        bool DeletePO_PoProcess(int poDataId);


    }
}
