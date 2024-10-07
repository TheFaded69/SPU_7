using System.Collections.Generic;
using SPU_7.Extensions.Interface;

namespace SPU_7.Models.Services.ContentServices;

public class FlowDataService : IFlowDataService
{
    public void SubscribeDataObserver(IFlowDataObserver flowDataObserver) => _observers.Add(flowDataObserver);
    public void UnsubscribeDataObserver(IFlowDataObserver flowDataObserver) => _observers.Remove(flowDataObserver);

    public void ReceiveData(double currentFlow, double currentFrequency) => _observers.ForEach(obs => obs.AcceptFlowData(currentFlow, currentFrequency));

    private readonly List<IFlowDataObserver> _observers = [];
}