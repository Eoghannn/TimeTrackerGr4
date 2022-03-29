// See https://aka.ms/new-console-template for more information

using TimeTracker.APIModel.API;
using TimeTracker.APIModel.API.Authentications;
using TimeTracker.APIModele.API.Projects;

Api api = new Api();
//await api.registerAsync("descamps.m91@hotmail.com","Matysse","Descamps","test1234");
//Response<LoginResponse> test = await api.loginAsync("descamps.m91@hotmail.com","test123");
//await api.refreshAsync(test.Data.RefreshToken);
//await api.passwordAsync(test.Data.AccessToken,"test1234","test123");
//await api.createprojetAsync(test.Data.AccessToken, "test2", "jesuisladescription2");
//Response<List<ProjectItem>> projets = await api.getprojetsAsync(test.Data.AccessToken);
//await api.modifprojetAsync(test.Data.AccessToken,"test1","jesuisladescription",projets.Data[0].Id);
//await api.deleteprojetAsync(test.Data.AccessToken, projets.Data[0].Id);
//Response<List<TaskItem>> task = await api.gettasksAsync(test.Data.AccessToken,projets.Data[0].Id);
//await api.createtaskAsync(test.Data.AccessToken, projets.Data[0].Id, "task1");
//await api.modiftaskAsync(test.Data.AccessToken,projets.Data[0].Id,task.Data[0].Id,"taskmodif");
//await api.deletetaskAsync(test.Data.AccessToken,projets.Data[0].Id,task.Data[0].Id);
/*DateTime debut = new DateTime(2022,03,29);
DateTime fin = new DateTime(2022,04,29);*/
//await api.settimetaskAsync(test.Data.AccessToken, projets.Data[0].Id, task.Data[0].Id,debut,fin);
//await api.modiftimetaskAsync(test.Data.AccessToken, projets.Data[0].Id, task.Data[0].Id,task.Data[0].Times[0].Id, debut, fin);
//await api.deletetimetaskAsync(test.Data.AccessToken, projets.Data[0].Id, task.Data[0].Id,task.Data[0].Times[0].Id);
//await api.getmeAsync(test.Data.AccessToken);
//await api.modifmeAsync(test.Data.AccessToken,"descamps.m91@hotmail.fr","Mathis","Deschamps");