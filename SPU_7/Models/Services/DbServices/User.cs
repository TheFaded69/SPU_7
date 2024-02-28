using SPU_7.Common.Stand;

namespace SPU_7.Models.Services.DbServices
{
    /// <summary>
    /// Класс пользователя
    /// </summary>
    public class User
    {
        /// <summary>
        /// Псевдоним
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string Employee { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Тип пользователя
        /// </summary>
        public UserType UserType { get; set; }
        
        /// <summary>
        /// Тип программы
        /// </summary>
        public ProgramType ProgramType { get; set; }

        /// <summary>
        /// Запомнить ли пользователя дя автоматической авторизации
        /// </summary>
        public bool IsRememberUser { get; set; }
    }
}
