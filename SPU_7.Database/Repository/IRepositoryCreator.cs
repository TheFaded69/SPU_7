using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Repository;

public interface IRepositoryCreator<TModelType, TKeyType> where TModelType : DbEntity<TKeyType>
{
    Task<Repository<TModelType, TKeyType>> CreateRepositoryAsync();
    Repository<TModelType, TKeyType> CreateRepository();
}