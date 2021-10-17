using System.Collections.Generic;
using System.Threading.Tasks;

public static class NetStarsServices
{
    public static async Task<NetResult> PostStars(int userId, Stars stars)
    {
        NetResult result = await NetClient.POST(stars, $"user/{userId}/Star");

        return result;
    }

    public static async Task<NetResult> GetStars(int userId, int starId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/Star/{starId}");

        return result;
    }

    public static async Task<NetResult> GetUserStars(int userId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/Star");

        return result;
    }

    public static async Task<NetResult> PutStars(int userId, int starId, Stars stars)
    {
        NetResult result = await NetClient.POST(stars, $"user/{userId}/Star/{starId}");

        return result;
    }

    public static async Task<NetResult> PatchStars(int userId, int starId, List<PatchData> patchData)
    {
        NetResult result = await NetClient.PATCH(patchData, $"user/{userId}/Star/{starId}");

        return result;
    }
}
