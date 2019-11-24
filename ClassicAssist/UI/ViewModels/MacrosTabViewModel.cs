using ClassicAssist.Data;
using ClassicAssist.Data.Hotkeys;
using ClassicAssist.Misc;
using Newtonsoft.Json.Linq;

namespace ClassicAssist.UI.ViewModels
{
    public class MacrosTabViewModel : BaseViewModel, ISettingProvider
    {
        private HotkeyManager _hotkeys;

        public MacrosTabViewModel()
        {
            _hotkeys = HotkeyManager.GetInstance();

        }
        public void Serialize( JObject json )
        {
        }

        public void Deserialize( JObject json, Options options )
        {
        }
    }
}