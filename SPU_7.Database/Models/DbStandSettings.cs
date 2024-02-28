using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Models;

public class DbStandSettings : DbEntityGuid
{
    public string StandSettings { get; set; }
    
    public int StandNumber { get; set; }
    
    public string ProfileName { get; set; }
}