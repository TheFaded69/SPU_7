using System.ComponentModel;

namespace SPU_7.Common.Scripts
{
    public enum ScriptType
    {
        None = 0,

        [Description("Калибровка")]
        Calibration = 1,

        [Description("Поверка")]
        Validation = 2,

        [Description("Программирование")]
        Programming = 3,

        [Description("Герметичность")]
        Germetic = 4,

        [Description("Работоспособность")]
        Workable = 5,

    }
}
