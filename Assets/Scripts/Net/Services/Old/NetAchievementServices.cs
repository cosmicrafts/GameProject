using System.Threading.Tasks;

public static class NetAchievementServices
{
    public static async Task<NetResult> GetTypeAchievement(int typeachivId)
    {
        NetResult result = await NetClient.GET($"Types/Achievement/{typeachivId}");

        return result;
    }

    public static async Task<NetResult> GetAllTypeAchievement()
    {
        NetResult result = await NetClient.GET($"Types/Achievement");

        return result;
    }
}
