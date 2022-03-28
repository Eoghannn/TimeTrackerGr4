// See https://aka.ms/new-console-template for more information
using TimeTracker.API.Authentications;
using TimeTracker.API;
using TimeTracker.APIModele;

Api api = new Api();
//await api.registerAsync("descamps.m91@hotmail.com","Matysse","Descamps","test1234");
Response<LoginResponse> test = await api.loginAsync("descamps.m91@hotmail.com","test1234");
await api.refreshAsync(test.Data.RefreshToken);