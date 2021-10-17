using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AchievementController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<TypeAchievement> GetTypeAchievement(int typeachivId)
    {
        NetResult netResult = await NetAchievementServices.GetTypeAchievement(typeachivId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<TypeAchievement>(netResult.Response);
        else
            return null;
    }

    public async Task<List<TypeAchievement>> GetAllTypeAchievement()
    {
        NetResult netResult = await NetAchievementServices.GetAllTypeAchievement();

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<TypeAchievement>>(netResult.Response);
        else
            return null;
    }
}
