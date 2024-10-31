using System.ComponentModel;

namespace SPU_7.Common.Settings
{
    public enum ValidationType
    {
        None = 0,

        [Description("Поверка по пройденному объему")]
        ValidationByVolume = 1,
    }
}
