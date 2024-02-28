using System.ComponentModel;

namespace SPU_7.Common.Settings
{
    public enum ValidationSettingsType
    {
        None = 0,

        [Description("Все приборы")]
        All = 1,

        [Description("До первого брака")]
        BeforeFirstDefect = 2,
    }
}
