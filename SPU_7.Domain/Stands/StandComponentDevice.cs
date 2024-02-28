namespace SPU_7.Domain.Stands;

public class StandComponentDevice
{ 
    /// <summary>
    /// from 0 to 9
    /// </summary>
    public int BitNumber { get; set; }

    /// <summary>
    /// 0 or 1
    /// </summary>
    public bool Statement { get; set; }
}