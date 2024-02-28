using System.ComponentModel;

namespace SPU_7.Common.Settings
{
    public enum ProcessorType
    {
        None = 0,

        [Description("PIC16LF1827")]
        PIC16LF1827 = 1,

        [Description("PIC16LF1847")]
        PIC16LF1847 = 2
    }
}
