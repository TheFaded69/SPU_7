using System.ComponentModel;

namespace SPU_7.Common.Settings
{
    public enum ValidationType
    {
        None = 0,

        [Description("Поверка по пройденному объему")]
        ValidationByVolume = 1,

        [Description("Поверка по установленному расходу")]
        ValidationByFlow = 2,
        
        [Description("Поверка по пройденном объему автоматическая")]
        AutoValidationByVolume = 3,
    }
}
