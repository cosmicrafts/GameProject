using System.Collections.Generic;
using System.Threading.Tasks;

public static class NetDeckCharServices
{
    public static async Task<NetResult> PostDeckChar(int userId, DeckCharacter deckcharacter)
    {
        NetResult result = await NetClient.POST(deckcharacter, $"user/{userId}/DeckChar");

        return result;
    }

    public static async Task<NetResult> GetDeckChar(int userId, int dechcharId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/DeckChar/{dechcharId}");

        return result;
    }

    public static async Task<NetResult> GetUserDeckChar(int userId)
    {
        NetResult result = await NetClient.GET($"user/{userId}/DeckChar");

        return result;
    }

    public static async Task<NetResult> PutDeckChar(int userId, int dechcharId, DeckCharacter deckcharacter)
    {
        NetResult result = await NetClient.POST(deckcharacter, $"user/{userId}/DeckChar/{dechcharId}");

        return result;
    }

    public static async Task<NetResult> PatchDeckChar(int userId, int dechcharId, List<PatchData> patchdata)
    {
        NetResult result = await NetClient.PATCH(patchdata, $"user/{userId}/DeckChar/{dechcharId}");

        return result;
    }

    public static async Task<NetResult> GetTypeDeckChar(int typedeckcharId)
    {
        NetResult result = await NetClient.GET($"Types/DeckChar/{typedeckcharId}");

        return result;
    }

    public static async Task<NetResult> GetAllTypeDeckChar()
    {
        NetResult result = await NetClient.GET($"Types/DeckChar");

        return result;
    }
}
