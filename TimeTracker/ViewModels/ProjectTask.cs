using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public class ProjectTask : ObjWithDuration
    {
        private String _title;
        public String Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }
        
        private Color _color;
        public Color Color
        {
            get => _color;
            set => SetValue(ref _color, value);
        }
        public virtual ICommand Remove{ get { return new Command(() =>
        {
            return;
        }); } }
        public virtual ICommand Tapped{ get { return new Command(() =>
        {
            return;
        }); } }
    }
}