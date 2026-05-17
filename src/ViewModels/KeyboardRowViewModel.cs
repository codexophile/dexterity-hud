using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DexterityHud.ViewModels
{
    internal sealed class KeyboardRowViewModel
    {
        public KeyboardRowViewModel(IEnumerable<KeyViewModel> keys)
        {
            Keys = new ObservableCollection<KeyViewModel>(keys);
        }

        public ObservableCollection<KeyViewModel> Keys { get; }
    }
}