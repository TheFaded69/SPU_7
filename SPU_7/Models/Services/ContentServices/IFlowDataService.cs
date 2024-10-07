using SPU_7.Extensions.Interface;

namespace SPU_7.Models.Services.ContentServices;

public interface IFlowDataService
{
    void SubscribeDataObserver(IFlowDataObserver flowDataObserver);

    void UnsubscribeDataObserver(IFlowDataObserver flowDataObserver);
    
    void ReceiveData(double currentFlow, double currentFrequency);
}