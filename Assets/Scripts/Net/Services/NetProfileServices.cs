using System.Collections.Generic;
using System.Threading.Tasks;

public static class NetProfileServices
{
    public static async Task<NetResult> GetProfile(int userId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/profile");

        return result;
    }

    public static async Task<NetResult> PutProfile(int userId, Profile profile)
    {
        NetResult result = await NetClient.POST(profile, $"user/{userId}/profile");

        return result;
    }

    public static async Task<NetResult> PatchProfile(int userId, List<PatchData> patchData)
    {
        NetResult result = await NetClient.PATCH(patchData, $"user/{userId}/profile");

        return result;
    }
}
