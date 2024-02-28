using System;

namespace SPU_7.ViewModels.WorkReportViewModels;

public class ScriptReportViewModel : ViewModelBase
{
    public ScriptReportViewModel()
    {
        
    }
    
    private string _name;
    private string _description;
    private DateTime _createdTime;

    public Guid Id { get; set; }
    
    public DateTime CreateTime
    {
        get => _createdTime;
        set => SetProperty(ref _createdTime, value);
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
}