using ClassicAssist.Data.Hotkeys;

namespace ClassicAssist.Data.Macros
{
    public class MacroEntry : HotkeySettable
    {
        private string _name;
        private bool _loop;
        private string _macro;
        private bool _doNotAutoInterrupt;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public bool Loop
        {
            get => _loop;
            set => SetProperty(ref _loop, value);
        }

        public string Macro
        {
            get => _macro;
            set => SetProperty(ref _macro, value);
        }

        public bool DoNotAutoInterrupt
        {
            get => _doNotAutoInterrupt;
            set => SetProperty(ref _doNotAutoInterrupt, value);
        }

        public override string ToString()
        {
            return _name;
        }
    }
}