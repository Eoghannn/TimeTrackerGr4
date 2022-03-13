using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewTemplate : ViewCell
    {
        public ListViewTemplate()
        {
            InitializeComponent();
        }

        private void ListViewTemplate_OnTapped(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}