using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UDMCalibrationStand.DeviceTaskSheduler
{
    #region ----- Описание -----
    /// <summary>
    /// Делегат исполняемого метода обработки ответа утройства
    /// </summary>
    internal delegate void AnswerHandlerMethod(byte[] Data, Guid ID);

    /// <summary>
    /// Пропуск опроса устройств
    /// </summary>
    public enum E_SkippDevice
    {
        /// <summary>
        /// Пропуск опроса устройств - Включен
        /// </summary>
        ON,
        /// <summary>
        /// Пропуск опроса устройств - Выключен
        /// </summary>
        OFF
    }

    /// <summary>
    /// Поддерживаемые интерфейсы обмена с устройсвами
    /// </summary>
    public enum DeviceInterface
    {
        /// <summary>
        /// Интерфейс RS-232
        /// </summary>
        RS_232,
        /// <summary>
        /// Интерфейс RS-485
        /// </summary>
        RS_485
    }

    internal class C_LOG
    {
        /// <summary>
        /// Дата / время
        /// </summary>
        internal DateTime DT { get; set; }
        /// <summary>
        /// СОМ порт
        /// </summary>
        internal string COMPort { get; set; }
        /// <summary>
        /// Адрес устройства на шине
        /// </summary>
        internal byte? DeviceAddress { get; set; }
        /// <summary>
        /// Запрос
        /// </summary>
        internal byte[] RequestData { get; set; }
        /// <summary>
        /// Ответ
        /// </summary>
        internal byte[] AnswerData { get; set; }

        internal C_LOG(DateTime DT, string COMPort, byte? DeviceAddress, byte[] RequestData, byte[] AnswerData)
        {
            this.DT = DT;
            this.COMPort = COMPort;
            this.DeviceAddress = DeviceAddress;
            this.RequestData = RequestData;
            this.AnswerData = AnswerData;
        }
    }

    /// <summary>
    /// Класс элемента очереди запроса - ответа
    /// </summary>
    internal class C_RequestAnswerItem
    {
        /// <summary>
        /// Байтовый запрос
        /// </summary>
        internal byte[] RequestData { get; set; }
        /// <summary>
        /// Метод обработки ответа
        /// </summary>
        internal AnswerHandlerMethod AnswerMethod { get; set; }
        /// <summary>
        /// Таймаут ответа
        /// </summary>
        internal ushort TimeOut { get; set; }
        /// <summary>
        /// Дополнительный таймаут ответа
        /// </summary>
        internal ushort AddTimeOut { get; set; }
        /// <summary>
        /// ИД команды
        /// </summary>
        internal Guid ID { get; set; }

        /// <summary>
        /// Элемент очереди запроса - ответа
        /// </summary>
        /// <param name="RequestData">Байтовый запрос</param>
        /// <param name="AnswerMethod">Метод обработки ответа</param>
        /// <param name="TimeOut">Таймаут</param>
        /// <param name="AddTimeOut">Дополнительный таймаут</param>        
        internal C_RequestAnswerItem(byte[] RequestData, AnswerHandlerMethod AnswerMethod, ushort TimeOut,
            ushort AddTimeOut)
        {
            this.RequestData = RequestData;
            this.AnswerMethod = AnswerMethod;
            this.TimeOut = TimeOut;
            this.AddTimeOut = AddTimeOut;
            ID = Guid.NewGuid();
        }
    }
    #endregion

    #region ----- Базовый класс обмена данными с устройством RS-485 и RS-232-----
    /// <summary>
    /// Базовый класс обмена данными с устройством RS-485 и RS-232
    /// </summary>      
    public abstract class RSxxx_Device
    {
        /// <summary>
        /// Имя устройства
        /// </summary>
        public string Name;

        /// <summary>
        /// Флаг доступности устройства
        /// </summary>
        public bool isAvailable;

        /// <summary>
        /// Используемый COM порт
        /// </summary>
        public SerialPort UsedSerialPort;

        /// <summary>
        /// Флаг включения логирования
        /// </summary>
        internal bool isLogEnable;

        /// <summary>
        /// Флаг пропускаемого при опросе устройства
        /// </summary>
        internal bool isSkipped;

        /// <summary>
        /// Очередь запроса - ответа (Приоритетная)
        /// </summary>
        internal ConcurrentQueue<C_RequestAnswerItem> RequestAnswer_Primary;

        /// <summary>
        /// Очередь запроса - ответа (Дополнительная)
        /// </summary>
        internal ConcurrentQueue<C_RequestAnswerItem> RequestAnswer_Secondary;

        /// <summary>
        /// Таймаут записи (мс.)
        /// </summary>
        internal ushort TimeOut_Save;

        /// <summary>
        /// Таймаут чтения (мс.)
        /// </summary>
        internal ushort TimeOut_Read;

        /// <summary>
        /// Таймаут доп. (мс.)
        /// </summary>
        internal ushort TimeOut_Add;

        /// <summary>
        /// Ожидание между повторами (мс.)
        /// </summary>
        internal ushort TimeOut_ErrorRetry;

        /// <summary>
        /// Кол-во повторов при ошибке
        /// </summary>
        internal byte Error_RetryPass;

        /// <summary>
        /// Адрес устройства на шине RS-485
        /// </summary>
        internal byte? DeviceAddress;

        /// <summary>
        /// Интерфейс обмена
        /// </summary>
        public abstract DeviceInterface GetDeviceInterface { get; }

        /// <summary>
        /// Количество выходов
        /// </summary>
        public abstract byte OutputsCount { get; }

        /// <summary>
        /// Количество входов
        /// </summary>
        public abstract byte InputsCount { get; }

        /// <summary>
        /// Таблица кросcировки выходов (номер выхода, перенаправленный номер)
        /// </summary>
        public abstract Dictionary<byte, byte> JamperingTable_Out { get; set; }

        /// <summary>
        /// Таблица кросcировки входов (номер выхода, перенаправленный номер)
        /// </summary>
        public abstract Dictionary<byte, byte> JamperingTable_In { get; set; }

        /// <summary>
        /// Значения на входе
        /// </summary>
        public abstract List<float> InputsValue { get; set; }

        /// <summary>
        /// Значения на выходе
        /// </summary>
        public abstract List<float> OutputsValue { get; set; }

        /// <summary>
        /// Текущее значение
        /// </summary>
        public abstract string CurrentValue { get; set; }

        /// <summary>
        /// ИД команды
        /// </summary>
        internal abstract Guid CurrentID { get; set; }

        /// <summary>
        /// Инициализация класса для устройств RS-232
        /// </summary>
        /// <param name="Name">Имя устройства</param>        
        /// <param name="isLogEnable">true - включить логирование (запрос / ответ)</param>
        public RSxxx_Device(string Name, bool isLogEnable)
        {
            this.Name = Name;
            isSkipped = false;
            RequestAnswer_Primary = new ConcurrentQueue<C_RequestAnswerItem>();
            RequestAnswer_Secondary = new ConcurrentQueue<C_RequestAnswerItem>();
            DeviceAddress = null;
            isAvailable = false;
            UsedSerialPort = null;
            this.isLogEnable = isLogEnable;
            CurrentValue = string.Empty;
            CurrentID = Guid.Empty;

            TimeOut_Save = 1000;
            TimeOut_Read = 1000;
            TimeOut_Add = 10000;
            TimeOut_ErrorRetry = 1000;
            Error_RetryPass = 3;

            // Заполняем таблицу кроссировки выходов (по умолчанию)
            JamperingTable_Out = new Dictionary<byte, byte>();
            for (byte idx = 1; idx <= OutputsCount; idx++)
                JamperingTable_Out.Add(idx, idx);

            // Заполняем таблицу кроссировки входов (по умолчанию)
            JamperingTable_In = new Dictionary<byte, byte>();
            for (byte idx = 1; idx <= InputsCount; idx++)
                JamperingTable_In.Add(idx, idx);

            InputsValue = new List<float>();
            for (byte idx = 1; idx <= InputsCount; idx++)
                InputsValue.Add(float.NaN);

            OutputsValue = new List<float>();
            for (byte idx = 1; idx <= OutputsCount; idx++)
                OutputsValue.Add(float.NaN);
        }

        /// <summary>
        /// Инициализация класса для устройств RS-485
        /// </summary>
        /// <param name="Name">Имя устройства</param>
        /// <param name="DeviceAddress">Адрес устройства на шине RS-485</param>
        /// <param name="isLogEnable">true - включить логирование (запрос / ответ)</param>
        public RSxxx_Device(string Name, byte DeviceAddress, bool isLogEnable)
        {
            this.Name = Name;
            isSkipped = false;
            RequestAnswer_Primary = new ConcurrentQueue<C_RequestAnswerItem>();
            RequestAnswer_Secondary = new ConcurrentQueue<C_RequestAnswerItem>();
            this.DeviceAddress = DeviceAddress;
            isAvailable = false;
            UsedSerialPort = null;
            this.isLogEnable = isLogEnable;
            CurrentValue = string.Empty;

            TimeOut_Save = 1000;
            TimeOut_Read = 1000;
            TimeOut_Add = 10000;
            TimeOut_ErrorRetry = 1000;
            Error_RetryPass = 3;

            // Заполняем таблицу кроссировки выходов (по умолчанию)
            JamperingTable_Out = new Dictionary<byte, byte>();
            for (byte idx = 1; idx <= OutputsCount; idx++)
                JamperingTable_Out.Add(idx, idx);

            // Заполняем таблицу кроссировки входов (по умолчанию)
            JamperingTable_In = new Dictionary<byte, byte>();
            for (byte idx = 1; idx <= InputsCount; idx++)
                JamperingTable_In.Add(idx, idx);
        }

        /// <summary>
        /// Автоматическое выполнение запроса 
        /// </summary>
        /// <returns></returns>
        internal abstract void AutoRun();

        /// <summary>
        /// Ошибка доступа к устройству
        /// </summary>
        internal abstract void AccessError();

        /// <summary>
        /// Отправляет символьную команду устройству
        /// </summary>
        /// <param name="CMD">Символьная команда</param>
        /// <param name="RN_Flag">true - добавляет в конец "\r\n"</param>
        /// <param name="AHM">Метод обработки ответа (null - без метода)</param>
        /// <param name="FirstInRequestQueue">Первый в очередь запроса</param>
        /// <param name="TimeOut">Таймаут ответа</param>     
        /// <param name="AddTimeOut">Дополнительный таймаут ответа</param>             
        /// <returns>Guid != Guid.Empty - команда отправлена</returns>
        internal Guid SendCMDToDevice(string CMD, bool RN_Flag, AnswerHandlerMethod AHM, bool FirstInRequestQueue,
            ushort TimeOut, ushort AddTimeOut)
        {
            var result = Guid.Empty;

            try
            {
                if (!string.IsNullOrEmpty(CMD))
                {
                    var CMD_Byte = Encoding.Default.GetBytes(CMD + (RN_Flag ? "\r\n" : ""));

                    if (CMD_Byte.Length > 0)
                    {

                        if (FirstInRequestQueue)
                        {
                            RequestAnswer_Primary.Enqueue(new C_RequestAnswerItem(CMD_Byte, AHM,
                                AHM == null ? (ushort)0 : TimeOut, AddTimeOut));

                            if (RequestAnswer_Primary.TryPeek(out var Item))
                                result = Item.ID;
                        }
                        else
                        {
                            RequestAnswer_Secondary.Enqueue(new C_RequestAnswerItem(CMD_Byte, AHM,
                                AHM == null ? (ushort)0 : TimeOut, AddTimeOut));

                            if (RequestAnswer_Secondary.TryPeek(out var Item))
                                result = Item.ID;
                        }
                    }
                }
            }
            catch { }

            return result;
        }

        /// <summary>
        /// Отправляет массив байт устройству без ожидания ответа
        /// </summary>
        /// <param name="CMD">Масив байт</param>
        /// <param name="AHM">Метод обработки ответа</param>
        /// <param name="FirstInRequestQueue">Первый в очередь запроса</param>
        /// <param name="TimeOut">Таймаут ответа запроса</param>        
        /// <param name="AddTimeOut">Дополнительный таймаут ответа запроса</param>                
        /// <returns>Guid != Guid.Empty - команда отправлена</returns>
        internal Guid SendByteToDevice(byte[] CMD, AnswerHandlerMethod AHM, bool FirstInRequestQueue, ushort TimeOut,
            ushort AddTimeOut)
        {
            var result = Guid.Empty;

            try
            {
                if (CMD != null && CMD.Length > 0)
                {
                    if (FirstInRequestQueue)
                    {
                        RequestAnswer_Primary.Enqueue(new C_RequestAnswerItem(CMD, AHM,
                            AHM == null ? (ushort)0 : TimeOut, AddTimeOut));

                        if (RequestAnswer_Primary.TryPeek(out var Item))
                            result = Item.ID;
                    }
                    else
                    {
                        RequestAnswer_Secondary.Enqueue(new C_RequestAnswerItem(CMD, AHM,
                            AHM == null ? (ushort)0 : TimeOut, AddTimeOut));

                        if (RequestAnswer_Secondary.TryPeek(out var Item))
                            result = Item.ID;
                    }
                }
            }
            catch { }

            return result;
        }

        /// <summary>
        /// Подготовка запроса (Modbus) для чтения данных с устройства
        /// </summary>
        /// <param name="func">Функция чтения</param>
        /// <param name="DeviceAddress">Адрес устройства</param>
        /// <param name="StartAddress">Начальный адрес</param>
        /// <param name="ReadByteCount">Кол-во байт для чтения</param>
        /// <returns>Данные в формате Modbus</returns>
        internal byte[] PrepareData_ModbusRead(byte func, byte DeviceAddress, ushort StartAddress, ushort ReadByteCount)
        {
            byte[] result = null;

            if ((func == 0x03 || func == 0x04) && ReadByteCount > 1)
            {
                result = new byte[] { DeviceAddress, func, HiByte(StartAddress), LoByte(StartAddress), HiByte((ushort)(ReadByteCount / 2)), LoByte((ushort)(ReadByteCount / 2)) };

                var CRC = CrcModbus(result);

                Array.Resize(ref result, result.Length + 2); // CRC

                result[result.Length - 2] = LoByte(CRC);
                result[result.Length - 1] = HiByte(CRC);
            }
            return result;
        }

        /// <summary>
        /// Подготовка запроса (Modbus 0x05) для установки единичного выхода
        /// </summary>
        /// <param name="func">Функция чтения 0x05</param>
        /// <param name="DeviceAddress">Адрес устройства</param>
        /// <param name="InputAddress">Адрес выхода</param>
        /// <param name="InputState">Состояние</param>
        /// <returns>Данные в формате Modbus</returns>
        internal byte[] PrepareData_ModbusForceSingleCoill(byte func, byte DeviceAddress, ushort InputAddress, bool InputState)
        {
            byte[] result = null;

            var State = (ushort)(InputState ? 0xFF00 : 0x0000);

            if (func == 0x05)
            {
                result = new byte[] { DeviceAddress, func, HiByte(InputAddress), LoByte(InputAddress), HiByte(State), LoByte(State) };

                var CRC = CrcModbus(result);

                Array.Resize(ref result, result.Length + 2); // CRC

                result[result.Length - 2] = LoByte(CRC);
                result[result.Length - 1] = HiByte(CRC);
            }
            return result;
        }

        /// <summary>
        /// Подготовка запроса (Modbus) для чтения информации об устройстве
        /// </summary>        
        /// <param name="DeviceAddress">Адрес устройства</param>        
        /// <returns>Данные в формате Modbus или null</returns>
        internal byte[] PrepareData_ModbusInfo(byte DeviceAddress)
        {
            byte[] result = null;

            result = new byte[] { DeviceAddress, 0x11 };
            var CRC = CrcModbus(result);
            Array.Resize(ref result, result.Length + 2); // CRC
            result[result.Length - 2] = LoByte(CRC);
            result[result.Length - 1] = HiByte(CRC);

            return result;
        }

        /// <summary>
        /// Подготовка запроса (Modbus) для записи даннных в устройство
        /// </summary>
        /// <param name="func">Функция записи</param>
        /// <param name="DeviceAddress">Адрес устройства</param>
        /// <param name="StartAddress">Начальный адрес</param>
        /// <param name="WriteData">Данные для записи</param>
        /// <returns>Данные в формате Modbus или null</returns>
        internal byte[] PrepareData_ModbusWrite(byte func, byte DeviceAddress, ushort StartAddress, byte[] WriteData)
        {
            byte[] result = null;

            if ((func == 0x05 || func == 0x06 || func == 0x10) && WriteData != null && WriteData.Count() > 1)
            {
                if (func == 0x10)
                {
                    result = new byte[]
                    {
                        DeviceAddress,
                        func,
                        HiByte(StartAddress), LoByte(StartAddress),
                        HiByte((ushort)(WriteData.Length / 2)), LoByte((ushort)(WriteData.Length / 2)),
                        (byte)WriteData.Length
                    };
                }
                else
                {
                    result = new byte[]
                     {
                        DeviceAddress,
                        func,
                        HiByte(StartAddress), LoByte(StartAddress),
                     };
                }

                Array.Resize(ref result, result.Length + WriteData.Length); // Заголовок + Данные

                Array.Copy(WriteData, 0, result, func == 0x10 ? 7 : 4, WriteData.Length);

                var CRC = CrcModbus(result);

                Array.Resize(ref result, result.Length + 2); // CRC
                result[result.Length - 2] = LoByte(CRC);
                result[result.Length - 1] = HiByte(CRC);
            }
            return result;
        }

        /// <summary>
        /// Подготовка запроса (Modbus 0x17 или 0x77) для записи даннных в устройство и последующего чтения 
        /// </summary>        
        /// <param name="Func">Функция (0x17 или 0x77)</param>
        /// <param name="DeviceAddress">Адрес устройства</param>
        /// <param name="SaveStartAddress">Начальный адрес записи</param>
        /// <param name="SaveData">Данные для записи</param>
        /// <param name="ReadStartAddress">Начальный адрес чтения</param>
        /// <param name="ReadByteCount">Кол-во байт для чтения</param>
        /// <returns>Данные в формате Modbus или null</returns>
        internal byte[] PrepareData_ModbusWriteRead(byte Func, byte DeviceAddress, ushort SaveStartAddress, byte[] SaveData,
            ushort ReadStartAddress, ushort ReadByteCount)
        {
            byte[] result = null;

            if ((Func != 0x17 && Func != 0x77) || SaveData == null || SaveData.Count() < 2)
                return result;

            result = new byte[]
            {   DeviceAddress,
                Func,
                HiByte(ReadStartAddress), LoByte(ReadStartAddress),
                HiByte((ushort)(ReadByteCount / 2)), LoByte((ushort)(ReadByteCount / 2)),
                HiByte(SaveStartAddress), LoByte(SaveStartAddress),
                HiByte((ushort)(SaveData.Length / 2)), LoByte((ushort)(SaveData.Length / 2)),
                (byte)SaveData.Length
            };

            Array.Resize(ref result, result.Length + SaveData.Length); // Заголовок + Данные

            Array.Copy(SaveData, 0, result, result.Length - SaveData.Length, SaveData.Length);

            var CRC = CrcModbus(result);
            Array.Resize(ref result, result.Length + 2);
            result[result.Length - 2] = LoByte(CRC);
            result[result.Length - 1] = HiByte(CRC);

            return result;
        }

        /// <summary>
        /// Младший байт слова
        /// </summary>
        /// <param name="nValue">Слово</param>
        /// <returns></returns>
        internal byte LoByte(ushort nValue)
        {
            return (byte)(nValue & 0xFF);
        }

        /// <summary>
        /// Старший байт слова
        /// </summary>
        /// <param name="nValue">Слово</param>
        /// <returns></returns>
        internal byte HiByte(ushort nValue)
        {
            return (byte)(nValue >> 8);
        }

        /// <summary>
        /// CRC Modbus
        /// </summary>
        /// <param name="Buf">Массив байт</param>
        /// <returns>CRC</returns>
        internal ushort CrcModbus(byte[] Buf)
        {
            var ByteCount = (ushort)Buf.Length;
            ushort retval = 0xFFFF;
            byte j;
            byte carry;
            for (var i = 0; i < ByteCount; i++)
            {
                retval ^= Buf[i];
                for (j = 0; j < 8; j++)
                {
                    carry = (byte)(retval & 1);
                    retval >>= 1;
                    if (carry != 0)
                    {
                        retval ^= 0xA001;
                    }
                }
            }

            return retval;
        }

        /// <summary>
        /// CRC ОВЕН
        /// </summary>
        /// <param name="data">Массив байт</param>
        /// <returns>CRC</returns>
        internal ushort CrcOVEN(byte[] data)
        {
            ushort result = 0x0000;

            for (var i = 0; i < data.Length; i++)
                for (var j = 0; j < 8; ++j, data[i] <<= 1)
                    result = ((data[i] ^ (result >> 8)) & 0x80) > 0 ? (ushort)((result << 1) ^ 0x8F57) : (ushort)(result << 1);

            return result;
        }
       
    }
    #endregion    

    #region ----- Класс планировщика устройств -----
    /// <summary>
    /// Класс планировщика устройств
    /// </summary>
    public class DeviceSheduler
    {
        /// <summary>
        /// Имя файла конфигурации оборудования
        /// </summary>
        private const string CFG_File = "devices.cfg";

        /// <summary>
        /// Имя файла логов оборудования
        /// </summary>
        private const string LOG_File = "./SerialPortLogs";

        /// <summary>
        /// Путь с именем файла конфигурации оборудования
        /// </summary>
        private string CFG_Path_FileName = CFG_File;

        /// <summary>
        /// Путь с именем файла LOG
        /// </summary>
        internal string LOG_Path_FileName = LOG_File;



        /// <summary>
        /// Статус планировщика
        /// </summary>
        /// <returns>true - планировщик запущен</returns>
        public bool DoWork { get; private set; }

        /// <summary>
        /// Список устройств RS (485, 232) (COM порт устройства, Устройство RS (485, 232))
        /// </summary>
        private readonly Dictionary<SerialPort, List<RSxxx_Device>> RS_Devices;

        /// <summary>
        /// Список оборудования
        /// </summary>
        public List<RSxxx_Device> Devices
        {
            get
            {
                var rs_devices = new List<RSxxx_Device>();
                foreach (var devices in RS_Devices.Values)
                    foreach (var device in devices)
                        rs_devices.Add(device);
                return rs_devices;
            }
        }


        /// <summary>
        /// Очередь данных в LOG-файл
        /// </summary>
        private ConcurrentQueue<C_LOG> LogData = new ConcurrentQueue<C_LOG>();

        /// <summary>
        /// Поток LOG-файла
        /// </summary>
        private Thread LogThread;

        /// <summary>
        /// Потоки планировщика устройств RS
        /// </summary>
        private List<Thread> ShedulerThreads_RS = null;

        /// <summary>
        /// Инициализация класса        
        /// </summary>
        public DeviceSheduler()
        {
            RS_Devices = new Dictionary<SerialPort, List<RSxxx_Device>>();
            ShedulerThreads_RS = null;

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);

            LOG_Path_FileName = Path.Combine(Path.GetDirectoryName(path), LOG_File);

            CFG_Path_FileName = Path.Combine(Path.GetDirectoryName(path), CFG_File);
        }

        public DeviceSheduler(string LOG_FileName, string CFG_Path) : this(CFG_Path)
        {
        }

        /// <summary>
        /// Инициализация класса        
        /// <param name="CFG_Path">Путь к файлу конфигурации</param>
        /// </summary>
        public DeviceSheduler(string CFG_Path)
        {
            RS_Devices = new Dictionary<SerialPort, List<RSxxx_Device>>();
            ShedulerThreads_RS = null;

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);

            LOG_Path_FileName = Path.Combine(Path.GetDirectoryName(path), LOG_File);

            if (!string.IsNullOrEmpty(CFG_Path))
            {
                try
                {
                    CFG_Path = $"{CFG_Path}{(CFG_Path.EndsWith("\\") ? string.Empty : "\\")}";

                    if (!Directory.Exists(CFG_Path))
                        Directory.CreateDirectory(CFG_Path);

                    CFG_Path_FileName = $"{CFG_Path}{CFG_File}";
                }
                catch { }
            }
            else
                CFG_Path_FileName = Path.Combine(Path.GetDirectoryName(path), CFG_File);
        }

        /// <summary>
        /// Сменить путь файла конфигурации
        /// </summary>
        /// <param name="CFG_Path">Новый путь</param>
        /// <returns></returns>
        public bool Change_CFG_Path(string CFG_Path)
        {
            var result = false;

            if (!string.IsNullOrEmpty(CFG_Path))
            {
                try
                {
                    CFG_Path = $"{CFG_Path}{(CFG_Path.EndsWith("\\") ? string.Empty : "\\")}";

                    if (!Directory.Exists(CFG_Path))
                        Directory.CreateDirectory(CFG_Path);

                    CFG_Path_FileName = $"{CFG_Path}{CFG_File}";

                    result = true;
                }
                catch { }
            }

            return result;
        }

        /// <summary>
        /// Добавить RS-485 устройства в планировщик
        /// </summary>
        /// <returns></returns>
        public bool Add_RS485(List<RSxxx_Device> Devices, string PortName, int BaudRate, int DataBits, Parity Parity, StopBits StopBits, Handshake Handshake, bool DTR, bool RTS)
        {
            var result = false;

            try
            {
                PortName = PortName.ToUpper();

                if (!DoWork && !string.IsNullOrEmpty(PortName) && Devices.Count > 0 &&
                    (PortName == "NONE" || RS_Devices.Keys.Where(x => x.PortName.Equals(PortName)).Count() == 0))
                {
                    result = true;

                    foreach (var Device in Devices)
                        if (Device.DeviceAddress == null)
                            result = false;

                    if (result)
                    {
                        RS_Devices.Add(new SerialPort(), new List<RSxxx_Device>(Devices));
                        RS_Devices.Keys.Last().PortName = PortName;
                        RS_Devices.Keys.Last().BaudRate = BaudRate;
                        RS_Devices.Keys.Last().DataBits = DataBits;
                        RS_Devices.Keys.Last().Parity = Parity;
                        RS_Devices.Keys.Last().StopBits = StopBits;
                        RS_Devices.Keys.Last().Handshake = Handshake;
                        RS_Devices.Keys.Last().DtrEnable = DTR;
                        RS_Devices.Keys.Last().RtsEnable = RTS;

                        RS_Devices.Keys.Last().WriteBufferSize = 4096;
                        RS_Devices.Keys.Last().ReadBufferSize = 4096;
                    }
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Добавить RS-232 устройство в планировщик
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="PortName"></param>
        /// <param name="BaudRate"></param>
        /// <param name="DataBits"></param>
        /// <param name="Parity"></param>
        /// <param name="StopBits"></param>
        /// <param name="Handshake"></param>
        /// <param name="DTR"></param>
        /// <param name="RTS"></param>
        /// <returns></returns>
        public bool Add_RS232(RSxxx_Device Device, string PortName, int BaudRate, int DataBits, Parity Parity, StopBits StopBits, Handshake Handshake, bool DTR, bool RTS)
        {
            var result = false;

            try
            {
                if (!DoWork && !string.IsNullOrEmpty(PortName) &&
                    (PortName == "NONE" || RS_Devices.Keys.Where(x => x.PortName.Equals(PortName)).Count() == 0))
                {
                    RS_Devices.Add(new SerialPort(), new List<RSxxx_Device>() { Device });
                    RS_Devices.Keys.Last().PortName = PortName;
                    RS_Devices.Keys.Last().BaudRate = BaudRate;
                    RS_Devices.Keys.Last().DataBits = DataBits;
                    RS_Devices.Keys.Last().Parity = Parity;
                    RS_Devices.Keys.Last().StopBits = StopBits;
                    RS_Devices.Keys.Last().Handshake = Handshake;
                    RS_Devices.Keys.Last().DtrEnable = DTR;
                    RS_Devices.Keys.Last().RtsEnable = RTS;

                    RS_Devices.Keys.Last().WriteBufferSize = 4096;
                    RS_Devices.Keys.Last().ReadBufferSize = 4096;

                    result = true;
                }
            }
            catch { }

            return result;
        }

        /// <summary>
        /// Пропустить опрос устройства
        /// </summary>        
        /// <param name="Name">Имя устройства</param>
        /// <param name="SkipDevice">Флаг пропуска опроса устройств</param>
        /// <returns></returns>
        public bool SkipDevice(string Name, E_SkippDevice SkipDevice)
        {
            var result = false;

            if (!string.IsNullOrEmpty(Name))
            {
                foreach (var Devices in RS_Devices)
                {
                    foreach (var Device in Devices.Value)
                    {
                        if (Device.Name == Name)
                        {
                            Device.isSkipped = SkipDevice == E_SkippDevice.ON;
                            result = true;
                            break;
                        }
                    }

                    if (result)
                        break;
                }
            }
            return result;
        }


        /// <summary>
        /// Запуск планировщика
        /// </summary>        
        /// <returns></returns>
        public bool Start()
        {
            var result = false;
            var isRS_Complete = false;

            try
            {
                // Потоки для устройств RS (485, 232) - Один поток на один COM-порт
                if ((ShedulerThreads_RS == null || ShedulerThreads_RS.Count == 0) && RS_Devices.Count > 0)
                {

                    // Задача LOG                    
                    LogData = new ConcurrentQueue<C_LOG>();
                    LogThread = new Thread(Log_Method) { Name = "DeveiceTaskSheduler_Log" };
                    LogThread.Start();

                    ShedulerThreads_RS = new List<Thread>(RS_Devices.Count);
                    DoWork = true;
                    var Number = 0;
                    foreach (var devices in RS_Devices)
                    {
                        // Добавление COM портов в класс
                        foreach (var device in devices.Value)
                            device.UsedSerialPort = devices.Key;

                        ShedulerThreads_RS.Add(new Thread(ThreadMethod_RS)
                        {
                            Name = $"DeviceTaskSheduler_Thread{Number + 1} (COM-PORT: {devices.Key.PortName})"
                        });
                        ShedulerThreads_RS[Number++].Start(devices);
                    }

                    result = true;
                }
            }
            catch
            {
                result = false;
                DoWork = false;
            }

            return result;
        }

        /// <summary>
        /// Метод записи в лог файл
        /// </summary>
        private void Log_Method()
        {
            

            while (DoWork)
            {
                try
                {
                    if (LogData.Count > 0 && LogData.TryDequeue(out var LOG))
                    {
                        for (var pass = 0; pass < 10; ++pass)
                        {
                            try
                            {
                                var LogDirectory = "./SerialPortLogs";
                                var path = $"{Directory.GetCurrentDirectory()}/{LogDirectory}/{LOG.COMPort}_DeviceSheduler_{DateTime.Now:yyyy-MM-dd}.log";
                                try
                                {
                                    var dir = Path.GetDirectoryName(path);
                                    if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                                    var _streamWriter = new StreamWriter(path, true);

                                    _streamWriter.WriteLine($"{LOG.DT:dd.MM.yyyy HH:mm:ss.fff} - " +
                                    $"COM-порт: {LOG.COMPort} " +
                                    $"Адрес: {(LOG.DeviceAddress == null ? "Нет" : "0x" + ((byte)LOG.DeviceAddress).ToString("X2"))} " +
                                    $"Запрос: {(LOG.RequestData.Length > 0 ? BitConverter.ToString(LOG.RequestData) : string.Empty)} " +
                                    $"Ответ: {(LOG.AnswerData.Length > 0 ? BitConverter.ToString(LOG.AnswerData) : string.Empty)}");

                                    _streamWriter.Flush();
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                                
                                break;
                            }
                            catch
                            {
                                Thread.Sleep(1);
                            }
                        }
                    }
                    Thread.Sleep(1);
                }
                catch { }
            }
        }

        /// <summary>
        /// Остановка планировщика
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            var result = false;

            if (DoWork)
            {
                try
                {
                    DoWork = false;

                    // Завершение потоков устройств RS (485, 232)
                    if (ShedulerThreads_RS != null)
                    {
                        // Перебираем потоки
                        int passes = 100, workingThreadsCnt = 0;
                        do
                        {
                            workingThreadsCnt = 0;
                            for (var j = 0; j < ShedulerThreads_RS.Count; ++j)
                            {
                                if (ShedulerThreads_RS[j] != null)
                                {
                                    if (ShedulerThreads_RS[j].Join(100))
                                        ShedulerThreads_RS[j] = null;
                                    else
                                        ++workingThreadsCnt;
                                }
                            }
                        } while (passes-- > 0 && workingThreadsCnt > 0);

                        ShedulerThreads_RS.Clear();
                        LogThread = null;

                        // Закрываем порты
                        foreach (var Device in RS_Devices.Keys)
                            Device.Close();

                        result = true;
                    }
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Метод потока планировщика устройств RS (485, 232) на одном COM порту
        /// </summary>        
        private void ThreadMethod_RS(object sPort)
        {
            var devices = (KeyValuePair<SerialPort, List<RSxxx_Device>>)sPort;

            while (DoWork)
            {
                // Проходим по всем устройствам на текущем COM порту
                foreach (var device in devices.Value)
                {
                    try
                    {
                        if (!DoWork)
                            break;

                        if (devices.Key.PortName.Contains("NONE"))
                        {
                            device.isAvailable = false;
                            continue;
                        }

                        if (device.isSkipped)
                        {
                            device.isAvailable = false;

                            if (devices.Key.IsOpen)
                            {
                                // Закрываем порт если опрос всех устройств на этом порту отключен
                                if (devices.Value.Count(x => x.isSkipped) == devices.Value.Count)
                                    devices.Key.Close();
                            }

                            continue;
                        }

                        if (!devices.Key.IsOpen)
                        {
                            try
                            {
                                devices.Key.Open();
                            }
                            catch
                            {
                                device.isAvailable = false;
                                continue;
                            }
                        }

                        // Вызываем метод автоопроса устройства                                               
                        device.AutoRun();

                        // Перебираем очередь запросов
                        while (true)
                        {
                            // Получаем элемент очереди
                            C_RequestAnswerItem RequestAnswerItem = null;
                            if (!device.RequestAnswer_Primary.TryDequeue(out RequestAnswerItem) &&
                                !device.RequestAnswer_Secondary.TryDequeue(out RequestAnswerItem))
                            {
                                break;
                            }

                            // Широковещательная команда всем устройствам на 485
                            var isAllDevice485 = device.GetDeviceInterface == DeviceInterface.RS_485 &&
                                                 RequestAnswerItem.RequestData.Length > 1 &&
                                                 RequestAnswerItem.RequestData[0] == 0x00 &&
                                                 RequestAnswerItem.RequestData[1] > 0;

                            // Отправляем данные на устройство
                            var passes = RequestAnswerItem.TimeOut > 100 ? RequestAnswerItem.TimeOut / 100 : 1;
                            var last_readbyte = 0;

                            devices.Key.DiscardOutBuffer();

                            // Если не дополнительная команда получения ответа без запроса
                            if (!RequestAnswerItem.RequestData.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }))
                            {
                                devices.Key.DiscardInBuffer();
                                devices.Key.Write(RequestAnswerItem.RequestData, 0, RequestAnswerItem.RequestData.Count());
                            }

                            // Основной таймаут
                            do
                            {
                                last_readbyte = devices.Key.BytesToRead;

                                Thread.Sleep(100);

                                // Данные пришли раньше чем тамйаут - выходим
                                if (devices.Key.BytesToRead > 0 && devices.Key.BytesToRead == last_readbyte && passes > 1)
                                    passes = 1;

                            } while (--passes > 0);

                            // Дополнительный таймаут, если есть
                            if (RequestAnswerItem.AddTimeOut > 0)
                                Thread.Sleep(RequestAnswerItem.AddTimeOut);

                            var ReadByte = new byte[devices.Key.BytesToRead];

                            if (ReadByte.Length > 0)
                                devices.Key.Read(ReadByte, 0, ReadByte.Length);

                            // Флаг активности устройства                            
                            device.isAvailable = RequestAnswerItem.TimeOut == 0 || isAllDevice485 || ReadByte.Length > 0;

                            // Вызов обработчика                            
                            RequestAnswerItem.AnswerMethod?.Invoke(ReadByte, RequestAnswerItem.ID);

                            // Логирование
                            if (!device.isLogEnable)
                            {
                                LogData.Enqueue(new C_LOG(
                                    DateTime.Now,
                                    devices.Key.PortName,
                                    device.DeviceAddress,
                                    RequestAnswerItem.RequestData,
                                    ReadByte));
                            }
                        }
                    }
                    catch
                    {
                        device.isAvailable = false;
                        device.AccessError();
                    }
                }

                // Не будем нагружать приборы                
                Thread.Sleep(3000);
            }
        }
    }
    #endregion
}
