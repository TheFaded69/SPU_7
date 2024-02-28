using SPU_7.Database.DbContext;
using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Repository;

/// <summary>
/// Основные методы репозитория
/// </summary>
public interface IRepository<TModelType, TKeyType> : IDisposable where TModelType : DbEntity<TKeyType>
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    IDataContext DbContext { get; }

    /// <summary>
    /// Запрос для извлечения данных без отслеживания
    /// </summary>
    IQueryable<TModelType> Query { get; }

    /// <summary>
    /// Запрос для извлечения данных с отслеживанием
    /// </summary>
    IQueryable<TModelType> TrackingQuery { get; }

    /// <summary>
    /// Начать транзакцию
    /// </summary>
    Task BeginTransaction();

    /// <summary>
    /// Зафиксировать изменения (независимо от транзакции)
    /// </summary>
    Task CommitAsync();

    /// <summary>
    /// Зафиксировать изменения (независимо от транзакции)
    /// </summary>
    void Commit();

    /// <summary>
    /// Откатить транзакцию
    /// </summary>
    Task Rollback();

    /// <summary>
    /// Вставить запись
    /// </summary>
    /// <param name="obj">запись</param>
    void Insert(TModelType obj);

    /// <summary>
    /// Получить одну запись по ключу PK
    /// </summary>
    /// <param name="key">ключ PK</param>
    /// <returns>запись</returns>
    Task<TModelType> GetAsync(TKeyType key);

    /// <summary>
    /// Получить одну запись по ключу PK
    /// </summary>
    /// <param name="key">ключ PK</param>
    /// <returns>запись</returns>
    TModelType Get(TKeyType key);

    /// <summary>
    /// Получить список записей по списку ключей PK
    /// </summary>
    /// <param name="keys">список ключей PK</param>
    /// <returns>список записей</returns>
    IQueryable<TModelType> Get(IEnumerable<TKeyType> keys);

    /// <summary>
    /// Удалить безвозвратно
    /// </summary>
    /// <param name="obj">запись</param>
    void PermanentDelete(TModelType obj);

    /// <summary>
    /// Удалить безвозвратно все записи
    /// </summary>
    void PermanentDeleteAll();

    /// <summary>
    /// Пометить запись на удаление
    /// </summary>
    /// <param name="obj">запись</param>
    void Delete(TModelType obj);

    /// <summary>
    /// Изменить запись
    /// </summary>
    /// <param name="obj">запись</param>
    void Update(TModelType obj);
}