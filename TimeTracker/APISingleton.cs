using System;
using System.Threading.Tasks;
using TimeTracker.API.Authentications;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace TimeTracker;
using TimeTracker.API;

public class ApiSingleton
{
    public static ApiSingleton Instance = new ApiSingleton();

    public Api Api = new Api();
    private string _access_token; // public car accessible au sein de l'app

    public string access_token // readonly
    {
        get => _access_token;
    }
    
    private string refresh_token;
    private int expires_in;
    private DateTime created_at;

    public void login(LoginResponse response)
    {
        _access_token = response.AccessToken;
        Preferences.Set("refresh_token", response.RefreshToken);
        Preferences.Set("expires_in", response.ExpiresIn);
        created_at = DateTime.Now;
        Preferences.Set("created_at", created_at);
        expires_in = response.ExpiresIn;
        refresh_token = response.RefreshToken;
    }

    public bool isExpired()
    {
        return created_at.Add(TimeSpan.FromSeconds(expires_in)) < DateTime.Now;
    }

    public void logout()
    {
        Preferences.Set("refresh_token", "");
        Preferences.Set("expires_in", 0);
        _access_token = "";
        refresh_token = "";
        expires_in= 0;
    }

    public async Task<bool> refreshToken()
    {
        if (isExpired())
        {
            return false;
        }

        Response<LoginResponse> response = await Api.refreshAsync(refresh_token);
        if (response.IsSucess)
        {
            login(response.Data);
            return true;
        }

        return false;
    }

    private ApiSingleton()
    {
        refresh_token = Preferences.Get("refresh_token", "");
        expires_in = Preferences.Get("expires_in", 0);
        created_at = Preferences.Get("created_at", DateTime.Now);
    }
}