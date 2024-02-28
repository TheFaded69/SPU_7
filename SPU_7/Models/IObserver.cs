namespace SPU_7.Models
{
    /// <summary>
    /// Интерфейс наблюдателя для реализации паттерна наблюдателя
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// Обновить данные у подписчиков
        /// </summary>
        /// <param name="obj">Обновленный объект</param>
        void Update(object obj);
    }
}
