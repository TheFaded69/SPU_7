using SPU_7.Extensions;

namespace SPU_7.Models.Stand
{
    public interface IStandObservable : IObservable
    {
        /// <summary>
        /// Уведомить наблюдателей о изменении
        /// </summary>
        /// <param name="dataPair"></param>
        void NotifyObserverByDataPair(DataPair dataPair);
    }
}
