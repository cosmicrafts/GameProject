using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TypeAvatarController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<TypeAvatarCharacter> GetTypeCharAvatar(int typeavatarId)
    {
        NetResult netResult = await NetCharAvatarServices.GetTypeCharAvatar(typeavatarId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<TypeAvatarCharacter>(netResult.Response);
        else
            return null;
    }

    public async Task<List<TypeAvatarCharacter>> GeTypeAllAvatarOfCharacter(int typecharacter)
    {
        NetResult netResult = await NetCharAvatarServices.GetAllTypeAvatarOfCharacter(typecharacter);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<TypeAvatarCharacter>>(netResult.Response);
        else
            return null;
    }

    public async Task<List<TypeAvatarCharacter>> GeTypeAllAvatar()
    {
        NetResult netResult = await NetCharAvatarServices.GetAllTypeCharAvatar();

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<TypeAvatarCharacter>>(netResult.Response);
        else
            return null;
    }
}
