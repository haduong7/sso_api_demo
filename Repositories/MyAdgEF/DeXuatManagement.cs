using Common.Utilities.Utils;
using Microsoft.EntityFrameworkCore;
using SSO_Api.Data;
using SSO_Api.DTO;
using SSO_Api.Models;
using SSO_Api.Utils;

namespace SSO_Api.Repositories.MyAdgEF
{
    public class DeXuatManagement : IDeXuatManagement
    {

        private readonly AppDbContext _dbContext;
        public DeXuatManagement(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        

        public List<ObjPoDataShow> getAllPoData(ref int TotalRecord, int pageNumber = 1, int pageSize = 10)
        {
            List<ObjPoDataShow> lstReturnAllPoData = new List<ObjPoDataShow>();
            var query = _dbContext.TPoDatas.ToList();
            if (query.Any())
            {
                foreach (var item in query)
                {
                    ObjPoDataShow returnObj = new ObjPoDataShow
                    {
                        poDataId = item.PoDataId,
                        DocType = item.docType,
                        Number = item.number,
                        Date = item.date,
                        Status = item.status,
                        AuthorId = item.authorId,
                        Author = item.author,
                        ResponsibilityId = item.responsibilityId,
                        Responsibility = item.responsibility,
                        Company = item.company,
                        Bu = item.bu,
                        Amount = item.amount,
                        Currency = item.currency,
                        Description = item.description,
                        Comment = item.comment,
                        HtmlPresentation = item.htmlPresentation,
                        //previousDraftID = item.previousDraftID,
                    };
                    List<TFile> listFile = new List<TFile>();
                    var fileQuery = _dbContext.TRefFilePos.Where(p => p.poDataId == returnObj.poDataId).ToList();
                    if(fileQuery.Any())
                    {
                        foreach (var itemFile in fileQuery)
                        {
                            var file = _dbContext.TFiles.FirstOrDefault(f => f.FileId == Convert.ToInt32(itemFile.fileId));
                            if (file != null)
                            {
                                listFile.Add(file);
                            }

                        }
                    }
                    returnObj.Files = listFile;
                    lstReturnAllPoData.Add(returnObj);
                }
            }
            TotalRecord = lstReturnAllPoData.Count();
            return lstReturnAllPoData.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<TPoFlowStage> GetPoFlowStagesByPoDataId(int poDataId)
        {
            var query = from p in _dbContext.TPoFlowStages
                        where p.poDataId == poDataId
                        select p;
            return query.ToList();
        }


        /// <summary>
        /// API tạo hoặc cập nhật PO / Quy trình PO
        /// </summary>
        /// <returns></returns>
        
        public async Task<SAPMessageReturn>CreateUpdatePO_PoProcess(OutPutAPICreateUpdatePO_PoProcess input, int? userId)
        {
            var draftId = "-1";
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                
                // Kiểm tra dữ liệu đầu vào (user tồn tại trong MyADG)
                var user = _dbContext.TUsers.FirstOrDefault(u => u.UserName == input.poData.author && u.IsDeleted == 0);
                var user2 = _dbContext.TUsers.FirstOrDefault(y => y.UserName == input.poData.responsibility && y.IsDeleted == 0);
                
                
                if (user != null)
                {
                    string defaultDocType = "PO"; //mặc định cho docType
                    
                    TPoData poData = new TPoData
                    {
                        number = input.poData.number,
                        date = (DateTime)input.poData.date,
                        status = input.poData.status,
                        authorId = user.UserId,
                        author = input.poData.author,
                        company = input.poData.company,
                        amount = input.poData.amount,
                        currency = input.poData.currency,
                        description = input.poData.description,
                        htmlPresentation = input.poData.htmlPresentation,
                        previousDraftID = input.poData.previousDraftID,

                        CreateBy = userId,
                        CreatedDate = DateTime.Now
                    };
                    if( input.poData.docType != null && input.poData.docType != "")
                    {
                        poData.docType = input.poData.docType;
                    }
                    else
                    {
                        poData.docType = defaultDocType;
                    }
                    if (input.poData.responsibility != null && input.poData.responsibility != "")
                    {
                        poData.responsibility = input.poData.responsibility;
                        poData.responsibilityId = user2.UserId;
                    }
                    if(input.poData.bu != null && input.poData.bu != "")
                    {
                        poData.bu = input.poData.bu;

                    }
                    if(input.poData.comment != null && input.poData.comment != "")
                    {
                        poData.comment = input.poData.comment;
                    }
                    _dbContext.TPoDatas.Add(poData);
                    await _dbContext.SaveChangesAsync();
                    draftId = poData.PoDataId.ToString();
                    //xử lý danh sách file

                    if (input.poData.files != null && input.poData.files.Count > 0)
                    {
                        foreach (var file in input.poData.files)
                        {
                            _dbContext.TFiles.Add(file);
                            _dbContext.SaveChanges();
                            TRefFilePo refFilePo = new TRefFilePo
                            {
                                poDataId = poData.PoDataId,
                                fileId = file.FileId
                            };
                            _dbContext.TRefFilePos.Add(refFilePo);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                    
                    
                    //Xử lý quy trình PO đi kèm
                    if ( input.poFlow != null && input.poFlow.Count > 0)
                    {
                        foreach (var stage in input.poFlow)
                        {
                            TPoFlowStage poFlowStage = new TPoFlowStage
                            {
                                poDataId = poData.PoDataId,
                                step = stage.step,
                                performer = stage.performer,
                                result = stage.result,
                                //sapapprovelink = GlobalVariable.PO_APPROVE_LINK + poData.PoDataId,
                                //saprejectlink = GlobalVariable.PO_REJECT_LINK + poData.PoDataId,
                                sapapprovelink = stage.sapapprovelink,
                                saprejectlink = stage.saprejectlink,

                            };
                            if(stage.deadlineDays != null && stage.deadlineDays > 0)
                            {
                                poFlowStage.deadlineDays = stage.deadlineDays;
                            }
                            if(stage.description != null && stage.description != "")
                            {
                                poFlowStage.description = stage.description;
                            }
                            if(stage.startDate != null)
                            {
                                poFlowStage.StartDate = stage.startDate;
                            }
                            if(stage.completionDate != null)
                            {
                                poFlowStage.CompletionDate = stage.completionDate;
                            }
                            if (stage.performComment != null && stage.performComment != "")
                            {
                                poFlowStage.performComment = stage.performComment;
                            }
                            _dbContext.TPoFlowStages.Add(poFlowStage);
                            await _dbContext.SaveChangesAsync();
                        }
                    }

                    //Neu refId ton tai thi xoa Po lien quan cu
                    if(input.poData.previousDraftID != null && input.poData.previousDraftID != "")
                    {
                        int poDataId = Convert.ToInt32(input.poData.previousDraftID);
                        bool deleteOldPo = DeletePO_PoProcess(poDataId);
                        if (!deleteOldPo)
                        {
                            throw new SapApiException("Không tìm thấy PO để xóa", ErrorCode.CANT_FIND_PO);

                        }
                    }
                    //het luong chay addPO
                    await transaction.CommitAsync();
                    return new SAPMessageReturn
                    {
                        errorCode = ErrorCode.ADD_PO_PO_PROCESS_SUCCESS,
                        errorDescription = "Thêm mới "+ poData.docType +" và quy trình thành công",
                        dataType = "SAPPO",
                        draftID = draftId,
                        appName = GlobalVariable.APP_NAME
                    };

                }
                else
                {
                    await transaction.RollbackAsync();
                    throw new SapApiException("Không thể khởi tạo vì không tìm thấy người thực hiện trên MyADG", ErrorCode.ADD_PO_PO_PROCESS_ERROR);
                }


            }
            catch (Exception ex)
            {
                //NLogManager.PublishException(ex);
                
                throw new SapApiException(ex.Message, ErrorCode.ADD_PO_PO_PROCESS_ERROR);
            }
            throw new SapApiException("Add khong thanh cong", ErrorCode.ADD_PO_PO_PROCESS_ERROR);
        }

        public bool DeletePO_PoProcess(int poDataId)
        {
            //check PoID ton tai
            var poData = _dbContext.TPoDatas.FirstOrDefault(p => p.PoDataId == poDataId);
            if(poData != null)
            {
                //xoa T Po Data
                _dbContext.TPoDatas.Remove(poData);
                //xoa T Ref File Po
                var refFilePosToDelete = _dbContext.TRefFilePos.Where(r => r.poDataId == poDataId).ToList();
                _dbContext.TRefFilePos.RemoveRange(refFilePosToDelete);
                //xoa T File
                if(refFilePosToDelete.Any())
                {
                    var fileIds = refFilePosToDelete.Select(r => r.fileId).ToList();
                    foreach (var fileId in fileIds)
                    {
                        var file = _dbContext.TFiles.FirstOrDefault(f => f.FileId == fileId);
                        if (file != null)
                        {
                            // FileUtils.DeleteFile(file.FilePath); // Giả sử có phương thức xóa file
                            _dbContext.TFiles.Remove(file);
                        }
                    }
                    
                }
                //xoa cac stage
                _dbContext.TPoFlowStages.RemoveRange(_dbContext.TPoFlowStages.Where(p => p.poDataId == poDataId));

                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public OutPutAPICreateUpdatePO_PoProcess GetPoProcess(InputAPIGetRoadMap input)
        {
            try
            {
                var poData = _dbContext.TPoDatas.FirstOrDefault(p => p.number == input.poNumber && input.poDate == p.date);
                if(poData != null)
                {
                    OutPutAPICreateUpdatePO_PoProcess returnObj = new OutPutAPICreateUpdatePO_PoProcess
                    {
                        poData = new PoData
                        {
                            //PoDataId = poData.PoDataId,
                            docType = poData.docType,
                            number = poData.number,
                            date = poData.date,
                            status = poData.status,
                            author = poData.author,
                            responsibility = poData.responsibility,
                            company = poData.company,
                            bu = poData.bu,
                            amount = poData.amount,
                            currency = poData.currency,
                            description = poData.description,
                            comment = poData.comment,
                            htmlPresentation = poData.htmlPresentation,
                            previousDraftID = poData.previousDraftID
                        },
                        

                    };

                    //Lấy danh sách file liên quan đến PO
                    List<TFile> files = new List<TFile>();
                    var fileRefs = _dbContext.TRefFilePos.Where(r => r.poDataId == poData.PoDataId).ToList();
                    if (fileRefs.Any())
                    {
                        foreach (var fileRef in fileRefs)
                        {
                            var file = _dbContext.TFiles.FirstOrDefault(f => f.FileId == fileRef.fileId);
                            if (file != null)
                            {
                                files.Add(file);
                            }
                        }
                    }
                    returnObj.poData.files = files;

                    //Lấy danh sách quy trình PO
                    List<POFlowStage> pOFlowStages = new List<POFlowStage>();
                    var poFlowStages = GetPoFlowStagesByPoDataId(poData.PoDataId);
                    if (poFlowStages.Any())
                    {
                        foreach (var stage in poFlowStages)
                        {
                            POFlowStage poFlowStage = new POFlowStage
                            {
                                step = stage.step,
                                performer = stage.performer,
                                deadlineDays = stage.deadlineDays,
                                description = stage.description,
                                startDate = stage.StartDate,
                                completionDate = stage.CompletionDate,
                                result = stage.result,
                                performComment = stage.performComment,
                                sapapprovelink = stage.sapapprovelink,
                                saprejectlink = stage.saprejectlink
                            };
                            pOFlowStages.Add(poFlowStage);
                        }
                        returnObj.poFlow = pOFlowStages;
                    }
                    return returnObj;
                }
                else
                {
                    throw new SapApiException("Không tìm thấy PO với số PONumber đã cung cấp", ErrorCode.CANT_FIND_PO);
                }
            }
            catch (Exception ex)
            {

                throw new SapApiException(ex.Message, ErrorCode.ADD_PO_PO_PROCESS_ERROR);
            }
            throw new NotImplementedException();
        }
    }
}
