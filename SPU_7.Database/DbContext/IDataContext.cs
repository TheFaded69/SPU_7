namespace SPU_7.Database.DbContext;

public interface IDataContext
{
    /// <summary>
    /// Начать транзакцию
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Принять изменения транзакции, или сохранить изменения,
    /// если транзакция не была запущена
    /// </summary>
    Task CommitAsync();

    /// <summary>
    /// Принять изменения транзакции, или сохранить изменения,
    /// если транзакция не была запущена
    /// </summary>
    public void Commit();

    /// <summary>
    /// Откат транзакции
    /// </summary>
    Task RollbackAsync();
}