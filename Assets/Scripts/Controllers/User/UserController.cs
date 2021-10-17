using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UserController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<List<User>> GetUsers(string query)
    {
        NetResult netResult = await NetUserServices.GetUsers(query);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<User>>(netResult.Response);

        Debug.LogError(netResult.Response);
        return null;
    }

    public async Task<User> PostUser(string username, string password, string email)
    {
        NetResult netResult = await NetUserServices.SignUp(username, password, email);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<User>(netResult.Response);
        
        Debug.LogError(netResult.Response);
        return null;
    }

    public async Task<bool> PatchUser(List<PatchData> patchData, int userId)
    {
        NetResult netResult = await NetUserServices.PatchUsers(patchData, userId);

        if (netResult.Status != EStatus.success)
            Debug.LogError(netResult.Response);

        return (netResult.Status == EStatus.success);
    }
}
