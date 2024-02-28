namespace SPU_7.Models
{
    /// <summary>
    /// Интерфейс наблюдаемого для реализации паттерна наблюдатель
    /// </summary>
    public interface IObservable
    {
        /// <summary>
        /// Зарегистрировать наблюдателя
        /// </summary>
        /// <param name="observer"></param>
        void RegisterObserver(IObserver observer);

        /// <summary>
        /// Убрать наблюдателя
        /// </summary>
        /// <param name="observer"></param>
        void RemoveObserver(IObserver observer);

        /// <summary>
        /// Уведомить наблюдателей о изменении
        /// </summary>
        /// <param name="obj"></param>
        void NotifyObservers(object obj);
    }
}
