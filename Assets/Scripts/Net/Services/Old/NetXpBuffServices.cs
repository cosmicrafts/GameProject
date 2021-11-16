using System.Threading.Tasks;

public static class NetXpBuffServices 
{
    public static async Task<NetResult> PostXpBuff(int userId, XpBuffs xpbuff)
    {
        NetResult result = await NetClient.POST(xpbuff, $"user/{userId}/XpBuff");

        return result;
    }

    public static async Task<NetResult> GetXpBuffActive(int userId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/XpBuff");

        return result;
    }

    public static async Task<NetResult> GetTypeXpBuff(int typexpbuffId)
    {
        NetResult result = await NetClient.GET($"Types/XpBuff/{typexpbuffId}");

        return result;
    }

    public static async Task<NetResult> GetAllTypeXpBuff()
    {
        NetResult result = await NetClient.GET($"Types/XpBuff");

        return result;
    }
}
