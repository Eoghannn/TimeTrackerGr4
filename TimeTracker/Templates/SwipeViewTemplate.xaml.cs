using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.ViewModels.ListViewItems;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwipeViewTemplate : SwipeView
    {
        public SwipeViewTemplate()
        {
            InitializeComponent();
            Debug.WriteLine(BindingContext);
        }

    }
}