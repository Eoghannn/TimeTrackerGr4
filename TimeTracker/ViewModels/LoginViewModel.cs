using Storm.Mvvm;
using System;
using System.Collections.Generic;
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
                Response<LoginResponse> test = await api.loginAsync(_email, _password);
                Preferences.Set("access_token", test.Data.AccessToken);
                Console.WriteLine(test.Data.AccessToken);
                await NavigationService.PushAsync<MainPage>();
            }
            catch (UserNotFoundException ex)
            {
                Error = true;
            }
        }

        public LoginViewModel()
        {
            api = new Api();
            _error = false;
            Register = new Command(goToRegister);
            Confirm = new Command(goToMain);
        }
    }
}
