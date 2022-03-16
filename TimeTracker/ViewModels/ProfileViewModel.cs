using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker
{
    public class ProfileViewModel : ViewModelBase
    {
        private bool _visible = false;
        
        private string _fname;
        private string _lname;
        private string _email;

        public string Fname
        {
            get { return _fname; }
            set { SetProperty(ref _fname, value); }
        }

        public string Lname
        {
            get { return _lname; }
            set { SetProperty(ref _lname, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        public bool Visible
        {
            get { return _visible; }
            set { SetProperty(ref _visible, value); }
        }

        public ICommand Edit { get; }
        public ICommand Back { get; }
        public ICommand ConfirmI { get; }
        public ICommand ConfirmP { get; }

        public void showPasswordFields()
        {
            Visible = !Visible;
        }

        public async void goBack()
        {
            try
            {
                await NavigationService.PopAsync();
                //await NavigationService.PushAsync(new Login());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void confirmInfos()
        {
            //TODO Requete des changements d'infos du profil PATCH sur /api/v1/me
            //et modifier les infos dans le modèle
            goBack();
        }
        public void confirmPassword()
        {
            //TODO Requete des changements de mdp PATCH sur /api/v1/password
            //et modifier les infos dans le modèle
            showPasswordFields();
        }

        public ProfileViewModel()
        {
            Edit = new Command(showPasswordFields);
            Back = new Command(goBack);
            ConfirmI = new Command(confirmInfos);
            ConfirmP = new Command(confirmPassword);
        }
    }
}

