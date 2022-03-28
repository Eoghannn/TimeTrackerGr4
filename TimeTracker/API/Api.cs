
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.API.Accounts;
using TimeTracker.API.Authentications;
using TimeTracker.API.Authentications.Credentials;
using TimeTracker.API.ThrowException;

namespace TimeTracker.API
{
    public class Api
    {
        public async Task<Response<LoginResponse>> loginAsync(string email, string mdp)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://timetracker.julienmialon.ovh/swagger/");

            LoginWithCredentialsRequest loginRequest = new LoginWithCredentialsRequest();

            loginRequest.Login = email;
            loginRequest.Password = mdp;
            loginRequest.ClientId = "MOBILE";
            loginRequest.ClientSecret = "COURS";

            StringContent content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/api/v1/login", content);

            var result = await response.Content.ReadAsStringAsync();
            Response<LoginResponse> test = JsonConvert.DeserializeObject<Response<LoginResponse>>(result);

            if (test.IsSucess)
            {
                return test;
            }
            else
            {
                throw new UserNotFoundException();
            }

        }


        public async Task<Response<LoginResponse>> registerAsync(string mail, string firstname, string lastname, string password)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://timetracker.julienmialon.ovh/swagger/");

            CreateUserRequest registerRequest = new CreateUserRequest();

            registerRequest.ClientId = "MOBILE";
            registerRequest.ClientSecret = "COURS";
            registerRequest.Email = mail;
            registerRequest.FirstName = firstname;
            registerRequest.LastName = lastname;
            registerRequest.Password = password;

            StringContent content = new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/api/v1/register", content);

            var result = await response.Content.ReadAsStringAsync();
            Response<LoginResponse> test = JsonConvert.DeserializeObject<Response<LoginResponse>>(result);

            if (test.IsSucess)
            {
                return test;
            }
            else
            {
                throw new MailAlreadyExistException();
            }
        }

        public async Task<Response<LoginResponse>> refreshAsync(string refresh_token)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://timetracker.julienmialon.ovh/swagger/");

            RefreshRequest refreshrequest = new RefreshRequest();

            refreshrequest.RefreshToken = refresh_token;
            refreshrequest.ClientId = "MOBILE";
            refreshrequest.ClientSecret = "COURS";

            StringContent content = new StringContent(JsonConvert.SerializeObject(refreshrequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/api/v1/refresh", content);

            var result = await response.Content.ReadAsStringAsync();
            Response<LoginResponse> test = JsonConvert.DeserializeObject<Response<LoginResponse>>(result);

            if (test.IsSucess)
            {
                return test;
            }
            else
            {
                throw new WrongRefreshTokenException();
            }
        }

        public async Task<Response<string>> passwordAsync(string old_password, string new_password)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://timetracker.julienmialon.ovh/swagger/");

            SetPasswordRequest changepassword = new SetPasswordRequest();

            changepassword.OldPassword = old_password;
            changepassword.NewPassword = new_password;

            StringContent content = new StringContent(JsonConvert.SerializeObject(changepassword), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PatchAsync("/api/v1/password", content);

            var result = await response.Content.ReadAsStringAsync();
            Response<string> test = JsonConvert.DeserializeObject<Response<string>>(result);

            if (test.IsSucess)
            {
                return test;
            }
            else
            {
                throw new WrongOldPasswordException();
            }
        }

    }
}