namespace SPU_7.Extensions;

public class LineInfoData
{
    public LineInfoData(int? lineIndex, bool isActive)
    {
        LineIndex = lineIndex;
        IsActive = isActive;
    }

    public int? LineIndex { get; set; }
    
    public bool IsActive { get; set; }
}