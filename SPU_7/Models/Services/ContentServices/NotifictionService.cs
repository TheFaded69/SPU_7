using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace SPU_7.Models.Services.ContentServices
{
    public class NotificationService : INotificationService
    {
        private int _notificationTimeout = 5;
        private WindowNotificationManager _notificationManager;

        public int NotificationTimeout
        {
            get => _notificationTimeout;
            set
            {
                _notificationTimeout = value < 0 ? 0 : value;
            }
        }

        /// <summary>Set the host window.</summary>
        /// <param name="hostWindow">Parent window.</param>
        public void SetHostWindow(TopLevel hostWindow)
        {
            WindowNotificationManager notificationManager = new(hostWindow)
            {

                Position = NotificationPosition.BottomRight,
                MaxItems = 5,
                Margin = new Thickness(0, 0, 20, 20)
            };

            _notificationManager = notificationManager;
        }

        /// <summary>Display the notification.</summary>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="onClick">Optional OnClick action.</param>
        public void Show(string title, string message, Action onClick = null)
        {
            if (_notificationManager is { } nm)
            {
                nm.Show(
                    new Notification(
                    title,
                    message,
                    NotificationType.Information,
                    TimeSpan.FromSeconds(_notificationTimeout),
                    onClick));
            }
        }
    }
}
