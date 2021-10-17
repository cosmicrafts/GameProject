using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ProgressController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<Progress> GetProgress(int userId)
    {
        NetResult netResult = await NetProgressServices.GetProgress(userId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<Progress>(netResult.Response);
        else
            return null;
    }

    public async Task<bool> PutProgress(int userId, Progress progress)
    {
        NetResult netResult = await NetProgressServices.PutProgress(userId, progress);

        return (netResult.Status == EStatus.success);
    }

    public async Task<bool> PatchProgress(int userId, List<PatchData> patchData)
    {
        NetResult netResult = await NetProgressServices.PatchProgress(userId, patchData);

        return (netResult.Status == EStatus.success);
    }
}
