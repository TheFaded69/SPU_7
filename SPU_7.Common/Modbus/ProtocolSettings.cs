namespace SPU_7.Common.Modbus
{
    /// <summary>
    /// Настройки протокола
    /// </summary>
    public class ProtocolSettings
    {
        /// <summary>
        /// Преамбула
        /// </summary>
        public byte[] Preamble { get; set; }

        /// <summary>
        /// Задержка после преамбулы
        /// </summary>
        public int DelayAfterPreamble { get; set; }

        /// <summary>
        /// Нужна или нет преамбула
        /// </summary>
        public bool IsPreambleNeed { get; set; }

        /// <summary>
        /// Количество попыток
        /// </summary>
        public int AttemptCount { get; set; }

        /// <summary>
        /// Таймаут чтения
        /// </summary>
        public int ReadTimeout { get; set; }

        /// <summary>
        /// Таймаут записи
        /// </summary>
        public int WriteTimeout { get; set; }

        /// <summary>
        /// Автоматический опрос регистров
        /// </summary>
        public bool IsPoolingNeed { get; set; }

        /// <summary>
        /// Период автоопроса (мс)
        /// </summary>
        public int PoolingPeriod { get; set; }
    }
}