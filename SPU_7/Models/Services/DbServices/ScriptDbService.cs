using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPU_7.Common.Device;
using SPU_7.Common.Stand;
using SPU_7.Database.Models;
using SPU_7.Database.Repository;
using SPU_7.Models.Services.Logger;
using SPU_7.ViewModels.ScriptViewModels;

namespace SPU_7.Models.Services.DbServices;

public class ScriptDbService : IScriptDbService
{
    public ScriptDbService(IMapper mapper, ILogger logger, IRepositoryCreator<DbScripts, Guid> repositoryCreator)
    {
        _mapper = mapper;
        _logger = logger;
        _repositoryCreator = repositoryCreator;
    }
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IRepositoryCreator<DbScripts, Guid> _repositoryCreator;
    
    public List<ScriptViewModel> GetAllScripts()
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            var data = repository
                .Query
                .Where(db => !db.Deleted)
                .OrderBy(db => db.CreateTime);

            var scripts = data.Select(dbScript => _mapper.Map<ScriptViewModel>(dbScript)).ToList();
            return scripts;
        }
        catch (Exception e)
        {
            throw;
        }
    }
    public async Task<List<ScriptViewModel>> GetAllScriptsAsync()
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();

            var data = repository
                .Query
                .Where(db => !db.Deleted)
                .OrderBy(db => db.CreateTime);

            return data.Select(dbScript => _mapper.Map<ScriptViewModel>(dbScript)).ToList();

        }
        catch (Exception e)
        {
            throw;
        }
    }

    public List<ScriptViewModel> GetFilterScripts(StandType? standType, DeviceType? deviceType)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            var data = repository
                .Query
                .Where(db => !db.Deleted)
                .OrderBy(db => db.CreateTime);

            if (standType != null)
                data = data
                    .Where(db => db.TargetStandType == standType)
                    .OrderBy(db => db.CreateTime);
            
            if (deviceType != null)
                data = data
                    .Where(db => db.DeviceType == deviceType)
                    .OrderBy(db => db.CreateTime);

            return data.Select(dbScript => _mapper.Map<ScriptViewModel>(dbScript)).ToList();
        }
        catch (Exception e)
        {
            throw;
        }
    }
    public  async Task<List<ScriptViewModel>> GetFilterScriptsAsync(StandType? standType, DeviceType? deviceType)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();

            var data = repository
                .Query
                .Where(db => !db.Deleted);

            if (standType != null)
                data = data
                    .Where(db => db.TargetStandType == standType)
                    .OrderBy(db => db.CreateTime);
            
            if (deviceType != null)
                data = data
                    .Where(db => db.DeviceType == deviceType)
                    .OrderBy(db => db.CreateTime);

            return await data.Select(dbScript => _mapper.Map<ScriptViewModel>(dbScript)).ToListAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public bool AddScript(ScriptViewModel script)
    {
        try
        {
            var dbScript = _mapper.Map<DbScripts>(script);
            var repository = _repositoryCreator.CreateRepository();
            repository.Insert(dbScript);
            repository.Commit();
            
            return true;
        }
        catch (Exception e)
        {
            throw;
        }
    }
    public async Task<bool> AddScriptAsync(ScriptViewModel script)
    {
        try
        {
            var dbScript = _mapper.Map<DbScripts>(script);
            var repository = await _repositoryCreator.CreateRepositoryAsync();
            repository.Insert(dbScript);
            await repository.CommitAsync();
            
            return true;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public bool EditScript(ScriptViewModel script)
    {
        
            var repository = _repositoryCreator.CreateRepository();

            var existScript = repository
                .Query
                .FirstOrDefault(db => !db.Deleted && db.Id == script.Id);

            if (existScript == null) return false;
            try
            {
                existScript = _mapper.Map<DbScripts>(script);
                repository.Update(existScript);
                repository.Commit();
                return true;
            }
            catch (Exception e)
            {
                
                throw;
            }

    }
    public async Task<bool> EditScriptAsync(ScriptViewModel script)
    {
        var repository = await _repositoryCreator.CreateRepositoryAsync();

        var existScript = await repository
            .Query
            .FirstOrDefaultAsync(db => !db.Deleted && db.Id == script.Id);

        if (existScript == null) return false;
        try
        {
            existScript = _mapper.Map<DbScripts>(script);
            repository.Update(existScript);
            await repository.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
                
            throw;
        }
    }

    public bool DeleteScript(Guid id)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            var existScript = repository
                .Query
                .FirstOrDefault(db => !db.Deleted && db.Id == id);
            
            repository.Delete(existScript);
            repository.Commit();
            return true;

        }
        catch (Exception e)
        {
            throw;
        }
    }
    public async Task<bool> DeleteScriptAsync(Guid id)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();

            var existScript = await repository
                .Query
                .FirstOrDefaultAsync(db => !db.Deleted && db.Id == id);
            
            repository.Delete(existScript);
            await repository.CommitAsync();
            return true;

        }
        catch (Exception e)
        {
            throw;
        }
    }
}