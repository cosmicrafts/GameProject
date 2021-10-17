using System.Collections.Generic;
using System.Threading.Tasks;

public static class NetTitlesServices
{
    public static async Task<NetResult> PostTitle(int userId, Title title)
    {
        NetResult result = await NetClient.POST(title, $"user/{userId}/Title");

        return result;
    }

    public static async Task<NetResult> GetTitle(int userId, int titleId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/Title/{titleId}");

        return result;
    }

    public static async Task<NetResult> GetUserTitle(int userId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/Title");

        return result;
    }

    public static async Task<NetResult> PutTitle(int userId, int titleId, Title title)
    {
        NetResult result = await NetClient.POST(title, $"user/{userId}/Title/{titleId}");

        return result;
    }

    public static async Task<NetResult> PatchTitle(int userId, int titleId, List<PatchData> patchData)
    {
        NetResult result = await NetClient.PATCH(patchData, $"user/{userId}/Title/{titleId}");

        return result;
    }
}
