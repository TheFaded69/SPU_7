using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using SPU_7.Database.Models;
using SPU_7.Database.Repository;
using SPU_7.Models.Services.Logger;
using SPU_7.ViewModels;

namespace SPU_7.Models.Services.DbServices;

public class DeviceNameDbService : IDeviceNameDbService
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IRepositoryCreator<DbDeviceInfo, Guid> _repositoryCreator;

    public DeviceNameDbService(IMapper mapper, ILogger logger, IRepositoryCreator<DbDeviceInfo, Guid> repositoryCreator)
    {
        _mapper = mapper;
        _logger = logger;
        _repositoryCreator = repositoryCreator;
    }


    public List<DeviceNameViewModel> GetDeviceNames()
    {
        var repository = _repositoryCreator.CreateRepository();

        var names = repository
            .Query
            .Where(db => !db.Deleted)
            .Select(db => db.DeviceTypeInformation);

        var deviceNameViewModels = new List<DeviceNameViewModel>();

        foreach (var name in names)
        {
            deviceNameViewModels.Add(new DeviceNameViewModel(){DeviceTypeInfo = name});
        }

        return deviceNameViewModels;
    }

    public void UpdateDeviceNames(ObservableCollection<DeviceNameViewModel> names)
    {
        var repository = _repositoryCreator.CreateRepository();
        var existNames = repository
            .Query
            .Where(db => !db.Deleted)
            .Select(db => db.DeviceTypeInformation)
            .ToList();

        foreach (var existName in existNames)
        {
            if (names.Select(name => name.DeviceTypeInfo).Contains(existName)) continue;
            
            DeleteDeviceName(new DeviceNameViewModel(){DeviceTypeInfo = existName});
        }
        
        foreach (var name in names)
        {
            if (existNames.Contains(name.DeviceTypeInfo)) continue;
            
            AddDeviceName(name);
        }
    }

    public void AddDeviceName(DeviceNameViewModel name)
    {
        var repository = _repositoryCreator.CreateRepository();

        repository.Insert(new DbDeviceInfo() { DeviceTypeInformation = name.DeviceTypeInfo });
        repository.Commit();
    }

    public void DeleteDeviceName(DeviceNameViewModel name)
    {
        var repository = _repositoryCreator.CreateRepository();

        var exist = repository
            .Query
            .FirstOrDefault(db => db.DeviceTypeInformation == name.DeviceTypeInfo && !db.Deleted);

        if (exist == null) return;

        repository.Delete(exist);
        repository.Commit();
    }
}