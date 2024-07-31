using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPU_7.DeviceCommunication.Base;

/// <summary>
/// Реализация <see cref="INotifyPropertyChanged"/> для упрощения View моделей.
/// </summary>
public abstract class BindableBase : INotifyPropertyChanged
{
    /// <summary>
    /// Событие, которое происходит после изменения свойства.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Проверяет свойство - содержит ли то же самое значение и оповещает подписавшихся на событие только при его изменении.
    /// </summary>
    /// <typeparam name="T">Тип свойства.</typeparam>
    /// <param name="storage">Ссылка на свойство с getter и setter.</param>
    /// <param name="value">Желаемое значение свойства.</param>
    /// <param name="propertyName">Название свойства используемое для оповещения слушающих событие изменения значения.
    /// Это значение опционально и автоматически подставляется компилятором, который поддерживает атрибут <see cref="CallerMemberNameAttribute"/>.</param>
    /// <returns>True если значение изменено, false если значение совпадает с текущим.</returns>
    protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;
        storage = value;
        RaisePropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// Проверяет свойство - содержит ли то же самое значение и оповещает подписавшихся на событие только при его изменении.
    /// </summary>
    /// <typeparam name="T">Тип свойства.</typeparam>
    /// <param name="storage">Ссылка на свойство с getter и setter.</param>
    /// <param name="value">Желаемое значение свойства.</param>
    /// <param name="propertyName">Название свойства используемое для оповещения слушающих событие изменения значения.
    /// Это значение опционально и автоматически подставляется компилятором, который поддерживает атрибут <see cref="CallerMemberNameAttribute"/>.</param>
    /// <param name="onChanged">Действие которое вызывается если значение свойства было изменено на желаемое.</param>
    /// <returns>True если значение изменено, false если значение совпадает с текущим.</returns>
    protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;
        storage = value;
        onChanged?.Invoke();
        RaisePropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// Проверяет свойство - содержит ли то же самое значение и оповещает подписавшихся на событие только при его изменении.
    /// </summary>
    /// <typeparam name="T">Тип свойства.</typeparam>
    /// <param name="storage">Ссылка на свойство с getter и setter.</param>
    /// <param name="value">Желаемое значение свойства.</param>
    /// <param name="propertyName">Название свойства используемое для оповещения слушающих событие изменения значения.
    /// Это значение опционально и автоматически подставляется компилятором, который поддерживает атрибут <see cref="CallerMemberNameAttribute"/>.</param>
    /// <param name="beforeChanged">Действие которое вызывается если значение свойства подлежит замене</param>
    /// <param name="onChanged">Действие которое вызывается если значение свойства было изменено на желаемое.</param>
    /// <returns>True если значение изменено, false если значение совпадает с текущим.</returns>
    protected virtual bool SetProperty<T>(ref T storage, T value, Action beforeChanged, Action onChanged, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;
        beforeChanged?.Invoke();
        storage = value;
        onChanged?.Invoke();
        RaisePropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// Вызывает событие PropertyChanged для текущего объекта.
    /// </summary>
    /// <param name="propertyName">Название свойства использемое для оповещения слушателей события.
    /// Это значение опционально и автоматически подставляется компилятором, который поддерживает атрибут <see cref="CallerMemberNameAttribute"/>.</param>
    protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Вызывает событие PropertyChanged для текущего объекта.
    /// </summary>
    /// <param name="args">Аргументы события PropertyChanged</param>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        PropertyChanged?.Invoke(this, args);
    }
}