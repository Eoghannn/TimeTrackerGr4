using System;
using System.Threading.Tasks;
using TimeTracker.API.Authentications;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TimeTracker;
using TimeTracker.API;
using TimeTracker.API.ThrowException;

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

    public void login(LoginResponse response)
    {
        _access_token = response.AccessToken;
        Preferences.Set("refresh_token", response.RefreshToken);
        Preferences.Set("expires_in", response.ExpiresIn);
        expires_in = response.ExpiresIn;
        refresh_token = response.RefreshToken;

        
        // On refresh le token automatiquement avant qu'il expire ( 120 secondes avant, pourquoi pas )
        Device.StartTimer(TimeSpan.FromSeconds(expires_in-120),  () =>
        {
            refreshToken().ContinueWith((async task =>
            {
                bool autoLogged = await task;
                if (!autoLogged)
                {
                    // il y a eu un problème avec le refresh token --> l'utilisateur a besoin de se reconnecter 
                    await Application.Current.MainPage.Navigation.PopToRootAsync();
                }
            }));
            return false;
        });
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
        try
        {
            Response<LoginResponse> response = await Api.refreshAsync(refresh_token);
            if (response.IsSucess)
            {
                login(response.Data);
                return true;
            }

            return false;
        } catch (WrongRefreshTokenException e)
        {
            return false;
        }

    }

    private ApiSingleton()
    {
        refresh_token = Preferences.Get("refresh_token", "");
        expires_in = Preferences.Get("expires_in", 0);
    }
}