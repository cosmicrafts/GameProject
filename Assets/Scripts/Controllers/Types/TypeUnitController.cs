using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TypeUnitController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<TypeUnitCharacter> GetTypeUnitCharacter(int typeunit)
    {
        NetResult netResult = await NetUnitServices.GetTypeCharUnit(typeunit);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<TypeUnitCharacter>(netResult.Response);
        else
            return null;
    }

    public async Task<List<TypeUnitCharacter>> GetAllTypeUnitOfCharacter(int typecharacterId)
    {
        NetResult netResult = await NetUnitServices.GetAllTypeUnitOfCharacter(typecharacterId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<TypeUnitCharacter>>(netResult.Response);
        else
            return null;
    }

    public async Task<List<TypeUnitCharacter>> GetAllTypeUnitCharacter()
    {
        NetResult netResult = await NetUnitServices.GetAllTypeCharUnit();

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<TypeUnitCharacter>>(netResult.Response);
        else
            return null;
    }
}
