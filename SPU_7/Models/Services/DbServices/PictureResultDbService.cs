using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPU_7.Database.Models;
using SPU_7.Database.Repository;
using SPU_7.Models.Scripts.Operations.Results;
using SPU_7.Models.Services.Logger;

namespace SPU_7.Models.Services.DbServices;

public class PictureResultDbService : IPictureResultDbService
{
    public PictureResultDbService(IMapper mapper, ILogger logger,
        IRepositoryCreator<DbPictureResult, Guid> repositoryCreator)
    {
        _mapper = mapper;
        _logger = logger;
        _repositoryCreator = repositoryCreator;
    }

    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IRepositoryCreator<DbPictureResult, Guid> _repositoryCreator;

    public PictureResultModel GetPictureResult(Guid id)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();
            return _mapper.Map<PictureResultModel>(repository
                .Query
                .FirstOrDefault(pic => pic.Id == id));
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<PictureResultModel> GetPictureResultAsync(Guid id)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();

            return _mapper.Map<PictureResultModel>(await repository
                .Query
                .FirstOrDefaultAsync(pic => pic.Id == id));
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public List<PictureResultModel> GetPictureResults(List<Guid> guids)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            return guids
                .Select(guid => _mapper.Map<PictureResultModel>(repository
                    .Query
                    .FirstOrDefault(pic => pic.Id == guid)))
                .ToList();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<List<PictureResultModel>> GetPictureResultsAsync(List<Guid> guids)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();

            return guids
                .Select(guid => _mapper.Map<PictureResultModel>(repository
                    .Query
                    .FirstOrDefault(pic => pic.Id == guid)))
                .ToList();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public void AddPictureResult(PictureResultModel pictureResultModel)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();
            repository.Insert(_mapper.Map<DbPictureResult>(pictureResultModel));
            repository.Commit();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async void AddPictureResultAsync(PictureResultModel pictureResultModel)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();
            repository.Insert(_mapper.Map<DbPictureResult>(pictureResultModel));
            await repository.CommitAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public void AddPictureResults(List<PictureResultModel> pictureResultModels)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();
            pictureResultModels.ForEach(pic => repository.Insert(_mapper.Map<DbPictureResult>(pic)));
            repository.Commit();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async void AddPictureResultsAsync(List<PictureResultModel> pictureResultModels)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();
            pictureResultModels.ForEach(pic => repository.Insert(_mapper.Map<DbPictureResult>(pic)));
            await repository.CommitAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }
}