namespace SPU_7.Common.Stand
{
    public class LogMessage
    {
        public LogMessage(string message, LogLevel logLevel)
        {
            Message = message;
            LogLevel = logLevel;
        }

        /// <summary>
        /// Сообщение для логирования
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Важность сообщения для логирования (нужно для определеения цвета при выводе куда либо например и поиска плохих сообещений в лог файле)
        /// </summary>
        public LogLevel LogLevel { get; set; }
    }
}
