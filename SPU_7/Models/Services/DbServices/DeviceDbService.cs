using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPU_7.Common.Stand;
using SPU_7.Database.Models;
using SPU_7.Database.Repository;
using SPU_7.Models.Services.Logger;
using SPU_7.ViewModels.WorkReportViewModels;

namespace SPU_7.Models.Services.DbServices;

public class DeviceDbService : IDeviceDbService
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IRepositoryCreator<DbDevice, Guid> _repositoryCreator;

    public DeviceDbService(IMapper mapper, ILogger logger, IRepositoryCreator<DbDevice, Guid> repositoryCreator)
    {
        _mapper = mapper;
        _logger = logger;
        _repositoryCreator = repositoryCreator;
    }

    public List<DeviceReportViewModel> GetDeviceReports(DateTimeOffset startTimeOffset, DateTimeOffset endTimeOffset, string subVendorNumber)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            return repository
                .Query
                .Where(d => d.CreateTime.Date >= startTimeOffset.DateTime.Date && d.CreateTime.Date <= endTimeOffset.DateTime.Date && d.VendorNumber.Contains(subVendorNumber) &&
                            !d.Deleted)
                .Select(d => _mapper.Map<DeviceReportViewModel>(d))
                .ToList();
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
            throw;
        }
    }

    public async Task<List<DeviceReportViewModel>> GetDeviceReportsAsync(DateTimeOffset startTimeOffset, DateTimeOffset endTimeOffset, string subVendorNumber)
    {
        try
        {
            var repository = await _repositoryCreator.CreateRepositoryAsync();

            return await repository
                .Query
                .Where(d => d.CreateTime > startTimeOffset.DateTime && d.CreateTime < endTimeOffset.DateTime && d.VendorNumber.Contains(subVendorNumber) && !d.Deleted)
                .Select(d => _mapper.Map<DeviceReportViewModel>(d))
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
            throw;
        }
    }

    public List<DeviceReportViewModel> GetDeviceReports(Guid scriptId)
    {
        try
        {
            var repository = _repositoryCreator.CreateRepository();

            return repository
                .Query
                .Where(d => d.OperationResultId == scriptId && !d.Deleted)
                .Select(d => _mapper.Map<DeviceReportViewModel>(d))
                .ToList();
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
            throw;
        }
    }

    public Task<List<DeviceReportViewModel>> GetDeviceReportsAsync(Guid scriptId)
    {
        throw new NotImplementedException();
    }

    public List<Guid> GetOperationsIdByParameter(string vendorNumber, string vendorName, string vendorAddress, DateTimeOffset startTimeOffset, DateTimeOffset endTimeOffset)
    {
        var repository = _repositoryCreator.CreateRepository();

        return repository
            .Query
            .Where(d => d.CreateTime.Date >= startTimeOffset.DateTime.Date 
                        && d.CreateTime.Date <= endTimeOffset.DateTime.Date 
                        && !d.Deleted 
                        && (string.IsNullOrEmpty(vendorAddress) ? d.VendorAddress.Contains("") : d.VendorAddress.Contains(vendorAddress))
                        && (string.IsNullOrEmpty(vendorName) ? d.VendorName.Contains("") : d.VendorName.Contains(vendorName))
                        && (string.IsNullOrEmpty(vendorNumber) ? d.VendorNumber.Contains("") : d.VendorNumber.Contains(vendorNumber)))
            .Select(d => d.OperationResultId)
            .ToList();
    }
}