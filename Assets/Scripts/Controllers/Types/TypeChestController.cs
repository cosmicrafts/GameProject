using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TypeChestController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<TypeChest> GetTypeChest(int typechestId)
    {
        NetResult netResult = await NetChestServices.GetTypeChest(typechestId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<TypeChest>(netResult.Response);
        else
            return null;
    }

    public async Task<List<TypeChest>> GetAllTypeChest()
    {
        NetResult netResult = await NetChestServices.GetAllTypeChest();

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<TypeChest>>(netResult.Response);
        else
            return null;
    }
}
