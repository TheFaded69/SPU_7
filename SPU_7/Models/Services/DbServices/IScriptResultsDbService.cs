using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SPU_7.Models.Scripts;
using SPU_7.ViewModels.WorkReportViewModels;

namespace SPU_7.Models.Services.DbServices;

public interface IScriptResultsDbService
{
    void AddScriptResult(ScriptResult scriptResult);

    Task AddScriptResultAsync(ScriptResult scriptResult);

    ScriptResult GetScriptResult(Guid scriptId);
    
    Task<ScriptResult> GetScriptResultAsync(Guid scriptId);
    
    Task<List<ScriptReportViewModel>> GetScriptResultsAsync(DateTimeOffset startDateOffsetScript, DateTimeOffset endDateOffsetScript);
    
    List<ScriptReportViewModel> GetScriptResults(DateTimeOffset startDateOffsetScript, DateTimeOffset endDateOffsetScript);

    List<ScriptReportViewModel> GetScriptResultsByGuidList(List<Guid> guids);
    
}