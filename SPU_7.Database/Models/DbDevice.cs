using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Models;

public class DbDevice: DbEntityGuid
{
    public Guid OperationResultId { get; set; }

    public DbOperationResult OperationResult { get; set; }
    
    public string VendorNumber { get; set; }
    
    public string DeviceName { get; set; }
    
    public string VendorName { get; set; }
    
    public string VendorAddress { get; set; }
}