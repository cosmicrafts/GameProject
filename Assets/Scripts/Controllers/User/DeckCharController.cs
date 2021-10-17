using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DeckCharController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<DeckCharacter> GetDeckCharacter(int userId, int characterId)
    {
        NetResult netResult = await NetDeckCharServices.GetDeckChar(userId, characterId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<DeckCharacter>(netResult.Response);
        else
            return null;
    }

    public async Task<List<DeckCharacter>> GetUserDeckCharacter(int userId)
    {
        NetResult netResult = await NetDeckCharServices.GetUserDeckChar(userId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<DeckCharacter>>(netResult.Response);
        else
            return null;
    }

    public async Task<DeckCharacter> PostDeckCharacter(int userId, DeckCharacter deckCharacter)
    {
        NetResult netResult = await NetDeckCharServices.PostDeckChar(userId, deckCharacter);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<DeckCharacter>(netResult.Response);
        else
            return null;
    }

    public async Task<bool> PutDeckCharacter(int userId, int characterId, DeckCharacter character)
    {
        NetResult netResult = await NetDeckCharServices.PutDeckChar(userId, characterId, character);

        return (netResult.Status == EStatus.success);
    }

    public async Task<bool> PatchDeckCharacter(int userId, int characterId, List<PatchData> patchData)
    {
        NetResult netResult = await NetDeckCharServices.PatchDeckChar(userId, characterId, patchData);

        return (netResult.Status == EStatus.success);
    }
}
