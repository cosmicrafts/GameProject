using System.Runtime.InteropServices;

public static class GameNetwork
{
    [DllImport("__Internal")]
    public static extern void SendJson(string json);
}
