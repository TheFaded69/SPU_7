using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using SPU_7.Common.Scripts;
using SPU_7.Common.Stand;
using SPU_7.Database.Models;
using SPU_7.Database.Repository;
using SPU_7.Models.Scripts;
using SPU_7.Models.Services.Logger;
using SPU_7.ViewModels.WorkReportViewModels;

namespace SPU_7.Models.Services.DbServices;

public class OperationResultsDbService : IOperationResultsDbService
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IRepositoryCreator<DbOperationResult, Guid> _repositoryCreator;

    public OperationResultsDbService(IMapper mapper, ILogger logger, IRepositoryCreator<DbOperationResult, Guid> repositoryCreator)
    {
        _mapper = mapper;
        _logger = logger;
        _repositoryCreator = repositoryCreator;
    }


    public List<OperationReportViewModel> GetOperationsById(Guid scriptId)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            var list = repository
                .Query
                .Where(d => !d.Deleted && d.ScriptResultId == scriptId)
                .Select(d => _mapper.Map<OperationReportViewModel>(d))
                .ToList();

            foreach (var operationReportViewModel in list)
            {
                var result = JsonConvert.DeserializeObject(operationReportViewModel.Result, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                if (result is not OperationResult operationResult) continue;

                operationReportViewModel.Message = operationResult.Message;
                operationReportViewModel.OperationResultType = operationResult.ResultStatus;
                operationReportViewModel.OperationNumber = operationResult.Result.OperationNumber;
                operationReportViewModel.OperationType = operationResult.OperationType;
                operationReportViewModel.BaseOperationResult = operationResult.Result;
            }

            return list;
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
            throw;
        }
    }

    public Task<List<OperationReportViewModel>> GetOperationsByIdAsync(Guid deviceId)
    {
        throw new NotImplementedException();
    }

    public List<Guid> GetScriptsIdByOperationId(List<Guid> operationsGuids)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            return operationsGuids
                .Select(guid => repository
                    .Query
                    .FirstOrDefault(op => op.Id == guid && !op.Deleted))
                .Select(op => op.ScriptResultId)
                .ToList();
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
            throw;
        }
    }
}