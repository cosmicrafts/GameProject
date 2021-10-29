using System.Collections.Generic;
using System.Threading.Tasks;

public static class NetProgressServices
{
    public static async Task<NetResult> GetProgress(int userId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/progress");

        return result;
    }

    public static async Task<NetResult> PutProgress(int userId, Progress progress)
    {
        NetResult result = await NetClient.POST(progress,$"user/{userId}/progress");

        return result;
    }

    public static async Task<NetResult> PatchProgress(int userId, List<PatchData> patchData)
    {
        NetResult result = await NetClient.PATCH(patchData, $"user/{userId}/progress");

        return result;
    }
}
