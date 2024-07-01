using AForge.Video.DirectShow;

namespace SPU_7.Common.Extensions;

public static class FilterInfoCollectionExtensions
{
    public static List<string> GetMonikerNames(this FilterInfoCollection filterInfoCollection)
    {
        return (from FilterInfo filterInfo in filterInfoCollection select filterInfo.MonikerString).ToList();
    }
    
    public static List<string> GetNames(this FilterInfoCollection filterInfoCollection)
    {
        return (from FilterInfo filterInfo in filterInfoCollection select filterInfo.Name).ToList();
    }
}