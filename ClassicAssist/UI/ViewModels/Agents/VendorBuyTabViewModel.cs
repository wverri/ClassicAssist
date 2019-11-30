using System.Collections.ObjectModel;
using ClassicAssist.Data;

namespace ClassicAssist.UI.ViewModels.Agents
{
    public class VendorBuyTabViewModel : BaseViewModel
    {
        private bool _includeBackpackAmount;
        private bool _includePurchasedAmount;
        private ObservableCollection<VendorBuyAgentEntry> _items = new ObservableCollection<VendorBuyAgentEntry>();
        private VendorBuyAgentEntry _selectedItem;

        public bool IncludeBackpackAmount
        {
            get => _includeBackpackAmount;
            set => SetProperty( ref _includeBackpackAmount, value );
        }

        public bool IncludePurchasedAmount
        {
            get => _includePurchasedAmount;
            set => SetProperty( ref _includePurchasedAmount, value );
        }

        public ObservableCollection<VendorBuyAgentEntry> Items
        {
            get => _items;
            set => SetProperty( ref _items, value );
        }

        public VendorBuyAgentEntry SelectedItem
        {
            get => _selectedItem;
            set => SetProperty( ref _selectedItem, value );
        }
    }
}