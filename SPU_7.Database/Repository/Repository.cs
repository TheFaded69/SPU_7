using Microsoft.EntityFrameworkCore;
using SPU_7.Database.DbContext;
using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Repository;

/// <summary>
/// Класс-репозиторий - фасад для доступа к базе данных
/// </summary>
/// <typeparam name="TModelType">тип модели БД</typeparam>
/// <typeparam name="TKeyType">тип PK</typeparam>
public sealed class Repository<TModelType, TKeyType> : IRepository<TModelType, TKeyType> where TModelType : DbEntity<TKeyType>
{
    public Repository(DataContext dataDbContext)
    {
        _dbContext = dataDbContext;
    }

    // public Repository(IDbContextFactory<DataContext> contextFactory)
    // {
    //     _dbContext = contextFactory.CreateDbContext();
    // }

    private readonly DataContext _dbContext;
    /// <summary>
    /// Контекст БД
    /// </summary>
    public IDataContext DbContext => _dbContext;

    /// <summary>
    /// Запрос для извлечения данных без отслеживания
    /// </summary>
    public IQueryable<TModelType> Query => _dbContext.Set<TModelType>().AsNoTracking();

    /// <summary>
    /// Запрос для извлечения данных с отслеживанием
    /// </summary>
    public IQueryable<TModelType> TrackingQuery => _dbContext.Set<TModelType>().AsTracking();

    public DbSet<TModelType> Set => _dbContext.Set<TModelType>();

    /// <summary>
    /// Начать транзакцию
    /// </summary>
    public Task BeginTransaction() => _dbContext.BeginTransactionAsync();

    /// <summary>
    /// Зафиксировать изменения (независимо от транзакции)
    /// </summary>
    public Task CommitAsync() => _dbContext.CommitAsync();

    /// <summary>
    /// Зафиксировать изменения (независимо от транзакции)
    /// </summary>
    public void Commit() => _dbContext.Commit();

    /// <summary>
    /// Откатить транзакцию
    /// </summary>
    public Task Rollback() => _dbContext.RollbackAsync();

    /// <summary>
    /// Вставить запись в БД
    /// </summary>
    /// <param name="obj">запись</param>
    public void Insert(TModelType obj)
    {
        if (obj.CreateTime == default) obj.CreateTime = DateTime.Now;
        var entry = _dbContext.Entry(obj);
        if (entry.State == EntityState.Detached) Set.Add(obj);
    }

    /// <summary>
    /// Получить одну запись по ключу PK
    /// </summary>
    /// <param name="key">ключ PK</param>
    /// <returns>запись</returns>
    public Task<TModelType> GetAsync(TKeyType key) => _dbContext.Set<TModelType>().FirstOrDefaultAsync(e => e.Id.Equals(key));

    /// <summary>
    /// Получить одну запись по ключу PK
    /// </summary>
    /// <param name="key">ключ PK</param>
    /// <returns>запись</returns>
    public TModelType Get(TKeyType key) => _dbContext.Set<TModelType>().FirstOrDefault(e => e.Id.Equals(key));

    /// <summary>
    /// Получить список записей по списку ключей PK
    /// </summary>
    /// <param name="keys">список ключей PK</param>
    /// <returns>список записей</returns>
    public IQueryable<TModelType> Get(IEnumerable<TKeyType> keys) => Query.Where(model => keys.Contains(model.Id));

    /// <summary>
    /// Удалить безвозвратно
    /// </summary>
    /// <param name="obj">запись</param>
    public void PermanentDelete(TModelType obj)
    {
        var entry = _dbContext.Entry(obj);
        if (entry.State == EntityState.Detached)
        {
            Set.Attach(obj);
        }

        Set.Remove(obj);
    }

    /// <summary>
    /// Удалить безвозвратно все записи
    /// </summary>
    public void PermanentDeleteAll() => Set.RemoveRange(Set);

    /// <summary>
    /// Пометить запись на удаление
    /// </summary>
    /// <param name="obj">запись</param>
    public void Delete(TModelType obj)
    {
        obj.Deleted = true;
        Update(obj);
    }

    /// <summary>
    /// Изменить запись
    /// </summary>
    /// <param name="obj">запись</param>
    public void Update(TModelType obj)
    {
        var entry = _dbContext.Entry(obj);
        if (entry.State == EntityState.Detached)
        {
            Set.Attach(obj);
        }
        entry.State = EntityState.Modified;
        _dbContext.Entry(obj).Property(p => p.CreateTime).IsModified = false;
    }

    public void Dispose() => _dbContext?.Dispose();
}