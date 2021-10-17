using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TypeCharacterController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<TypeCharacter> GetTypeCharacter(int typecharacterid)
    {
        NetResult netResult = await NetDeckCharServices.GetTypeDeckChar(typecharacterid);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<TypeCharacter>(netResult.Response);
        else
            return null;
    }

    public async Task<List<TypeCharacter>> GetAllTypeCharacter()
    {
        NetResult netResult = await NetDeckCharServices.GetAllTypeDeckChar();

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<TypeCharacter>>(netResult.Response);
        else
            return null;
    }
}
