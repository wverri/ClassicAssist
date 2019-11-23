using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using ClassicAssist.Annotations;
using ClassicAssist.Misc;

namespace ClassicAssist.Data.Hotkeys
{
    public class HotkeySettable : INotifyPropertyChanged
    {
        public delegate void HotkeyChangedEventHandler(object sender, HotkeyChangedEventArgs e);

        private ShortcutKeys _hotkey = new ShortcutKeys { Modifier = Key.None, Key = Key.None };

        private bool _passToUo = true;
        public Action<HotkeySettable> Action { get; set; }

        public ShortcutKeys Hotkey
        {
            get => _hotkey;
            set
            {
                SetProperty(ref _hotkey, value);
                HotkeyChanged?.Invoke(this, new HotkeyChangedEventArgs(_hotkey, value));
            }
        }

        [XmlIgnore]
        public ImageSource Image => Equals(Hotkey, ShortcutKeys.Default) ? Properties.Resources.red_circle.ToImageSource() : Properties.Resources.green_circle.ToImageSource();

        public bool PassToUO
        {
            get => _passToUo;
            set
            {
                _passToUo = value;

                //if (value == false)
                //    Macro.AddKeyboardFilter(Client, Hotkey.ToArray());
                //else
                //    Macro.RemoveKeyboardFilter(Client, Hotkey.ToArray());
            }
        }

        public HotkeySettable()
        {
            HotkeyChanged += OnHotkeyChanged;
        }

        public event HotkeyChangedEventHandler HotkeyChanged;

        protected virtual void OnHotkeyChanged(object sender, HotkeyChangedEventArgs e)
        {
            //if (e.OldValue.Key != Key.None) Commands.RemoveKeyboardMacro(e.OldValue.ToArray(), !PassToUO);

            //if (e.NewValue.Key != Key.None) Commands.AddKeyboardMacro(e.NewValue.ToArray(), index => { Action?.Invoke(this); }, !PassToUO);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ReSharper disable once RedundantAssignment
        public void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            OnPropertyChanged(propertyName);
        }

        #endregion
    }
}