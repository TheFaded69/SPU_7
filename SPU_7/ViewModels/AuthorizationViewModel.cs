using System;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Models.Services.Autorization;

namespace SPU_7.ViewModels
{
    public class AuthorizationViewModel : ViewModelBase, IDialogAware
    {
        public AuthorizationViewModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;

            AuthorizeCommand = new DelegateCommand(AuthorizeCommandHandler);
            CancelCommand = new DelegateCommand(CancelCommandHandler);

            Title = "Авторизация пользователя";
        }
        private readonly IAuthorizationService _authorizationService;

        private string _userName;
        public string UserName
        {
            get => _userName; set
            {
                SetProperty(ref _userName, value);
                IsInvalidUser = false;
            }
        }

        private string _userPassword;

        public event Action<IDialogResult> RequestClose;

        public string UserPassword
        {
            get => _userPassword; set
            {
                SetProperty(ref _userPassword, value);
                IsInvalidUser = false;
            }
        }

        private bool _isInvalidUser;
        public bool IsInvalidUser
        {
            get => _isInvalidUser; set => SetProperty(ref _isInvalidUser, value);
        }

        private bool _isRememberUser;

        public bool IsRememberUser
        {
            get => _isRememberUser;
            set => SetProperty(ref _isRememberUser, value);
        }
        
        
        #region Dialog

        public DelegateCommand AuthorizeCommand { get; }
        public async void AuthorizeCommandHandler()
        {
            IsInvalidUser = !await _authorizationService.AuthorizeAsync(UserName, UserPassword);

            if (!IsInvalidUser)
            {
                await _authorizationService.UpdateAutoUser(IsRememberUser);
                RequestClose.Invoke(new DialogResult(ButtonResult.OK));
            }
        }

        public DelegateCommand CancelCommand { get; }
        public void CancelCommandHandler() 
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.Cancel));
        }


        public bool CanCloseDialog() => true;
        public void OnDialogClosed()
        {
            
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }

        #endregion
    }
}
