
using System.Text;

namespace SPU_7.DeviceCommunication.Extensions;

public static class MemoryExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TAccumulate"></typeparam>
    /// <param name="source"></param>
    /// <param name="seed"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static TAccumulate Aggregate<TSource, TAccumulate>(this ReadOnlySpan<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
    {
        if (source == null) {
            throw new ArgumentNullException(nameof(source));
        }

        if (func == null) {
            throw new ArgumentNullException(nameof(func));
        }

        TAccumulate result = seed;
        foreach (TSource element in source) {
            result = func(result, element);
        }

        return result;
    }

    public static TAccumulate Aggregate<TSource, TAccumulate>(this Span<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
    {
        if (source == null) {
            throw new ArgumentNullException(nameof(source));
        }

        if (func == null) {
            throw new ArgumentNullException(nameof(func));
        }

        TAccumulate result = seed;
        foreach (TSource element in source) {
            result = func(result, element);
        }

        return result;
    }

    /// <summary>
    /// Инвертировать порядок в байтовом сегменте массива
    /// </summary>
    /// <param name="bytesSegment">Сегмент байтового массива</param>
    /// <returns>Ссылка на тот же сегмент</returns>
    public static ArraySegment<byte> ReverseSegment(this ArraySegment<byte> bytesSegment)
    {
        if (bytesSegment.Array is null) throw new InvalidOperationException("InvalidOperation_NullArray");
        Array.Reverse(bytesSegment.Array, bytesSegment.Offset, bytesSegment.Count);
        return bytesSegment;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytesSegment"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static int IndexOf(this ArraySegment<byte> bytesSegment, byte value)
    {
        if (bytesSegment.Array is null) throw new InvalidOperationException("InvalidOperation_NullArray");
        return Array.IndexOf(bytesSegment.Array, value, bytesSegment.Offset, bytesSegment.Count);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytesSegment"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetEncodedString(this ArraySegment<byte> bytesSegment, Encoding encoding)
    {
        if (bytesSegment.Array is null) throw new InvalidOperationException("InvalidOperation_NullArray");
        var zeroIndex = Array.FindIndex(bytesSegment.Array, bytesSegment.Offset, v => v == 0);
        return encoding.GetString(bytesSegment.Array, bytesSegment.Offset, zeroIndex);
    }
}