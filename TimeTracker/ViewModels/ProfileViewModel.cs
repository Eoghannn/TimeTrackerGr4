using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeTracker.API;
using TimeTracker.API.Accounts;
using TimeTracker.API.ThrowException;
using TimeTracker.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TimeTracker
{
    public class ProfileViewModel : ViewModelBase
    {
        private MainPageViewModel mpvm;
        private bool _visible = false;
        
        private string _fname;
        private string _lname;
        private string _email;

        private string _oldP;
        private string _newP;

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

        public string OldP
        {
            get { return _oldP; }
            set { SetProperty(ref _oldP, value); }
        }

        public string NewP
        {
            get { return _newP; }
            set { SetProperty(ref _newP, value); }
        }

        private bool _error;

        public bool Error
        {
            get { return _error; }
            set { SetProperty(ref _error, value); }
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

        public async void confirmInfos()
        {
            //TODO Requete des changements d'infos du profil PATCH sur /api/v1/me
            //et modifier les infos dans le modèle
            try
            {
                Response<UserProfileResponse> modification = await ApiSingleton.Instance.Api.modifmeAsync(ApiSingleton.Instance.access_token, _email, _fname, _lname);
                mpvm.EmailString = _email;
                mpvm.nomPrenomString = _fname + " " + _lname;
                goBack();
            } catch (WrongAccessTokenException ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async void confirmPassword()
        {
            //TODO Requete des changements de mdp PATCH sur /api/v1/password
            //et modifier les infos dans le modèle
            try
            {
                ResponseStandard isSucces = await ApiSingleton.Instance.Api.passwordAsync(ApiSingleton.Instance.access_token, _oldP, _newP);
                Console.WriteLine(isSucces.ToString());
                if (Error)
                    Error = false;
                showPasswordFields();
            } catch(WrongOldPasswordException e)
            {
                Error = true;
                Console.WriteLine(e.Message);
            }

        }

        public ProfileViewModel(MainPageViewModel mpvm)
        {
            this.mpvm = mpvm;
            _error = false;
            Edit = new Command(showPasswordFields);
            Back = new Command(goBack);
            ConfirmI = new Command(confirmInfos);
            ConfirmP = new Command(confirmPassword);

            getUserInfos();          
        }

        public async void getUserInfos()
        {
            try
            {
                Response<UserProfileResponse> getInfos = await ApiSingleton.Instance.Api.getmeAsync(ApiSingleton.Instance.access_token);
                Fname = getInfos.Data.FirstName;
                Lname = getInfos.Data.LastName;
                Email = getInfos.Data.Email;
            } catch (WrongAccessTokenException e)
            {
                Console.WriteLine(e);
            }

        }
    }
}

