using System;

namespace SPU_7.Models.Scripts
{
    /// <summary>
    /// Интерфейс управления сценарием
    /// </summary>
    public interface IScriptController : IDisposable, IObservable
    {
        /// <summary>
        /// Запуск сценария
        /// </summary>
        void Start();

        /// <summary>
        /// Остановка сценария
        /// </summary>
        void Stop();

        /// <summary>
        /// Установка сценария для контролера
        /// </summary>
        /// <param name="scriptModel">Модель сценария</param>
        void SetScript(ScriptModel scriptModel);
    }
}
