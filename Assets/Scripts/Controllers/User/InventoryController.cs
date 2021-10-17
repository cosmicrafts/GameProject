using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<Inventory> GetInventory(int userId)
    {
        NetResult netResult = await NetInventoryServices.GetInventory(userId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<Inventory>(netResult.Response);
        else
            return null;
    }

    public async Task<bool> PutInventory(int userId, Inventory inventory)
    {
        NetResult netResult = await NetInventoryServices.PutInventory(userId, inventory);

        return (netResult.Status == EStatus.success);
    }

    public async Task<bool> PatchInventory(int userId, List<PatchData> patchData)
    {
        NetResult netResult = await NetInventoryServices.PatchInventory(userId, patchData);

        return (netResult.Status == EStatus.success);
    }
}
