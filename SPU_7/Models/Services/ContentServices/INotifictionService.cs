using System;
using Avalonia.Controls;

namespace SPU_7.Models.Services.ContentServices
{
    /// <summary>
    /// Сервис для создания уведомлений
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Время отображения уведомления
        /// </summary>
        int NotificationTimeout { get; set; }

        /// <summary>
        /// Установление окна, над которым будут отображаться уведомления
        /// </summary>
        /// <param name="window">Уровень окна (от 0 до +...)</param>
        void SetHostWindow(TopLevel window);

        /// <summary>
        /// Показатель уведомление
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="message">Текст уведомления</param>
        /// <param name="onClick">Действие при нажатии на уведомление (по умолчению null)</param>
        void Show(string title, string message, Action onClick = null);
    }
}
