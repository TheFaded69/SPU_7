using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SPU_7.Models.Scripts.Operations.Results;

namespace SPU_7.Models.Services.DbServices;

public interface IPictureResultDbService
{
    PictureResultModel GetPictureResult(Guid id);
    Task<PictureResultModel> GetPictureResultAsync(Guid id);
    List<PictureResultModel> GetPictureResults(List<Guid> guids);
    Task<List<PictureResultModel>> GetPictureResultsAsync(List<Guid> guids);
    void AddPictureResult(PictureResultModel pictureResultModel);
    void AddPictureResultAsync(PictureResultModel pictureResultModel);
    void AddPictureResults(List<PictureResultModel> pictureResultModels);
    void AddPictureResultsAsync(List<PictureResultModel> pictureResultModels);
}