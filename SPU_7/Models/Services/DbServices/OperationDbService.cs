using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SPU_7.Common.Scripts;
using SPU_7.Database.Models;
using SPU_7.Database.Repository;
using SPU_7.Models.Scripts.Operations.Configurations;
using SPU_7.Models.Services.Logger;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels;

namespace SPU_7.Models.Services.DbServices;

public class OperationDbService : IOperationDbService
{
    public OperationDbService(IMapper mapper, ILogger logger, IRepositoryCreator<DbOperations, Guid> repositoryCreator)
    {
        _mapper = mapper;
        _logger = logger;
        _repositoryCreator = repositoryCreator;
    }
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IRepositoryCreator<DbOperations, Guid> _repositoryCreator;


    public void AddOperation(OperationViewModel operation)
    {
        try
        {
            var dbOperation = _mapper.Map<DbOperations>(operation);
            dbOperation.Configuration = JsonConvert.SerializeObject(operation.ConfigurationModel, 
                Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            var repository = _repositoryCreator.CreateRepository();
            repository.Insert(dbOperation);
            repository.Commit();
        }
        catch (Exception e)
        {
            throw;
        }
    }
    
    public async Task AddOperationAsync(OperationViewModel operation)
    {
        try
        {
            var dbOperation = _mapper.Map<DbOperations>(operation);
            dbOperation.Configuration = JsonConvert.SerializeObject(operation.ConfigurationModel, 
                Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            var repository = await _repositoryCreator.CreateRepositoryAsync();
            repository.Insert(dbOperation);
            await repository.CommitAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public void UpdateOperation(OperationViewModel operation)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            var existDbOperation = repository
                .Get(operation.Id);

            if (existDbOperation == null) return;

            existDbOperation = _mapper.Map<DbOperations>(operation);
            existDbOperation.Configuration = JsonConvert.SerializeObject(operation.ConfigurationModel, 
                Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            repository.Update(existDbOperation);
            repository.Commit();
        }
        catch (Exception e)
        {
            throw;
        }
    }
    
    public async Task UpdateOperationAsync(OperationViewModel operation)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();

            var existDbOperation = repository
                .Get(operation.Id);

            if (existDbOperation == null) return;
        
            existDbOperation.Configuration = JsonConvert.SerializeObject(operation.ConfigurationModel, 
                Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            existDbOperation = _mapper.Map<DbOperations>(operation);
            repository.Update(existDbOperation);
            await repository.CommitAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public List<OperationViewModel> GetOperations(Guid scriptId)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();
            var dbOperations = repository
                .Query
                .Where(op => !op.Deleted && op.ScriptId == scriptId)
                .OrderBy(op => op.Number)
                .ToList();

            return dbOperations.Select(dbOperation => new OperationViewModel
                {
                    ScriptId = dbOperation.ScriptId,
                    Id = dbOperation.Id,
                    Description = dbOperation.Description,
                    Name = dbOperation.Name,
                    OperationType = dbOperation.OperationType,
                    ConfigurationModel = JsonConvert.DeserializeObject<BaseOperationConfigurationModel>(dbOperation.Configuration,
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }),
                    Number = dbOperation.Number
                })
                .ToList();

        }
        catch (Exception e)
        {
            throw;
        }
    }
    
    public async Task<List<OperationViewModel>> GetOperationsAsync(Guid scriptId)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();
            var dbOperations = await repository
                .Query
                .Where(op => !op.Deleted && op.ScriptId == scriptId)
                .OrderBy(op => op.Number)
                .ToListAsync();

            return dbOperations
                .Select(dbOperation => new OperationViewModel
                {
                    ScriptId = dbOperation.ScriptId,
                    Id = dbOperation.Id,
                    Description = dbOperation.Description,
                    Name = dbOperation.Name,
                    OperationType = dbOperation.OperationType,
                    ConfigurationModel = JsonConvert.DeserializeObject<BaseOperationConfigurationModel>(dbOperation.Configuration, 
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto, 
                            NullValueHandling = NullValueHandling.Ignore,
                        }),
                    Number = dbOperation.Number
                })
                .ToList();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public void DeleteOperation(Guid id)
    {
        var repository = _repositoryCreator.CreateRepository();
        var dbOperations = repository.Get(id);
        if (dbOperations == null) return;
        repository.Delete(dbOperations);
        repository.Commit();
    }

    public async Task DeleteOperationAsync(Guid id)
    {
        var repository = await _repositoryCreator.CreateRepositoryAsync();
        var dbOperations = await repository.GetAsync(id);
        if (dbOperations == null) return;
        repository.Delete(dbOperations);
        await repository.CommitAsync();
    }
}