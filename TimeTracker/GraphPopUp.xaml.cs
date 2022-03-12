using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphPopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        public GraphPopUp()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}