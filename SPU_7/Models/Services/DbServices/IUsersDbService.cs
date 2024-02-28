using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SPU_7.Database.Models;

namespace SPU_7.Models.Services.DbServices
{
    /// <summary>
    /// Сервис для работы с таблицей пользователей в БД
    /// </summary>
    public interface IUsersDbService
    {
        /// <summary>
        /// Получить всех пользователей из БД
        /// </summary>
        /// <returns>Список полизователей</returns>
        List<User> GetAllUsers();

        /// <summary>
        /// Получить всех пользователей из БД (асинхронно)
        /// </summary>
        /// <returns>Список полизователей</returns>
        Task<List<User>> GetAllUsersAsync();

        /// <summary>
        /// Получить пользователя из БД
        /// </summary>
        /// <returns>Пользователь</returns>
        User GetUser(Guid id);

        /// <summary>
        /// Получить пользователя из БД (асинхронно)
        /// </summary>
        /// <returns>Пользователь</returns>
        Task<User> GetUserAsync(Guid id);

        /// <summary>
        /// Поиск ID пользователя по нику
        /// </summary>
        /// <returns>ID</returns>
        Guid FindUser(string username);

        /// <summary>
        /// Поиск ID пользователя по нику (асинхронно)
        /// </summary>
        /// <returns>ID</returns>
        Task<Guid> FindUserAsync(string username);

        /// <summary>
        /// Сравнение паролей пользователя в БД и введенного
        /// </summary>
        /// <returns>Результат сравнения</returns>
        bool IsEqualPassword(Guid id, string password);

        /// <summary>
        /// Сравнение паролей пользователя в БД и введенного (асинхронно)
        /// </summary>
        /// <returns>Результат сравнения</returns>
        Task<bool> IsEqualPasswordAsync(Guid id, string password);

        /// <summary>
        /// Добавить пользователя в БД
        /// </summary>
        /// <returns>Результат добавления</returns>
        bool AddUser(User user);

        /// <summary>
        /// Добавить пользователя в БД (асинхронно)
        /// </summary>
        /// <returns>Результат добавления</returns>
        Task<bool> AddUserAsync(User user);

        /// <summary>
        /// Удалить пользователя в БД
        /// </summary>
        /// <returns>Результат удаления</returns>
        bool DeleteUser(Guid id);

        /// <summary>
        /// Удалить пользователя в БД (асинхронно)
        /// </summary>
        /// <returns>Результат удаления</returns>
        Task<bool> DeleteUserAsync(Guid id);

        /// <summary>
        /// Найти пользователя с запоминалкой
        /// </summary>
        /// <returns>ID пользователя</returns>
        Task<Guid> FindAutoUserAsync();
        
        /// <summary>
        /// Найти пользователя с запоминалкой
        /// </summary>
        /// <returns>ID пользователя</returns>
        Guid FindAutoUser();

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        void UpdateUser(DbUsers user);
        
        /// <summary>
        /// Обновить пользователя (асинхронно)
        /// </summary>
        /// <param name="users">Пользователь</param>
        Task UpdateUserAsync(DbUsers users);

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="isRememberUser">Запомнить ли пользователя</param>
        void UpdateAutoUser(Guid id, bool isRememberUser);

        /// <summary>
        /// Обновить пользователя (асинхронно)
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="isRememberUser">Запомнить ли пользователя</param>
        Task UpdateAutoUserAsync(Guid id, bool isRememberUser);
    }
}
