using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using TimeTracker.API;
using TimeTracker.API.Authentications;
using TimeTracker.API.ThrowException;
using Xamarin.Essentials;

namespace TimeTracker
{
    public class LoginViewModel : ViewModelBase
    {
        private Api api;
        private String _email;
        private String _password;

        public String Email { 
            get { return _email; } 
            set { SetProperty(ref _email, value); } 
        }

        public String Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private bool _error;

        public bool Error
        {
            get { return _error; }
            set { SetProperty(ref _error, value); }
        }

        public ICommand Register
        {
            get;
        }

        public ICommand Confirm { get; }

        public async void goToRegister()
        {
            await NavigationService.PushAsync(new Register());
        }

        public async void goToMain()
        {
            try
            {
                //Requete POST sur /api/v1/login pour récupérer un token d'accès
                Response<LoginResponse> test = await ApiSingleton.Instance.Api.loginAsync(_email, _password);
                if (test.IsSucess)
                {
                    ApiSingleton.Instance.login(test.Data);
                }
                else
                {
                    Debug.WriteLine(test);
                }
                await NavigationService.PushAsync<MainPage>();
                
            }
            catch (UserNotFoundException ex)
            {
                Error = true;
                Debug.WriteLine("Error");
            }
        }
        
        public async void testLoggin() 
        {
            bool isLoggedin = await ApiSingleton.Instance.refreshToken();
            if (isLoggedin)
            {
                await NavigationService.PushAsync<MainPage>();
            }
        }

        public LoginViewModel()
        {
            
            _error = false;
            Register = new Command(goToRegister);
            Confirm = new Command(goToMain);
            
            // lors de la connection, on regarde si l'utilisateur s'était déjà connecté sur ce device, dans ce cas il possède un refresh token, s'il n'est pas expiré il doit pouvoir se log à partir de celui ci
            testLoggin();
        }
    }
}
