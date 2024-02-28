using System;
using System.ComponentModel;

namespace SPU_7.ViewModels.WorkReportViewModels;

public class DeviceReportViewModel : ViewModelBase
{
    public DeviceReportViewModel()
    {
        
    }
    
    private string _vendorNumber;
    private DateTime _createTime;
    private Guid _id;
    private string _deviceName;
    private string _vendorName;
    private string _vendorAddress;


    public Guid Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    [Description("Заводской номер")]
    public string VendorNumber
    {
        get => _vendorNumber;
        set => SetProperty(ref _vendorNumber, value);
    }
    
    [Description("Дата проверки")]
    public DateTime CreateTime
    {
        get => _createTime;
        set => SetProperty(ref _createTime, value);
    }

    public string DeviceName
    {
        get => _deviceName;
        set => SetProperty(ref _deviceName, value);
    }

    public string VendorName
    {
        get => _vendorName;
        set => SetProperty(ref _vendorName, value);
    }

    public string VendorAddress
    {
        get => _vendorAddress;
        set => SetProperty(ref _vendorAddress, value);
    }
}