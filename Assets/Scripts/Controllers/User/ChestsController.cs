using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChestsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<Chests> GetChest(int userId, int chestId)
    {
        NetResult netResult = await NetChestServices.GetChest(userId, chestId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<Chests>(netResult.Response);
        else
            return null;
    }

    public async Task<List<Chests>> GetUserChests(int userId)
    {
        NetResult netResult = await NetChestServices.GetUserChests(userId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<Chests>>(netResult.Response);
        else
            return null;
    }

    public async Task<Chests> PostChest(int userId, Chests chest)
    {
        NetResult netResult = await NetChestServices.PostChest(userId, chest);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<Chests>(netResult.Response);
        else
            return null;
    }

    public async Task<bool> PutChest(int userId, int chestId, Chests chests)
    {
        NetResult netResult = await NetChestServices.PutChest(userId, chestId, chests);

        return (netResult.Status == EStatus.success);
    }

    public async Task<bool> PatchChest(int userId, int chestId, List<PatchData> patchData)
    {
        NetResult netResult = await NetChestServices.PatchChest(userId, chestId, patchData);

        return (netResult.Status == EStatus.success);
    }
}
