using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Extensions;
using SPU_7.Common.Stand;
using SPU_7.Models.Services.Autorization;
using SPU_7.Models.Services.DbServices;

namespace SPU_7.ViewModels;

public class AddUserViewModel : ViewModelBase, IDialogAware
{
    public AddUserViewModel(IUsersDbService usersDbService, IAuthorizationService authorizationService)
    {
        _usersDbService = usersDbService;
        
        AddUserCommand = new DelegateCommand(AddUserCommandHandler);
        CancelCommand = new DelegateCommand(CancelCommandHandler);

        StringUserTypes = new ObservableCollection<string>(Enum
            .GetValues<UserType>()
            .Where(ut => ut != UserType.None)
            .Select(ut => ut.GetDescription()));

        if (authorizationService.GetUser().UserType != UserType.Developer)
            StringUserTypes.Remove(StringUserTypes.FirstOrDefault(st => st == Enum
                .GetValues<UserType>()
                .FirstOrDefault(ut => ut == UserType.Developer)
                .GetDescription()));
        
        Title = "Новый пользователь";
    }
    private readonly IUsersDbService _usersDbService;

    private string _userName;
    private string _employee;
    private string _userPassword;
    private string _selectedStringUserType;

    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }

    public string Employee
    {
        get => _employee;
        set => SetProperty(ref _employee, value);
    }

    public string UserPassword
    {
        get => _userPassword;
        set => SetProperty(ref _userPassword, value);
    }

    public ObservableCollection<string> StringUserTypes { get; set; }

    public string SelectedStringUserType
    {
        get => _selectedStringUserType;
        set
        {
            SetProperty(ref _selectedStringUserType, value);
            UserType = Enum
                .GetValues<UserType>()
                .FirstOrDefault(ut => ut.GetDescription() == SelectedStringUserType);
        }
    }
    public UserType UserType { get; set; }

    public DelegateCommand AddUserCommand { get; }
    private void AddUserCommandHandler()
    {
        _usersDbService.AddUser(new User()
        {
            UserType = UserType,
            Employee = Employee,
            UserName = UserName,
            Password = UserPassword,
            ProgramType = ProgramType.UniversalStand
        });
        
        RequestClose.Invoke(new DialogResult(ButtonResult.OK));
    }
    
    public DelegateCommand CancelCommand { get; }
    private void CancelCommandHandler()
    {
        RequestClose.Invoke(new DialogResult(ButtonResult.Cancel));
    }
    
    #region Dialog

    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        
    }

    public event Action<IDialogResult> RequestClose;

    public static void Show(IDialogService dialogService, Action positiveAction, Action negativeAction)
    {
        
    }
    
    #endregion
}