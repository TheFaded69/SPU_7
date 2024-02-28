using System.ComponentModel;

namespace SPU_7.Common.Stand
{
    [Description("Режим управления стендом")]
    public enum ControlType
    {
        None = 0,

        [Description("Автоматический")]
        Auto = 1,

        [Description("Ручной")]
        Manual = 2,
    }
}
