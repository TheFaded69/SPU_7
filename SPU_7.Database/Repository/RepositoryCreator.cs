using Microsoft.EntityFrameworkCore;
using SPU_7.Database.DbContext;
using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Repository;

public class RepositoryCreator<TModelType, TKeyType> : IRepositoryCreator<TModelType, TKeyType> where TModelType : DbEntity<TKeyType>
{
    public RepositoryCreator(IDbContextFactory<DataContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    private readonly IDbContextFactory<DataContext> _contextFactory;

    public async Task<Repository<TModelType, TKeyType>> CreateRepositoryAsync() => new(await _contextFactory.CreateDbContextAsync());
    public Repository<TModelType, TKeyType> CreateRepository() => new(_contextFactory.CreateDbContext());
}