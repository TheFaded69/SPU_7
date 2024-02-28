using System.ComponentModel;

namespace SPU_7.Common.Stand;

[Obsolete("Old enum")]
public enum JetJetValveType
{
    None = 0,

    [Description("0.04")]
    JetValve0_04 = 1,

    [Description("0.06")]
    JetValve0_06 = 2,

    [Description("0.15")]
    JetValve0_15 = 3,

    [Description("0.6")]
    JetValve0_6 = 4,

    [Description("1.0")]
    JetValve1_0 = 5,

    [Description("2.5")]
    JetValve2_5 = 6,

    [Description("4.0")]
    JetValve4_0 = 7,

    [Description("6.0")]
    JetValve6_0 = 8,

    [Description("10.0")]
    JetValve10_0 = 9,

    [Description("20.0")]
    JetValve20_0 = 10,
}