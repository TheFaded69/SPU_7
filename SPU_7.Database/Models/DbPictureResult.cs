using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Models;

public class DbPictureResult : DbEntityGuid
{
    public DbOperationResult OperationResult { get; set; }
    
    public Guid OperationResultId { get; set; }
    
    public int DeviceNumber { get; set; }
    
    public int MeasureNumber { get; set; }
    
    public int PointNumber { get; set; }
    
    public byte[]? BitMapData { get; set; }  
}