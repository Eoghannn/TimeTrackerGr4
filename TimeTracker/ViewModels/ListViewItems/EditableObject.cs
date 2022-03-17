using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker.ViewModels.ListViewItems
{
    public abstract class EditableObject : BaseViewModel
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
            set => SetValue(ref _editEntry, value);
        }
        // à surcharger par les classes qui l'implem pour set le focus sur l'entry qui correspond 

        public bool IsEdited
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
                    // TODO ici l'utilisateur a potentiellement changer le titre du projet, si c'est le cas il faut l'envoyer au serveur
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
                
                SetValue(ref _isEdited, value);
            }
        }

        public ICommand ToggleEdit { get { return new Command(() => {IsEdited = !IsEdited; }); } }
        
    }
}