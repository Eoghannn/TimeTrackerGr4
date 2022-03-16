
using System.Windows.Input;
using Xamarin.Forms;

namespace TimeTracker.ViewModels
{
    public abstract class EditableObject : BaseViewModel
    {
        private static EditableObject _inEdition; // il ne peut y avoir qu'un project en édition à la fois
        public static EditableObject inEdition
        {
            get => _inEdition;
            set
            {
                if (value == null) MainPage.nPButton.IsEnabled = true;
                else MainPage.nPButton.IsEnabled = false;
                _inEdition = value;
            }
        }
        
        private bool _isEdited;

        public virtual Entry EditEntry { get; set; }
        // à surcharger par les classes qui l'implem pour set le focus sur l'entry qui correspond 

        public bool IsEdited
        {
            get => _isEdited;
            set
            {
                if (!value)
                {
                    if (inEdition == this)
                    {
                        inEdition = null;
                    }
                    // TODO ici l'utilisateur a potentiellement changer le titre du projet, si c'est le cas il faut l'envoyer au serveur
                }
                else
                {
                    if (inEdition != null)
                    {
                        inEdition.IsEdited = false;
                    }
                    inEdition = this;
                    if (EditEntry != null)
                    {
                        EditEntry.Focus();
                        EditEntry.Unfocused += (sender, args) => IsEdited = false ;
                    }
                }
                
                SetValue(ref _isEdited, value);
            }
        }

        public ICommand ToggleEdit { get { return new Command(() => {IsEdited = !IsEdited; }); } }
        
    }
}