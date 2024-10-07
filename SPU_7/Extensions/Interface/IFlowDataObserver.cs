namespace SPU_7.Extensions.Interface;

public interface IFlowDataObserver
{
    void AcceptFlowData(double currentFlow, double currentFrequency);
}