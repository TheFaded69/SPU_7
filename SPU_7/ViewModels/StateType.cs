namespace SPU_7.ViewModels;

public enum StateType
{
    None = 0,
    
    /// <summary>
    /// Клапан открыт
    /// </summary>
    Open = 1,
    
    /// <summary>
    /// Клапан закрыт
    /// </summary>
    Close = 2,
    
    /// <summary>
    /// Клапан открывается\закрывается
    /// </summary>
    Work = 3
}