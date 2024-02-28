using System.IO.Ports;
using SPU_7.Common.SerialPortSettings;

namespace SPU_7.Models.Stand.Settings.Connection
{
    public class SPIConnetionSetting
    {
        public string PortName { get; set; }

        public SerialPortBaudRate SerialPortBaudRate { get; set; }

        public string DataBits { get; set; }

        public Parity Parity { get; set; }

        public StopBits StopBits { get; set; }

        public bool ThreadControl { get; set; }

        public string ID { get; set; }

        public int RequestCount { get; set; }
    }
}
