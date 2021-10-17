using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharAvatarController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<AvatarCharacter> GetCharAvatar(int userId, int characterId, int avatarId)
    {
        NetResult netResult = await NetCharAvatarServices.GetCharAvatar(userId, characterId, avatarId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<AvatarCharacter>(netResult.Response);
        else
            return null;
    }

    public async Task<List<AvatarCharacter>> GetAllCharAvatar(int userId, int characterId)
    {
        NetResult netResult = await NetCharAvatarServices.GetAllCharAvatar(userId, characterId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<AvatarCharacter>>(netResult.Response);
        else
            return null;
    }

    public async Task<AvatarCharacter> PostCharAvatar(int userId, int characterId, AvatarCharacter avatar)
    {
        NetResult netResult = await NetCharAvatarServices.PostCharAvatar(userId, characterId, avatar);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<AvatarCharacter>(netResult.Response);
        else
            return null;
    }

    public async Task<bool> PutCharAvatar(int userId, int characterId, int avatarId, AvatarCharacter avatar)
    {
        NetResult netResult = await NetCharAvatarServices.PutCharAvatar(userId, characterId, avatarId, avatar);

        return (netResult.Status == EStatus.success);
    }

    public async Task<bool> PatchCharAvatar(int userId, int characterId, int avatarId, List<PatchData> patchData)
    {
        NetResult netResult = await NetCharAvatarServices.PatchCharAvatar(userId, characterId, avatarId, patchData);

        return (netResult.Status == EStatus.success);
    }
}
