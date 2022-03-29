using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TimeTracker.API;
using TimeTracker.API.Authentications;
using TimeTracker.API.ThrowException;
using Xamarin.Forms;

namespace TimeTracker
{
    public class RegisterViewModel : ViewModelBase
    {
        private Api api;
        private String _firstname;
        private String _lastname;
        private String _email;
        private String _password;

        public String Firstname
        {
            get { return _firstname; }
            set { SetProperty(ref _firstname, value); }
        }
        public String Lastname
        {
            get { return _lastname; }
            set { SetProperty(ref _lastname, value); }
        }
        public String Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        public String Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public ICommand Confirm { get; }
        public ICommand Back { get; }

        private bool _error;

        private String _textError;

        public String TextError
        {
            get { return _textError; }
            set { SetProperty(ref _textError, value); }
        }

        public bool Error
        {
            get { return _error; }
            set { SetProperty(ref _error, value); }
        }

        public async void goToMain()
        {
            try
            {
                if (_firstname.Length > 0 && _lastname.Length>0 && _email.Length>0 && _password.Length>0)
                {
                    //TODO Créer le compte avec requete POST sur /api/v1/register et dans le modèle 
                    Response<LoginResponse> test = await api.registerAsync(_email, _firstname, _lastname, _password);
                    Console.WriteLine(test);
                    await NavigationService.PushAsync<MainPage>();
                }
                else{
                    TextError = "All fields must be completed";
                    Error = true;
                }
            }
            catch (MailAlreadyExistException ex) {
                TextError = "Mail already used";
                Error = true;
            }
        }

        public async void goToLogin()
        {
            try
            { 
                await NavigationService.PopAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public RegisterViewModel()
        {
            _error = false;
            api = new Api();
            Confirm = new Command(goToMain);
            Back = new Command(goToLogin);
            _firstname = "";
            _lastname = "";
            _email = "";
            _password = "";
        }
    }
}
