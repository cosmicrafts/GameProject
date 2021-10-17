using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TypeXpBuffsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<TypeXpBuff> GetTypeXpBuff(int typexpbuff)
    {
        NetResult netResult = await NetXpBuffServices.GetTypeXpBuff(typexpbuff);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<TypeXpBuff>(netResult.Response);
        else
            return null;
    }

    public async Task<List<TypeXpBuff>> GetAllTypeXpBuff()
    {
        NetResult netResult = await NetXpBuffServices.GetAllTypeXpBuff();

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<TypeXpBuff>>(netResult.Response);
        else
            return null;
    }
}
