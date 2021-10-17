using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StarsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<Stars> GetStar(int userId, int starId)
    {
        NetResult netResult = await NetStarsServices.GetStars(userId, starId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<Stars>(netResult.Response);
        else
            return null;
    }

    public async Task<List<Stars>> GetUserStars(int userId)
    {
        NetResult netResult = await NetStarsServices.GetUserStars(userId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<Stars>>(netResult.Response);
        else
            return null;
    }

    public async Task<Stars> PostStars(int userId, Stars star)
    {
        NetResult netResult = await NetStarsServices.PostStars(userId, star);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<Stars>(netResult.Response);
        else
            return null;
    }

    public async Task<bool> PutStar(int userId, int starId, Stars star)
    {
        NetResult netResult = await NetStarsServices.PutStars(userId, starId, star);

        return (netResult.Status == EStatus.success);
    }

    public async Task<bool> PatchStar(int userId, int starId, List<PatchData> patchData)
    {
        NetResult netResult = await NetStarsServices.PatchStars(userId, starId, patchData);

        return (netResult.Status == EStatus.success);
    }
}
