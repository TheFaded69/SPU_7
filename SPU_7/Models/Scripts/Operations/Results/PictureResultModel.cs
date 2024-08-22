namespace SPU_7.Models.Scripts.Operations.Results;

public class PictureResultModel
{
    public int DeviceNumber { get; set; }
    
    public int MeasureNumber { get; set; }
    
    public int PointNumber { get; set; }
    
    public byte[]? BitMapData { get; set; }  
}