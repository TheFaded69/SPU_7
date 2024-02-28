using System;
using Prism.Commands;

namespace SPU_7.ViewModels
{
    public class MenuItemViewModel : ViewModelBase
    {
        private bool _isVisible;

        public MenuItemViewModel(string name,  Action action)
        {
            Name = name;

            MenuActionCommand = new DelegateCommand(action);

        }
        
        public string Name { get; }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public DelegateCommand MenuActionCommand { get; }
    }
}
