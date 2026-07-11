namespace DcsSymlinker;


public class DeviceIdService : IDeviceIdService
{
    public Dictionary<string, List<string>> GetPossibleIds(string baseId, int count)
    {
        var x = Guid.Parse("9E573ED6-7734-11d2-8D4A-23903FB6BDF7");
        // x.Variant
        for (int i = 0; i < count; i++)
        {
            
        }

        return new Dictionary<string, List<string>>();
    }

    // private (string Hex) GetIdParts
}

public interface IDeviceIdService
{
    Dictionary<string, List<string>> GetPossibleIds(string baseId, int count);
}