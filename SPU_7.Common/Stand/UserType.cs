using System.ComponentModel;

namespace SPU_7.Common.Stand
{
    public enum UserType
    {
        None = 0,

        [Description("Обычный пользователь")]
        Common = 1,

        [Description("Администратор")]
        Admin = 2,

        [Description("Разработчик")]
        Developer = 3,
    }
}
