using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UnitsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<UnitCharacter> GetUnitCharacter(int userId, int charId, int unitId)
    {
        NetResult netResult = await NetUnitServices.GetCharUnit(userId, charId, unitId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<UnitCharacter>(netResult.Response);
        else
            return null;
    }

    public async Task<List<UnitCharacter>> GetAllUnitsCHaracter(int userId, int charId)
    {
        NetResult netResult = await NetUnitServices.GetAllCharUnit(userId, charId);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<List<UnitCharacter>>(netResult.Response);
        else
            return null;
    }

    public async Task<UnitCharacter> PostUnitCharacter(int userId, int charId, UnitCharacter unit)
    {
        NetResult netResult = await NetUnitServices.PostCharUnit(userId, charId, unit);

        if (netResult.Status == EStatus.success)
            return JsonConvert.DeserializeObject<UnitCharacter>(netResult.Response);
        else
            return null;
    }
    public async Task<bool> PutUnitCharacter(int userId, int charId, int unitId, UnitCharacter unit)
    {
        NetResult netResult = await NetUnitServices.PutCharUnit(userId, charId, unitId, unit);

        return (netResult.Status == EStatus.success);
    }

    public async Task<bool> PatchUnitCharacter(int userId, int charId, int unitId, List<PatchData> patchData)
    {
        NetResult netResult = await NetUnitServices.PatchCharUnit(userId, charId, unitId, patchData);

        return (netResult.Status == EStatus.success);
    }

}
