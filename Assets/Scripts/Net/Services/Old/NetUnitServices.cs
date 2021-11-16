
using System.Collections.Generic;
using System.Threading.Tasks;

public static class NetUnitServices
{
    public static async Task<NetResult> PostCharUnit(int userId, int characterId, UnitCharacter unitCharacter)
    {
        NetResult result = await NetClient.POST(unitCharacter, $"user/{userId}/DeckChar/{characterId}/Unit");

        return result;
    }

    public static async Task<NetResult> GetCharUnit(int userId, int characterId, int unitId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/DeckChar/{characterId}/Unit/{unitId}");

        return result;
    }

    public static async Task<NetResult> GetAllCharUnit(int userId, int characterId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/DeckChar/{characterId}/Unit");

        return result;
    }

    public static async Task<NetResult> PutCharUnit(int userId, int characterId, int unitId, UnitCharacter unitCharacter)
    {
        NetResult result = await NetClient.POST(unitCharacter, $"user/{userId}/DeckChar/{characterId}/Unit/{unitId}");

        return result;
    }

    public static async Task<NetResult> PatchCharUnit(int userId, int characterId, int unitId, List<PatchData> patchdata)
    {
        NetResult result = await NetClient.PATCH(patchdata, $"user/{userId}/DeckChar/{characterId}/Unit/{unitId}");

        return result;
    }

    public static async Task<NetResult> GetTypeCharUnit(int typeunitId)
    {
        NetResult result = await NetClient.GET($"Types/Unit/{typeunitId}");

        return result;
    }

    public static async Task<NetResult> GetAllTypeUnitOfCharacter(int typecharacterId)
    {
        NetResult result = await NetClient.GET($"Types/Unit/Character/{typecharacterId}");

        return result;
    }

    public static async Task<NetResult> GetAllTypeCharUnit()
    {
        NetResult result = await NetClient.GET($"Types/Unit");

        return result;
    }
}
