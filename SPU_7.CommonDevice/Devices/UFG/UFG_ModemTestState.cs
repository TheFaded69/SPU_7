namespace SPU_7.CommonDevice.Devices.UFG;

public enum UFG_ModemTestState : uint
{
    Disable = 0x0u,
    ActivateGPRS = 0x1u,
    ActivateCSD = 0x2u,
    ActivateAccept = 0x3u,
}