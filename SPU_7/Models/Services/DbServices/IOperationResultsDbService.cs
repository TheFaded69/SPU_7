using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SPU_7.ViewModels.WorkReportViewModels;

namespace SPU_7.Models.Services.DbServices;

public interface IOperationResultsDbService
{
    List<OperationReportViewModel> GetOperationsById(Guid deviceId);
    
    Task<List<OperationReportViewModel>> GetOperationsByIdAsync(Guid deviceId);

    List<Guid> GetScriptsIdByOperationId(List<Guid> operationsGuids);

}