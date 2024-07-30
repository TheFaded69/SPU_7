using System.Text;

namespace UDMCalibrationStand.DeviceTaskSheduler
{

    #region----- Паскаль-04 -----
    /// <summary>
    /// Класс устройства Паскаль-04 или Метран-518 (Барометрический модуль давления)
    /// </summary>
    public class Metran518 : RSxxx_Device
    {
        /// <summary>
        /// Текущий ИД
        /// </summary>
        internal override Guid CurrentID { get; set; }

        /// <summary>
        /// Интерфейс обмена
        /// </summary>
        public override DeviceInterface GetDeviceInterface { get { return DeviceInterface.RS_232; } }

        /// <summary>
        /// Количество выходов
        /// </summary>
        public override byte OutputsCount { get { return 0x00; } }

        /// <summary>
        /// Количество входов
        /// </summary>
        public override byte InputsCount { get { return 0x00; } }

        /// <summary>
        /// Таблица кроссировки выходов
        /// </summary>
        public override Dictionary<byte, byte> JamperingTable_Out { get; set; }

        /// <summary>
        /// Таблица кроссировки входов
        /// </summary>
        public override Dictionary<byte, byte> JamperingTable_In { get; set; }

        /// <summary>
        /// Значение на входах
        /// </summary>
        public override List<float> InputsValue { get; set; }

        /// <summary>
        /// Значение на выходах
        /// </summary>
        public override List<float> OutputsValue { get; set; }

        /// <summary>
        /// Текущее значение давления в кПа
        /// </summary>
        public override string CurrentValue { get; set; }

        /// <summary>
        /// Давление в кПа
        /// </summary>
        public float Pressure { get; set; }

        /// <summary>
        /// Единица измерения давления
        /// </summary>
        public E_Measure_Unit Measure_Unit { get; set; }

        /// <summary>
        /// Единицы измерения
        /// </summary>
        public enum E_Measure_Unit
        {
            /// <summary>
            /// Нет
            /// </summary>
            Нет = 0,
            /// <summary>
            /// кПа
            /// </summary>
            кПа,
            /// <summary>
            /// МПа
            /// </summary>
            МПа,
            /// <summary>
            /// Па
            /// </summary>
            Па
        }

        /// <summary>
        /// Устройство
        /// </summary>
        public enum E_Device_Type : byte
        {
            /// <summary>
            /// Метран-518
            /// </summary>
            Метран_518 = 0x02,
            /// <summary>
            /// Паскаль-04
            /// </summary>
            Паскаль_04 = 0x03
        }

        /// <summary>
        /// Инициализация класса (Настройка: COM, 19200, Data:8, Parity:Odd, StopBits:One )
        /// </summary>
        /// <param name="Name">Имя устройства</param>
        /// <param name="DeviceType">Паскаль_04 (0x03), Метран-518 (0x02)</param>
        /// <param name="isLogEnable">Логирование</param>
        public Metran518(string Name, E_Device_Type DeviceType, bool isLogEnable)
            : base(Name, isLogEnable)
        {
            this.DeviceAddress = (byte)DeviceType;
            this.CurrentValue = string.Empty;
            this.Measure_Unit = E_Measure_Unit.Нет;
        }


        /// <summary>
        /// Автоматическое выполнение запроса 
        /// </summary>
        internal override void AutoRun()
        {
            // Опрос давления
            var data = new byte[] { 0x02, (byte)this.DeviceAddress, 0x01, 0x00,
                (byte)this.DeviceAddress == (byte)E_Device_Type.Паскаль_04 ? (byte)0x00 : (byte)0x01 };

            SendByteToDevice(new byte[] { 0x02, (byte)this.DeviceAddress, 0x01, 0x00,
                (byte)this.DeviceAddress == (byte)E_Device_Type.Паскаль_04 ? (byte)0x00 : (byte)0x01 }, Answer_VendorNumber,
                false, this.TimeOut_Read, 0);
        }

        private void Answer_VendorNumber(byte[] Data, Guid ID)
        {
            try
            {
                if (Data.Length == 12 && Data[0] == 0x06 && Data[1] == (byte)this.DeviceAddress && Data[2] == 0x01 && Data[3] == 0x07)
                {
                    var pressureData = new byte[] { Data[10], Data[9], Data[8], Data[7] };

                    this.Pressure = BitConverter.ToSingle(new byte[] { Data[10], Data[9], Data[8], Data[7] }, 0);

                    switch (Data[6])
                    {
                        case 0: this.Measure_Unit = E_Measure_Unit.кПа; break;
                        case 1: this.Measure_Unit = E_Measure_Unit.МПа; break;
                        case 2: this.Measure_Unit = E_Measure_Unit.Па; break;
                        default: this.Measure_Unit = E_Measure_Unit.Нет; break;
                    }

                    this.CurrentValue = $"{this.Pressure}{(this.Measure_Unit == E_Measure_Unit.Нет ? string.Empty : $" {this.Measure_Unit}")}";

                    switch (this.Measure_Unit)
                    {
                        case E_Measure_Unit.кПа:
                            Pressure *= 1000;
                            Pressure = (float)Math.Round((double)Pressure, 3);
                            break;

                        case E_Measure_Unit.МПа:
                            Pressure *= 1000000;
                            Pressure = (float)Math.Round((double)Pressure, 3);
                            break;

                    }

                    //Pressure += 101325; //добавляем одну атмосферу
                }
                else
                {
                    this.CurrentValue = string.Empty;
                    this.Measure_Unit = E_Measure_Unit.Нет;
                }
            }
            catch
            {
                this.CurrentValue = string.Empty;
                this.Measure_Unit = E_Measure_Unit.Нет;
            }

            this.CurrentID = ID;
        }

