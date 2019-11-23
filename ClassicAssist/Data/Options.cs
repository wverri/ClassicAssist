using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using ClassicAssist.Annotations;
using ClassicAssist.Misc;
using ClassicAssist.UI.ViewModels;
using Newtonsoft.Json.Linq;
using IOPath = System.IO.Path;

namespace ClassicAssist.Data
{
    public class Options : INotifyPropertyChanged
    {
        private bool _alwaysOnTop;
        private int _lightLevel;
        private bool _actionDelay;
        private int _actionDelayMs;
        private const string SETTINGS_FILENAME = "settings.json";

        public bool AlwaysOnTop
        {
            get => _alwaysOnTop;
            set => SetProperty(ref _alwaysOnTop, value);
        }

        public int LightLevel
        {
            get => _lightLevel;
            set => SetProperty(ref _lightLevel, value);
        }

        public bool ActionDelay
        {
            get => _actionDelay;
            set => SetProperty(ref _actionDelay, value);
        }

        public int ActionDelayMS
        {
            get => _actionDelayMs;
            set => SetProperty(ref _actionDelayMs, value);
        }

        public static Options CurrentOptions { get; set; } = new Options();

        public static void Save( string startupPath )
        {
            BaseViewModel[] instances = BaseViewModel.Instances;

            JObject obj = new JObject();

            foreach ( BaseViewModel instance in instances )
            {
                if ( instance is ISettingProvider settingProvider )
                {
                    settingProvider.Serialize( obj );
                }
            }

            File.WriteAllText( IOPath.Combine( startupPath, SETTINGS_FILENAME ), obj.ToString() );
        }

        public static void Load( string startupPath, Options options )
        {
            BaseViewModel[] instances = BaseViewModel.Instances;

            JObject json = new JObject();

            if ( File.Exists( IOPath.Combine( startupPath, SETTINGS_FILENAME ) ) )
            {
                json = JObject.Parse(File.ReadAllText(IOPath.Combine(startupPath, SETTINGS_FILENAME)));
            }

            foreach ( BaseViewModel instance in instances )
            {
                if ( instance is ISettingProvider settingProvider )
                {
                    settingProvider.Deserialize( json, options );
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public void SetProperty<T>(ref T obj, T value, [CallerMemberName] string propertyName = "")
        {
            obj = value;
            OnPropertyChanged(propertyName);
        }
    }
}