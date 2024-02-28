using System.Collections.ObjectModel;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels
{
    public class DeviceViewModel : ViewModelBase
    {
        private string _deviceTypeString;
        public string DeviceTypeString
        {
            get => _deviceTypeString; set => SetProperty(ref _deviceTypeString, value);
        }

        private string _vendorNumberString;
        private ObservableCollection<string> _parameters;
        private bool _isEnabled;

        public string VendorNumberString
        {
            get => _vendorNumberString; set => SetProperty(ref _vendorNumberString, value);
        }

        public ObservableCollection<string> Parameters
        {
            get => _parameters;
            set => SetProperty(ref _parameters, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
    }
}
