using System.Collections.ObjectModel;
using SPU_7.Common.Stand;

namespace SPU_7.Models.Services.Logger
{
    /// <summary>
    /// Интерфейс простейшего логгера
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Логгирование внутри программы (например для отображения в кастомной консоли)
        /// </summary>
        /// <param name="logMessage">Сообщение</param>
        /// <param name="needLogToFile">Нужно ли записать в файл</param>
        /// <returns>Удалось ли записать</returns>
        bool Logging(LogMessage logMessage);
        
        /// <summary>
        /// Добавляет коллекцию куда будет вестись дополнительное логирование
        /// </summary>
        /// <param name="collection">Коллекция для логирования</param>
        void AddCollectionForLogging(ObservableCollection<LogMessage> collection);

        /// <summary>
        /// Clear log
        /// </summary>
        void ClearLog();
    }
}