namespace SPU_7.Extensions;

public class LineData
{
    public LineData(int lineIndex, object? data)
    {
        LineIndex = lineIndex;
        Data = data;
    }
    
    public object? Data { get; set; }
    
    public int LineIndex { get; set; }
}