        /// <summary>
        /// Ошибка доступа к устройству
        /// </summary>
        internal override void AccessError()
        {
            this.CurrentValue = string.Empty;
            this.Measure_Unit = E_Measure_Unit.Нет;
        }
    }
    #endregion

    #region----- Mit8 -----
    /// <summary>
    /// Класс устройства Паскаль-04 или Метран-518 (Барометрический модуль давления)
    /// </summary>
    public class Mit8 : RSxxx_Device
    {
        /// <summary>
        /// Текущий ИД
        /// </summary>
        internal override Guid CurrentID { get; set; }

        /// <summary>
        /// Интерфейс обмена
        /// </summary>
        public override DeviceInterface GetDeviceInterface { get { return DeviceInterface.RS_232; } }

        /// <summary>
        /// Количество выходов
        /// </summary>
        public override byte OutputsCount { get { return 0x00; } }

        /// <summary>
        /// Количество входов
        /// </summary>
        public override byte InputsCount { get { return 0x00; } }

        /// <summary>
        /// Таблица кроссировки выходов
        /// </summary>
        public override Dictionary<byte, byte> JamperingTable_Out { get; set; }

        /// <summary>
        /// Таблица кроссировки входов
        /// </summary>
        public override Dictionary<byte, byte> JamperingTable_In { get; set; }

        /// <summary>
        /// Значение на входах
        /// </summary>
        public override List<float> InputsValue { get; set; }

        /// <summary>
        /// Значение на выходах
        /// </summary>
        public override List<float> OutputsValue { get; set; }

        /// <summary>
        /// Текущее значение давления в кПа
        /// </summary>
        public override string CurrentValue { get; set; }

        /// <summary>
        /// Температура канала 1
        /// </summary>
        public float? Temperature1 { get; set; }

        /// <summary>
        /// Температура канала 2
        /// </summary>
        public float? Temperature2 { get; set; }

        /// <summary>
        /// Единица измерения давления
        /// </summary>
        public E_Measure_Unit Measure_Unit { get; set; }

        /// <summary>
        /// Единицы измерения
        /// </summary>
        public enum E_Measure_Unit
        {
            
            /// <summary>
            /// Нет
            /// </summary>
            Нет = 0,
            /// <summary>
            /// кПа
            /// </summary>
            кПа,
            /// <summary>
            /// МПа
            /// </summary>
            МПа,
            /// <summary>
            /// Па
            /// </summary>
            Па
        }

        /// <summary>
        /// Устройство
        /// </summary>
        public enum E_Device_Type : byte
        {
            /// <summary>
            /// Метран-518
            /// </summary>
            Mit8 = 0x02,

        }

        /// <summary>
        /// Инициализация класса (Настройка: COM, 19200, Data:8, Parity:Odd, StopBits:One )
        /// </summary>
        /// <param name="Name">Имя устройства</param>
        /// <param name="DeviceType">Паскаль_04 (0x03), Метран-518 (0x02)</param>
        /// <param name="isLogEnable">Логирование</param>
        public Mit8(string Name, E_Device_Type DeviceType, bool isLogEnable)
            : base(Name, isLogEnable)
        {
            this.DeviceAddress = (byte)DeviceType;
            this.CurrentValue = string.Empty;
            this.Measure_Unit = E_Measure_Unit.Нет;
        }


        /// <summary>
        /// Автоматическое выполнение запроса 
        /// </summary>
        internal override void AutoRun()
        {
            // Опрос давления
            SendByteToDevice(new byte[] { 0x02, (byte)this.DeviceAddress, 0x01, 0x00,
                (byte)this.DeviceAddress}, Answer_VendorNumber,
                false, this.TimeOut_Read, 0);
        }

        private void Answer_VendorNumber(byte[] Data, Guid ID)
        {
            try
            {
                if (Data.Length == 0 || Data.Length > 16)
                {
                    this.CurrentID = ID;
                    return;
                }

                if (Data[0] == '1')
                {
                    var data = new byte[Data.Count() - 4];

                    for (var i = 2; i < Data.Count() - 2; i++)
                    {
                        data[i - 2] = Data[i];
                    }

                    var str1 = Encoding.ASCII.GetString(data);

                    Temperature1 = (float)Math.Round(ParseAnswer(str1), 4);
                }
                else if (Data[0] == '2')
                {
                    var data = new byte[Data.Count() - 4];

                    for (var i = 2; i < Data.Count() - 2; i++)
                    {
                        data[i - 2] = Data[i];
                    }

                    var str2 = Encoding.ASCII.GetString(data);

                    Temperature2 = (float)Math.Round(ParseAnswer(str2), 4);
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch
            {
                throw;
                /*this.CurrentValue = string.Empty;
                this.Measure_Unit = E_Measure_Unit.Нет;*/
            }

            this.CurrentID = ID;
        }

        private double ParseAnswer(string data)
        {
            double result;

            var splitted = data.Split('E');

            var splittedNew = splitted[0].Replace('.', ',');

            result = double.Parse(splittedNew);

            if (splitted[1][0] == '+') 
            {
                return result * Math.Pow(10, double.Parse(splitted[1]));
            }
            else if (splitted[1][0] == '-') 
            { 
                return result * Math.Pow(10, double.Parse(splitted[1]));

            }

            return result;
        }

        /// <summary>
        /// Ошибка доступа к устройству
        /// </summary>
        internal override void AccessError()
        {
            this.CurrentValue = string.Empty;
            this.Measure_Unit = E_Measure_Unit.Нет;
        }
    }
    #endregion   

}