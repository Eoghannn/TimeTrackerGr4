using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker
{
    public class LoginViewModel : ViewModelBase
    {
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
                //TODO Requete POST sur /api/v1/login (si token expiré ?) pour récupérer un token d'accès
                await NavigationService.PushAsync<MainPage>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public LoginViewModel()
        {
            Register = new Command(goToRegister);
            Confirm = new Command(goToMain);
        }
    }
}
