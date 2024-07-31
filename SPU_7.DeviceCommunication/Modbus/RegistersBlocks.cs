namespace SPU_7.DeviceCommunication.Modbus;
public class RegistersBlocks //: IEnumerable<(ushort, IList<byte>)>
{
    public RegistersBlocks(IList<Register> registers)
    {
        Registers = registers;
    }

    private IList<Register> Registers { get; }

    /// <summary>
    /// Получить перечислитель блоков регистров
    /// </summary>
    public IEnumerator<(ushort, IList<Register>)> GetEnumerator()
    {
        var firstRegister = Registers.First();
        var startAddress = firstRegister.Configuration.Address;
        var nextAddress = firstRegister.Configuration.Address;
        var registersCount = 0;
        var registers = new List<Register>();
        foreach (var register in Registers) {
            if (register.Configuration.Address != nextAddress) { // Если следующий адрес не находится в соседнем местоположении
                yield return (startAddress, registers); // Вернуть итератор вызывающему
                // Сбросить подсчёт и накопленные данные
                registers = new List<Register>();
                startAddress = register.Configuration.Address;
                nextAddress = register.Configuration.Address;
            }
            registersCount += register.Configuration.NumberOfRegisters;
            if (registersCount > 125) { // Если превышено количество регистров в одном запросе
                yield return (startAddress, registers); // Вернуть итератор вызывающему
                // Сбросить подсчёт и накопленные данные
                registers = new List<Register>();
                registersCount = register.Configuration.NumberOfRegisters;
                startAddress = register.Configuration.Address;
                nextAddress = register.Configuration.Address;
            }
            registers.Add(register);
            nextAddress += register.Configuration.NumberOfRegisters;
        }
        yield return (startAddress, registers); // Вернуть последний доступный элемент
    }

    /*IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }*/
}