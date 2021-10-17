using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ProfileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<Profile> GetProfile(int userId)
    {
        NetResult netResult = await NetProfileServices.GetProfile(userId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<Profile>(netResult.Response);
        else
            return null;
    }

    public async Task<bool> PutProfile(int userId, Profile profile)
    {
        NetResult netResult = await NetProfileServices.PutProfile(userId, profile);

        return (netResult.Status == EStatus.success);
    }

    public async Task<bool> PatchProfile(int userId, List<PatchData> patchData)
    {
        NetResult netResult = await NetProfileServices.PatchProfile(userId, patchData);

        return (netResult.Status == EStatus.success);
    }
}
