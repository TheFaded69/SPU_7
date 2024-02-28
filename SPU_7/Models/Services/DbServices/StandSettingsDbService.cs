using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SPU_7.Database.Models;
using SPU_7.Database.Repository;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Stand.Settings.Stand;

namespace SPU_7.Models.Services.DbServices;

public class StandSettingsDbService : IStandSettingsDbService
{
    
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IRepositoryCreator<DbStandSettings, Guid> _repositoryCreator;

    public StandSettingsDbService(IMapper mapper, ILogger logger, IRepositoryCreator<DbStandSettings, Guid> repositoryCreator)
    {
        _mapper = mapper;
        _logger = logger;
        _repositoryCreator = repositoryCreator;
    }
    
    public List<StandSettingsProfile> GetSettingsProfiles()
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();
            
            return repository
                .Query
                .Where(db => !db.Deleted)
                .Select(db => _mapper.Map<StandSettingsProfile>(db))
                .ToList();
        }
        catch (Exception e)
        {
            
            throw;
        }
    }

    public async Task<List<StandSettingsProfile>> GetSettingsProfilesAsync()
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();
            
            return await repository
                .Query
                .Where(db => !db.Deleted)
                .Select(db => _mapper.Map<StandSettingsProfile>(db))
                .ToListAsync();
        }
        catch (Exception e)
        {
            
            throw;
        }
    }

    public StandSettingsModel GetStandSettings(Guid id)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            var data = repository
                .Query
                .Where(db => !db.Deleted)
                .FirstOrDefault(db => db.Id == id);

            return JsonConvert.DeserializeObject<StandSettingsModel>(data.StandSettings);

        }
        catch (Exception e)
        {
            
            throw;
        }
    }

    public async Task<StandSettingsModel> GetStandSettingsAsync(Guid id)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();

            var data = await repository
                .Query
                .Where(db => !db.Deleted)
                .FirstOrDefaultAsync(db => db.Id == id);

            return JsonConvert.DeserializeObject<StandSettingsModel>(data.StandSettings);
        }
        catch (Exception e)
        {
            
            throw;
        }
    }

    public void AddStandSettings(StandSettingsModel standSettingsModel)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();
            var dbStandSettingsModel = _mapper.Map<DbStandSettings>(standSettingsModel);
            repository.Insert(dbStandSettingsModel);
            repository.Commit();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task AddStandSettingsAsync(StandSettingsModel standSettingsModel)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();
            var dbStandSettingsModel = _mapper.Map<DbStandSettings>(standSettingsModel);
            repository.Insert(dbStandSettingsModel);
            await repository.CommitAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public void EditStandSettings(StandSettingsModel standSettingsModel)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            var existSettings = repository
                .Query
                .FirstOrDefault(db => !db.Deleted && db.Id == standSettingsModel.Id);

            if (existSettings == null) return;

            existSettings.StandSettings = JsonConvert.SerializeObject(standSettingsModel);
            existSettings.ProfileName = standSettingsModel.ProfileName;
            if (standSettingsModel.StandNumber != null) existSettings.StandNumber = (int)standSettingsModel.StandNumber;
            
            repository.Update(existSettings);
            repository.Commit();
        }
        catch (Exception e)
        {
            
            throw;
        }
    }

    public async Task EditStandSettingsAsync(StandSettingsModel standSettingsModel)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();

            var existSettings = await repository
                .Query
                .FirstOrDefaultAsync(db => !db.Deleted && db.Id == standSettingsModel.Id);

            if (existSettings == null) return;

            existSettings.StandSettings = JsonConvert.SerializeObject(standSettingsModel);
            existSettings.ProfileName = standSettingsModel.ProfileName;
            if (standSettingsModel.StandNumber != null) existSettings.StandNumber = (int)standSettingsModel.StandNumber;
            
            repository.Update(existSettings);
            await repository.CommitAsync();
        }
        catch (Exception e)
        {
            
            throw;
        }
    }

    public void DeleteStandSettings(Guid id)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            var existSettings =  repository
                .Query
                .FirstOrDefault(db => !db.Deleted && db.Id == id);
            
            repository.Delete(existSettings);
            repository.CommitAsync();
        }
        catch (Exception e)
        {
            
            throw;
        }
    }

    public async Task DeleteStandSettingsAsync(Guid id)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();

            var existSettings = await repository
                .Query
                .FirstOrDefaultAsync(db => !db.Deleted && db.Id == id);
            
            repository.Delete(existSettings);
            await repository.CommitAsync();
        }
        catch (Exception e)
        {
            
            throw;
        }
    }
}