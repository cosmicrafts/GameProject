using System.Collections.Generic;
using System.Threading.Tasks;

public static class NetInventoryServices
{
    public static async Task<NetResult> GetInventory(int userId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/inventory");

        return result;
    }

    public static async Task<NetResult> PutInventory(int userId, Inventory inventory)
    {
        NetResult result = await NetClient.POST(inventory, $"user/{userId}/inventory");

        return result;
    }

    public static async Task<NetResult> PatchInventory(int userId, List<PatchData> patchData)
    {
        NetResult result = await NetClient.PATCH(patchData, $"user/{userId}/inventory");

        return result;
    }
}
