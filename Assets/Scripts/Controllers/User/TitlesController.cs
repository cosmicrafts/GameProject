using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TitlesController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<Title> GetTitle(int userId, int titleId)
    {
        NetResult netResult = await NetTitlesServices.GetTitle(userId, titleId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<Title>(netResult.Response);
        else
            return null;
    }

    public async Task<List<Title>> GetUserTitles(int userId)
    {
        NetResult netResult = await NetTitlesServices.GetUserTitle(userId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<Title>>(netResult.Response);
        else
            return null;
    }
    public async Task<Title> PostTitle(int userId, Title title)
    {
        NetResult netResult = await NetTitlesServices.PostTitle(userId, title);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<Title>(netResult.Response);
        else
            return null;
    }

    public async Task<bool> PutTitle(int userId, int titleId, Title title)
    {
        NetResult netResult = await NetTitlesServices.PutTitle(userId, titleId, title);

        return (netResult.Status == EStatus.success);
    }

    public async Task<bool> PatchTitle(int userId, int titleId, List<PatchData> patchData)
    {
        NetResult netResult = await NetTitlesServices.PatchTitle(userId, titleId, patchData);

        return (netResult.Status == EStatus.success);
    }

}
