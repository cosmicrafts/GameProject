using System.Collections.Generic;
using System.Threading.Tasks;

public static class NetUserServices
{
    public static async Task<NetResult> LogIn(string user, string pass)
    {
        NetResult result = await NetClient.POST(new
        {
            NikeName = user,
            Password = pass
        }, "user/auth");

        return result;
    }

    public static async Task<NetResult> LogInOrAdd(string user, string pass, string mail)
    {
        NetResult result = await NetClient.POST(new
        {
            NikeName = user,
            Password = pass,
            Email = mail,
        }, "user/authoradd");

        return result;
    }

    public static async Task<NetResult> SignUp(string user, string pass, string email)
    {
        NetResult result = await NetClient.POST(new
        {
            NikeName = user,
            PassWord = pass,
            Email = email,
            Rol = 1,
            GameData.Region
        }, "user");

        return result;
    }

    public static void LogOut()
    {
        GameData.ClearUser();
    }

    public static async Task<NetResult> GetUsers(string query)
    {
        NetResult result = await NetClient.GET("user?SearchQuery=" + query);

        return result;
    }

    public static async Task<NetResult> GetTwitchResponse(string email)
    {
        NetResult result = await NetClient.POST(new
        {
            NikeName = "nameempty",
            PassWord = "passempty",
            Email = email,
            Rol = 1,
            GameData.Region
        }, "user/twitch/validate");

        return result;
    }

    public static async Task<NetResult> PatchUsers(List<PatchData> patchData,int userId)
    {
        NetResult result = await NetClient.PATCH(patchData, "user/"+ userId.ToString());

        return result;
    }
}
