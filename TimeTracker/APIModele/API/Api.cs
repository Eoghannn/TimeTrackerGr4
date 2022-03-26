
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.API.Accounts;
using TimeTracker.API.Authentications;
using TimeTracker.API.Authentications.Credentials;
using TimeTracker.APIModele;

namespace TimeTracker.API
{
    public class Api
    {
        public async Task loginAsync(string email, string mdp)
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

            Console.WriteLine(test.Data.AccessToken);

        }


        public async Task registerAsync(string mail, string firstname, string lastname, string password)
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
            Response<LoginResponse> test = JsonConvert.DeserializeObject<Response<LoginResponse>> (result);

            Console.WriteLine(test.ErrorMessage);
        }

    }
}