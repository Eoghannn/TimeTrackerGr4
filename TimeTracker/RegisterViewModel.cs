using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker
{
    public class RegisterViewModel : ViewModelBase
    {
        public ICommand Confirm { get; }
        public ICommand Back { get; }

        public async void goToMain()
        {
            try
            {
                //TODO Créer le compte avec requete POST sur /api/v1/register et dans le modèle 
                await NavigationService.PushAsync<Profile>();
            }
            catch (Exception ex) { 
                Console.WriteLine(ex);
            }
        }

        public async void goToLogin()
        {
            try
            { 
                await NavigationService.PushAsync<Login>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public RegisterViewModel()
        {
            Confirm = new Command(goToMain);
            Back = new Command(goToLogin);
        }
    }
}
