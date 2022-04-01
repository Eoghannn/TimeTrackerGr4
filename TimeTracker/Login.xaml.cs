using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storm.Mvvm.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {

        private bool alreadyAutoLogged = false;
        public Login()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
        
        public async void testLoggin() 
        {
            bool isLoggedin = await ApiSingleton.Instance.refreshToken();
            if (isLoggedin)
            {
                alreadyAutoLogged = true;
                await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Le testLoggin sert à regarder si on a pas déjà un refresh Token de stocker dans les prefs, auquel cas on peut se connecter immédiatement
            // Apparement il faut attendre que la page soit construite avant de faire un appel au service de navigation, je ne peux donc pas mettre testLogin() directement dans le constructeur de la page ( ou celui du viewmodel )
            // Avec OnAppearing ça a l'air de fonctionner correctement 
            if(!alreadyAutoLogged)
                testLoggin();
        }
        
        
    }
}