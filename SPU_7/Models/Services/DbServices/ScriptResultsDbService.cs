using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SPU_7.Common.Stand;
using SPU_7.Database.Models;
using SPU_7.Database.Repository;
using SPU_7.Models.Scripts;
using SPU_7.Models.Services.Logger;
using SPU_7.ViewModels.WorkReportViewModels;

namespace SPU_7.Models.Services.DbServices;

public class ScriptResultsDbService : IScriptResultsDbService
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IRepositoryCreator<DbScriptResult, Guid> _repositoryCreator;

    public ScriptResultsDbService(IMapper mapper, ILogger logger, IRepositoryCreator<DbScriptResult, Guid> repositoryCreator)
    {
        _mapper = mapper;
        _logger = logger;
        _repositoryCreator = repositoryCreator;
    }

    public void AddScriptResult(ScriptResult scriptResult)
    {
        try
        {
            var dbScriptResults = _mapper.Map<DbScriptResult>(scriptResult);
            
            var repository = _repositoryCreator.CreateRepository();
            repository.Insert(dbScriptResults);
            repository.Commit();
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Error));

            throw;
        }
    }

    public Task AddScriptResultAsync(ScriptResult scriptResult)
    {
        throw new NotImplementedException();
    }

    public ScriptResult GetScriptResult(Guid scriptId)
    {
        throw new NotImplementedException();
    }

    public Task<ScriptResult> GetScriptResultAsync(Guid scriptId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ScriptReportViewModel>> GetScriptResultsAsync(DateTimeOffset startDateOffsetScript, DateTimeOffset endDateOffsetScript)
    {
        throw new NotImplementedException();
    }

    public List<ScriptReportViewModel> GetScriptResults(DateTimeOffset startDateOffsetScript, DateTimeOffset endDateOffsetScript)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            return repository
                .Query
                .Where(d => d.CreateTime.Date >= startDateOffsetScript.DateTime.Date && d.CreateTime.Date <= endDateOffsetScript.DateTime.Date)
                .Select(d => _mapper.Map<ScriptReportViewModel>(d))
                .ToList();
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
            throw;
        }
    }

    public List<ScriptReportViewModel> GetScriptResultsByGuidList(List<Guid> guids)
    {
        var repository = _repositoryCreator.CreateRepository();

        return guids.Select(guid => repository
            .Query
            .FirstOrDefault(d => !d.Deleted && d.Id == guid))
            .Select(a => _mapper.Map<ScriptReportViewModel>(a))
            .ToList();
    }
}