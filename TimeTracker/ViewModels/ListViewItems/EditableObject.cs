using System;
using System.Diagnostics;
using System.Windows.Input;
using Storm.Mvvm;
using Xamarin.Forms;

namespace TimeTracker.ViewModels.ListViewItems
{
    public abstract class EditableObject : ViewModelBase
    {
        private MainPageViewModel _mainPageViewModel;
        
        private bool _isEdited;

        protected EditableObject(MainPageViewModel mainPageViewModel)
        {
            _mainPageViewModel = mainPageViewModel;
        }

        private Entry _editEntry;
        public Entry EditEntry
        {
            get => _editEntry;
            set => SetProperty(ref _editEntry, value);
        }
        // à surcharger par les classes qui l'implem pour set le focus sur l'entry qui correspond 

        public virtual bool IsEdited
        {
            get => _isEdited;
            set
            {
                if (!value)
                {
                    if (_mainPageViewModel.InEdition == this)
                    {
                        _mainPageViewModel.InEdition = null;
                    }
                }
                else
                {
                    if (_mainPageViewModel.InEdition != null)
                    {
                        _mainPageViewModel.InEdition.IsEdited = false;
                    }
                    _mainPageViewModel.InEdition = this;
                    if (EditEntry != null)
                    {
                        EditEntry.Focus();
                        EditEntry.Unfocused += (sender, args) => IsEdited = false ;
                    }
                    else
                    {
                        Debug.WriteLine(this + " n'a pas de Edit Entry");
                    }
                }
                
                SetProperty(ref _isEdited, value);
            }
        }

        public ICommand ToggleEdit { get { return new Command(() => {IsEdited = !IsEdited; }); } }
        
    }
}