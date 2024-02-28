namespace SPU_7.Modbus.Extensions;

public static class ArrayExtensions
{
    /// <summary>
    /// Попарно меняет байты местами
    /// </summary>
    /// <param name="data">массив байт</param>
    public static byte[] SwapBytes(this byte[] data)
    {
        if (data.Length % 2 > 0) Array.Resize(ref data, data.Length + 1);
        for (var i = 0; i < data.Length; i += 2)
        {
            (data[i], data[i + 1]) = (data[i + 1], data[i]);
        }

        return data;
    }
}