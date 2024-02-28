using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SPU_7.ViewModels.WorkReportViewModels;

namespace SPU_7.Models.Services.DbServices;

public interface IDeviceDbService
{
    List<DeviceReportViewModel> GetDeviceReports(DateTimeOffset startTimeOffset, DateTimeOffset endTimeOffset, string subVendorNumber);
    Task<List<DeviceReportViewModel>> GetDeviceReportsAsync(DateTimeOffset startTimeOffset, DateTimeOffset endTimeOffset, string subVendorNumber);
    
    List<DeviceReportViewModel> GetDeviceReports(Guid scriptId);
    Task<List<DeviceReportViewModel>> GetDeviceReportsAsync(Guid scriptId);

    List<Guid> GetOperationsIdByParameter(string vendorNumber, string vendorName, string vendorAddress, DateTimeOffset startTimeOffset, DateTimeOffset endTimeOffset);
}