﻿namespace SPU_7.Domain.Devices.UniversalDevices;

public interface IUniversalDevice : IDevice
{
   
    /// <summary>
    /// Считать давление с ДД привязанного к позиции СГ
    /// </summary>
    /// <returns></returns>
    Task<float?> ReadPressureAsync();
}