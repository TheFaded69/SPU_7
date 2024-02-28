using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using Prism.Services.Dialogs;
using SPU_7.Common.Extensions;
using SPU_7.Common.SerialPortSettings;

namespace SPU_7.ViewModels
{
    public class ConnectionSettingsViewModel : ViewModelBase, IDialogAware
    {
        public ConnectionSettingsViewModel()
        {


            SerialPorts = new ObservableCollection<string>(SerialPort.GetPortNames());
            SerialPortBaudRates = new ObservableCollection<KeyValuePair<SerialPortBaudRate, string>>();
            foreach (var baudRate in Enum.GetValues<SerialPortBaudRate>())
            {
                SerialPortBaudRates.Add(new KeyValuePair<SerialPortBaudRate, string>(baudRate, baudRate.GetDescription()));
            }
        }

        /// <summary>
        /// Список последовательных портов, присутствующих в системе
        /// </summary>
        public ObservableCollection<string> SerialPorts { get; set; }

        /// <summary>
        /// Список скоростей порта
        /// </summary>
        public ObservableCollection<KeyValuePair<SerialPortBaudRate, string>> SerialPortBaudRates { get; set; }


        #region Dialog

        public event Action<IDialogResult> RequestClose;

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
