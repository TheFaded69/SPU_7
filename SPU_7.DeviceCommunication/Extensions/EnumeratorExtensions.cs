using System.Collections;

namespace SPU_7.DeviceCommunication.Extensions;
public static class EnumeratorExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerator"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static T[] TakeToArray<T>(this IEnumerator enumerator, int count)
    {
        var al = new T[count];
        var offset = 0;
        while(offset < count && enumerator.MoveNext()) {
            al[offset++] = (T)enumerator.Current;
        }
        return al;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public static T? TakeValue<T>(this IEnumerator enumerator) => (T?)(enumerator.MoveNext() ? enumerator.Current : default);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerator"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool TakeValue<T>(this IEnumerator enumerator, out T? value)
    {
        var result = enumerator.MoveNext();
        value = (T?)(result ? enumerator.Current : default);
        return result;
    }
}