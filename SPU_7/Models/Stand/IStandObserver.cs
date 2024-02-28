using SPU_7.Extensions;

namespace SPU_7.Models.Stand
{
    public interface IStandObserver : IObserver
    {
        /// <summary>
        /// Обновить данные у подписчиков
        /// </summary>
        /// <param name="dataPair">Обновленный объект</param>
        void UpdateFromDataPair(DataPair dataPair);
    }
}
