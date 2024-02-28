using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPU_7.Common.Stand;
using SPU_7.Database.Models;
using SPU_7.Database.Repository;
using SPU_7.Models.Services.Logger;

namespace SPU_7.Models.Services.DbServices
{
    public class UsersDbService : IUsersDbService
    {
        public UsersDbService(IMapper mapper, ILogger logger, IRepositoryCreator<DbUsers, Guid> repositoryCreator)
        {
            _repositoryCreator = repositoryCreator;
            _mapper = mapper;
            _logger = logger;
            
        }
        private readonly IRepositoryCreator<DbUsers, Guid> _repositoryCreator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;


        public bool AddUser(User user)
        {
            try
            {
                var dbUser = _mapper.Map<DbUsers>(user);
                var repository = _repositoryCreator.CreateRepository();
                repository.Insert(dbUser);
                repository.Commit();

                return true;

            }
            catch (Exception e)
            {
                _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
                throw;
            }
        }

        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                var dbUser = _mapper.Map<DbUsers>(user);
                var repository = await _repositoryCreator.CreateRepositoryAsync();
                repository.Insert(dbUser);
                await repository.CommitAsync();

                return true;

            }
            catch (Exception e)
            {
                _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
                throw;
            }
        }

        public bool DeleteUser(Guid id)
        {
            try
            {
                var repository = _repositoryCreator.CreateRepository();
                var existUser = repository.Get(id) ?? throw new Exception($"Такого пользователя [{id}] не существует");
                repository.Delete(existUser);
                repository.Commit();
                return true;
            }
            catch (Exception e)
            {
                _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            try
            {
                var repository = await _repositoryCreator.CreateRepositoryAsync();
                var existUser = await repository.GetAsync(id) ?? throw new Exception($"Такого пользователя [{id}] не существует");
                repository.Delete(existUser);
                await repository.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
                return false;
            }
        }

        public Guid FindUser(string username)
        {
            try
            {
                var repository = _repositoryCreator.CreateRepository();
                var user = repository
                    .Query
                    .FirstOrDefault(user => !user.Deleted && user.UserName.Equals(username));

                return user != null ? user.Id : Guid.Empty;
            }
            catch (Exception e)
            {
                _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
                throw;
            }
        }

        public async Task<Guid> FindUserAsync(string username)
        {
            try
            {
                var repository = await _repositoryCreator.CreateRepositoryAsync();
                var user = await repository
                    .Query
                    .Where(user => !user.Deleted && user.UserName.Equals(username))
                    .FirstOrDefaultAsync();

                return user != null ? user.Id : Guid.Empty;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                var repository = _repositoryCreator.CreateRepository();
                var users = repository
                    .Query
                    .Where(user => !user.Deleted)
                    .ToList();

                return _mapper.Map<List<User>>(users);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var repository = await _repositoryCreator.CreateRepositoryAsync();
                var users = await repository
                    .Query
                    .Where(user => !user.Deleted)
                    .ToListAsync();

                return _mapper.Map<List<User>>(users);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public User GetUser(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return null;

                var repository = _repositoryCreator.CreateRepository();
                var dbUser = repository
                    .Query
                    .FirstOrDefault(user => !user.Deleted && user.Id.Equals(id));

                return _mapper.Map<User>(dbUser);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<User> GetUserAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return null;

                var repository = await _repositoryCreator.CreateRepositoryAsync();
                var dbUser = await repository
                    .Query
                    .Where(user => !user.Deleted && user.Id.Equals(id))
                    .FirstOrDefaultAsync();

                return _mapper.Map<User>(dbUser);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        
        public async Task<Guid> FindAutoUserAsync()
        {
            try
            {
                var repository = await _repositoryCreator.CreateRepositoryAsync();
                var user = await repository
                    .Query
                    .Where(user => user.ProgramType == ProgramType.UniversalStand)
                    .FirstOrDefaultAsync(user => user.IsRememberUser);

                return user != null ? user.Id : Guid.Empty;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public Guid FindAutoUser()
        {
            try
            {
                var repository =  _repositoryCreator.CreateRepository();
                var user =  repository
                    .Query
                    .Where(user => user.ProgramType == ProgramType.UniversalStand)
                    .FirstOrDefault(user => user.IsRememberUser);

                return user != null ? user.Id : Guid.Empty;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public void UpdateUser(DbUsers user)
        {
            try
            {
                var repository = _repositoryCreator.CreateRepository();
                
                repository.Update(user);
                repository.Commit();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task UpdateUserAsync(DbUsers user)
        {
            try
            {
                var repository = await _repositoryCreator.CreateRepositoryAsync();
                
                repository.Update(user);
                await repository.CommitAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void UpdateAutoUser(Guid id, bool isRememberUser)
        {
            try
            {
                var repository = _repositoryCreator.CreateRepository();

                var dbUser = repository
                    .Query
                    .FirstOrDefault(db => db.Id == id);

                if (dbUser == null) return;
                dbUser.IsRememberUser = isRememberUser;
                repository.Update(dbUser);
                repository.Commit();

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task UpdateAutoUserAsync(Guid id, bool isRememberUser)
        {
            try
            {
                var repository = await _repositoryCreator.CreateRepositoryAsync();

                var dbUser = await repository
                    .Query
                    .FirstOrDefaultAsync(db => db.Id == id);

                if (dbUser != null)
                {
                    dbUser.IsRememberUser = isRememberUser;
                    repository.Update(dbUser);
                    await repository.CommitAsync();

                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool IsEqualPassword(Guid id, string password)
        {
            try
            {
                if (id == Guid.Empty) return false;

                var repository = _repositoryCreator.CreateRepository();
                var dbUser = repository
                    .Query
                    .FirstOrDefault(user => !user.Deleted && user.Id.Equals(id));

                return dbUser?.Password == password;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<bool> IsEqualPasswordAsync(Guid id, string password)
        {
            try
            {
                if (id == Guid.Empty) return false;

                var repository = await _repositoryCreator.CreateRepositoryAsync();
                var dbUser = await repository
                    .Query
                    .Where(user => !user.Deleted && user.Id.Equals(id))
                    .FirstOrDefaultAsync();

                return dbUser?.Password == password;
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
