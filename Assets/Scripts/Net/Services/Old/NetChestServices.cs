using System.Collections.Generic;
using System.Threading.Tasks;

public static class NetChestServices
{
    public static async Task<NetResult> PostChest(int userId, Chests chests)
    {
        NetResult result = await NetClient.POST(chests,$"user/{userId}/Chest");

        return result;
    }

    public static async Task<NetResult> GetChest(int userId, int chestId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/Chest/{chestId}");

        return result;
    }

    public static async Task<NetResult> GetUserChests(int userId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/Chest");

        return result;
    }

    public static async Task<NetResult> PutChest(int userId, int chestId, Chests chests)
    {
        NetResult result = await NetClient.POST(chests, $"user/{userId}/Chest/{chestId}");

        return result;
    }

    public static async Task<NetResult> PatchChest(int userId, int chestId, List<PatchData> patchData)
    {
        NetResult result = await NetClient.PATCH(patchData, $"user/{userId}/Chest/{chestId}");

        return result;
    }

    public static async Task<NetResult> GetTypeChest(int typechestId)
    {
        NetResult result = await NetClient.GET($"Types/Chest/{typechestId}");

        return result;
    }

    public static async Task<NetResult> GetAllTypeChest()
    {
        NetResult result = await NetClient.GET($"Types/Chest");

        return result;
    }
}
