using System.Threading.Tasks;
using SPU_7.Models.Services.DbServices;

namespace SPU_7.Models.Services.Autorization
{
    public interface IAuthorizationService
    {
        /// <summary>
        /// Авторизован ли пользователь
        /// </summary>
        /// <returns>true - да, false - нет</returns>
        bool IsAuthorize();

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="userName">Псевдоним</param>
        /// <param name="userPassword">Пароль</param>
        /// <returns>Получилось ли авторизироваться</returns>
        Task<bool> AuthorizeAsync(string userName, string userPassword);
        
        /// <summary>
        /// Попытка автоматической авторизации, если был сохранен пользователь
        /// </summary>
        /// <returns>Получилось ли авторизироваться</returns>
        bool AutoAuthorize();

        /// <summary>
        /// Получить текущего пользователя
        /// </summary>
        /// <returns>Пользователь</returns>
        User GetUser();

        /// <summary>
        /// Выйти из учетной записи пользователя
        /// </summary>
        /// <returns></returns>
        void ExitUser();
        
        /// <summary>
        /// Обновить запоминание пользователя
        /// </summary>
        /// <param name="isRememberUser"></param>
        /// <returns></returns>
        Task UpdateAutoUser(bool isRememberUser);
    }
}
