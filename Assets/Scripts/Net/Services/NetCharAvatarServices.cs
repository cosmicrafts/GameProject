using System.Collections.Generic;
using System.Threading.Tasks;

public static class NetCharAvatarServices
{
    public static async Task<NetResult> PostCharAvatar(int userId, int characterId, AvatarCharacter avatarchar)
    {
        NetResult result = await NetClient.POST(avatarchar, $"user/{userId}/DeckChar/{characterId}/Avatar");

        return result;
    }

    public static async Task<NetResult> GetCharAvatar(int userId, int characterId, int avatarId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/DeckChar/{characterId}/Avatar/{avatarId}");

        return result;
    }

    public static async Task<NetResult> GetAllCharAvatar(int userId, int characterId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/DeckChar/{characterId}/Avatar");

        return result;
    }

    public static async Task<NetResult> PutCharAvatar(int userId, int characterId, int avatarId, AvatarCharacter avatarchar)
    {
        NetResult result = await NetClient.POST(avatarchar, $"user/{userId}/DeckChar/{characterId}/Avatar/{avatarId}");

        return result;
    }

    public static async Task<NetResult> PatchCharAvatar(int userId, int characterId, int avatarId, List<PatchData> patchdata)
    {
        NetResult result = await NetClient.PATCH(patchdata, $"user/{userId}/DeckChar/{characterId}/Avatar/{avatarId}");

        return result;
    }

    public static async Task<NetResult> GetTypeCharAvatar(int typeavatarId)
    {
        NetResult result = await NetClient.GET($"Types/Avatar/{typeavatarId}");

        return result;
    }

    public static async Task<NetResult> GetAllTypeAvatarOfCharacter(int typecharacterId)
    {
        NetResult result = await NetClient.GET($"Types/Avatar/Character/{typecharacterId}");

        return result;
    }

    public static async Task<NetResult> GetAllTypeCharAvatar()
    {
        NetResult result = await NetClient.GET($"Types/Avatar");

        return result;
    }
}
