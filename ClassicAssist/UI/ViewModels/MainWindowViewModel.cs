using System;
using System.ComponentModel;
using System.Reflection;
using Assistant;
using ClassicAssist.Data;
using ClassicAssist.Resources;
using ClassicAssist.UO.Objects;

namespace ClassicAssist.UI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _status = Strings.Ready___;
        private bool _alwaysOnTop;
        private string _title = Strings.ProductName;

        [OptionsBinding(Property = "AlwaysOnTop")]
        public bool AlwaysOnTop
        {
            get => _alwaysOnTop;
            set => SetProperty(ref _alwaysOnTop, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public MainWindowViewModel()
        {
            Engine.PlayerInitializedEvent += PlayerInitialized;
        }

        private void PlayerInitialized( PlayerMobile player )
        {
            Title = string.IsNullOrEmpty( player.Name ) ? Strings.ProductName : $"{Strings.ProductName} - {player.Name}";
        }

        public string Title 
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }

    public class OptionsBindingAttribute : Attribute
    {
        public string Property { get; set; }
    }
}