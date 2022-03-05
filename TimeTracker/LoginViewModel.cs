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

        public async void goToRegister()
        {
            await NavigationService.PushAsync(new Register());
        }

        public LoginViewModel()
        {
            Register = new Command(goToRegister);
        }
    }
}
