using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class XpBuffController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<XpBuffs> GetXpBuffActive(int userId)
    {
        NetResult netResult = await NetXpBuffServices.GetXpBuffActive(userId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<XpBuffs>(netResult.Response);
        else
            return null;
    }

    public async Task<XpBuffs> PostXpBuffs(int userId, XpBuffs xpBuffs)
    {
        NetResult netResult = await NetXpBuffServices.PostXpBuff(userId, xpBuffs);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<XpBuffs>(netResult.Response);
        else
            return null;
    }
}
