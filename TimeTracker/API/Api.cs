
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.API.Accounts;
using TimeTracker.API.Authentications;
using TimeTracker.API.Authentications.Credentials;
using TimeTracker.API.Projects;
using TimeTracker.API.ThrowException;

namespace TimeTracker.API
{
    public class Api
    {
        public async Task<Response<LoginResponse>> loginAsync(string email, string mdp)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);

            LoginWithCredentialsRequest loginRequest = new LoginWithCredentialsRequest();

            loginRequest.Login = email;
            loginRequest.Password = mdp;
            loginRequest.ClientId = "MOBILE";
            loginRequest.ClientSecret = "COURS";

            StringContent content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Urls.LOGIN, content);

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
            client.BaseAddress = new Uri(Urls.API);

            CreateUserRequestTest registerRequest = new CreateUserRequestTest();

            registerRequest.ClientId = "MOBILE";
            registerRequest.ClientSecret = "COURS";
            registerRequest.Email = mail;
            registerRequest.FirstName = firstname;
            registerRequest.LastName = lastname;
            registerRequest.Password = password;

            StringContent content = new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Urls.CREATE_USER, content);

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
            client.BaseAddress = new Uri(Urls.API);

            RefreshRequest refreshrequest = new RefreshRequest();

            refreshrequest.RefreshToken = refresh_token;
            refreshrequest.ClientId = "MOBILE";
            refreshrequest.ClientSecret = "COURS";

            StringContent content = new StringContent(JsonConvert.SerializeObject(refreshrequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Urls.REFRESH_TOKEN, content);

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

        public async Task<string> passwordAsync(string access_token, string old_password, string new_password)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            SetPasswordRequest changepassword = new SetPasswordRequest();

            changepassword.OldPassword = old_password;
            changepassword.NewPassword = new_password;

            StringContent content = new StringContent(JsonConvert.SerializeObject(changepassword), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PatchAsync(Urls.SET_PASSWORD, content);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return "Succès";
            }
            else
            {
                throw new WrongOldPasswordException();
            }
        }

        public async Task<Response<List<ProjectItem>>> getprojetsAsync(string access_token)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            HttpResponseMessage response = await client.GetAsync(Urls.LIST_PROJECTS);

            var result = await response.Content.ReadAsStringAsync();
            Response<List<ProjectItem>> test = JsonConvert.DeserializeObject<Response<List<ProjectItem>>>(result);

            if (test.IsSucess)
            {
                return test;
            }
            else
            {
                throw new WrongAccessTokenException();
            }
        }

        public async Task<Response<ProjectItem>> createprojetAsync(string access_token, string name, string description)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            AddProjectRequest addproject = new AddProjectRequest();

            addproject.Name = name;
            addproject.Description = description;

            StringContent content = new StringContent(JsonConvert.SerializeObject(addproject), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Urls.ADD_PROJECT, content);

            var result = await response.Content.ReadAsStringAsync();
            Response<ProjectItem> test = JsonConvert.DeserializeObject<Response<ProjectItem>>(result);

            if (test.IsSucess)
            {
                return test;
            }
            else
            {
                throw new WrongAccessTokenException();
            }
        }

        public async Task<Response<ProjectItem>> modifprojetAsync(string access_token, string name, string description, long id_projet)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            AddProjectRequest addproject = new AddProjectRequest();

            addproject.Name = name;
            addproject.Description = description;

            StringContent content = new StringContent(JsonConvert.SerializeObject(addproject), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(Urls.UPDATE_PROJECT.Replace("{projectId}", id_projet.ToString()), content);

            var result = await response.Content.ReadAsStringAsync();
            Response<ProjectItem> test = JsonConvert.DeserializeObject<Response<ProjectItem>>(result);

            if (test.IsSucess)
            {
                return test;
            }
            else
            {
                throw new WrongIdProjectException();
            }
        }

        public async Task<string> deleteprojetAsync(string access_token, long id_projet)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            HttpResponseMessage response = await client.DeleteAsync(Urls.DELETE_PROJECT.Replace("{projectId}", id_projet.ToString()));

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return "succès";
            }
            else
            {
                throw new WrongIdProjectException();
            }
        }

        public async Task<Response<List<TaskItem>>> gettasksAsync(string access_token, long id_projet)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            HttpResponseMessage response = await client.GetAsync(Urls.LIST_TASKS.Replace("{projectId}", id_projet.ToString()));

            var result = await response.Content.ReadAsStringAsync();
            Response<List<TaskItem>> test = JsonConvert.DeserializeObject<Response<List<TaskItem>>>(result);

            if (test.IsSucess)
            {
                return test;
            }
            else
            {
                throw new WrongIdProjectOrTaskException();
            }
        }

        public async Task<Response<TaskItem>> createtaskAsync(string access_token, long id_projet, string name)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            AddTaskRequest addtask = new AddTaskRequest();

            addtask.Name = name;

            StringContent content = new StringContent(JsonConvert.SerializeObject(addtask), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Urls.CREATE_TASK.Replace("{projectId}", id_projet.ToString()), content);

            var result = await response.Content.ReadAsStringAsync();
            Response<TaskItem> test = JsonConvert.DeserializeObject<Response<TaskItem>>(result);

            if (test.IsSucess)
            {
                return test;
            }
            else
            {
                throw new WrongIdProjectOrTaskException();
            }
        }

        public async Task<Response<TaskItem>> modiftaskAsync(string access_token, long id_projet, long id_task, string name)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            AddTaskRequest addtask = new AddTaskRequest();

            addtask.Name = name;

            string urlmodif = Urls.UPDATE_TASK.Replace("{projectId}", id_projet.ToString());
            urlmodif = urlmodif.Replace("{taskId}", id_task.ToString());

            StringContent content = new StringContent(JsonConvert.SerializeObject(addtask), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(urlmodif, content);

            var result = await response.Content.ReadAsStringAsync();
            Response<TaskItem> test = JsonConvert.DeserializeObject<Response<TaskItem>>(result);

            if (test.IsSucess)
            {
                return test;
            }
            else
            {
                throw new WrongIdProjectOrTaskException();
            }
        }

        public async Task<string> deletetaskAsync(string access_token, long id_projet, long id_task)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            string urlmodif = Urls.DELETE_TASK.Replace("{projectId}", id_projet.ToString());
            urlmodif = urlmodif.Replace("{taskId}", id_task.ToString());

            HttpResponseMessage response = await client.DeleteAsync(urlmodif);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return "Succès";
            }
            else
            {
                throw new WrongIdProjectOrTaskException();
            }
        }

        public async Task<Response<TimeItem>> settimetaskAsync(string access_token, long id_projet, long id_task, DateTime start_time, DateTime end_time)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            AddTimeRequest addtime = new AddTimeRequest();

            addtime.StartTime = start_time;
            addtime.EndTime = end_time;

            string urlmodif = Urls.ADD_TIME.Replace("{projectId}", id_projet.ToString());
            urlmodif = urlmodif.Replace("{taskId}", id_task.ToString());

            StringContent content = new StringContent(JsonConvert.SerializeObject(addtime), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(urlmodif, content);

            var result = await response.Content.ReadAsStringAsync();
            Response<TimeItem> test = JsonConvert.DeserializeObject<Response<TimeItem>>(result);

            if (test.IsSucess)
            {
                return test;
            }
            else
            {
                throw new WrongIdProjectOrTaskOrTimeException();
            }
        }

        public async Task<Response<TimeItem>> modiftimetaskAsync(string access_token, long id_projet, long id_task, long id_time, DateTime start_time, DateTime end_time)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            AddTimeRequest addtime = new AddTimeRequest();

            addtime.StartTime = start_time;
            addtime.EndTime = end_time;

            string urlmodif = Urls.UPDATE_TIME.Replace("{projectId}", id_projet.ToString());
            urlmodif = urlmodif.Replace("{taskId}", id_task.ToString());
            urlmodif = urlmodif.Replace("{timeId}", id_time.ToString());

            StringContent content = new StringContent(JsonConvert.SerializeObject(addtime), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(urlmodif, content);

            var result = await response.Content.ReadAsStringAsync();
            Response<TimeItem> test = JsonConvert.DeserializeObject<Response<TimeItem>>(result);

            if (response.IsSuccessStatusCode)
            {
                return test;
            }
            else
            {
                throw new WrongIdProjectOrTaskOrTimeException();
            }
        }

        public async Task<string> deletetimetaskAsync(string access_token, long id_projet, long id_task, long id_time)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            string urlmodif = Urls.DELETE_TIME.Replace("{projectId}", id_projet.ToString());
            urlmodif = urlmodif.Replace("{taskId}", id_task.ToString());
            urlmodif = urlmodif.Replace("{timeId}", id_time.ToString());

            HttpResponseMessage response = await client.DeleteAsync(urlmodif);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return "Succès";
            }
            else
            {
                throw new WrongIdProjectOrTaskOrTimeException();
            }
        }

        public async Task<Response<UserProfileResponse>> getmeAsync(string access_token)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            HttpResponseMessage response = await client.GetAsync(Urls.USER_PROFILE);

            var result = await response.Content.ReadAsStringAsync();
            Response<UserProfileResponse> test = JsonConvert.DeserializeObject<Response<UserProfileResponse>>(result);

            if (response.IsSuccessStatusCode)
            {
                return test;
            }
            else
            {
                throw new WrongAccessTokenException();
            }
        }

        public async Task<Response<UserProfileResponse>> modifmeAsync(string access_token, string mail, string first_name, string last_name)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Urls.API);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            UserProfileResponse infome = new UserProfileResponse();

            infome.Email = mail;
            infome.FirstName = first_name;
            infome.LastName = last_name;

            StringContent content = new StringContent(JsonConvert.SerializeObject(infome), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PatchAsync(Urls.SET_USER_PROFILE, content);

            var result = await response.Content.ReadAsStringAsync();
            Response<UserProfileResponse> test = JsonConvert.DeserializeObject<Response<UserProfileResponse>>(result);

            if (response.IsSuccessStatusCode)
            {
                return test;
            }
            else
            {
                throw new WrongAccessTokenException();
            }
        }

    }
}