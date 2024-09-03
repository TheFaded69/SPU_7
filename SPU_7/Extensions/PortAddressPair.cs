namespace SPU_7.Extensions;

public class PortAddressPair
{
    public PortAddressPair(string port, int address)
    {
        Port = port;
        Address = address;
    }

    public string Port { get; set; }
    
    public int Address { get; set; }
}