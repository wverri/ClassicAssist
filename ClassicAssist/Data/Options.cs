using System.ComponentModel;
using System.IO;
using System.Linq;
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
        private bool _useDeathScreenWhilstHidden;
        private string _name;
        private static string _profilePath;
        private const string DEFAULT_SETTINGS_FILENAME = "settings.json";

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

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

        public bool UseDeathScreenWhilstHidden
        {
            get => _useDeathScreenWhilstHidden;
            set => SetProperty(ref _useDeathScreenWhilstHidden, value);
        }

        public static void Save( string startupPath )
        {
            BaseViewModel[] instances = BaseViewModel.Instances;

            JObject obj = new JObject { { "Name", DEFAULT_SETTINGS_FILENAME } };


            foreach ( BaseViewModel instance in instances )
            {
                if ( instance is ISettingProvider settingProvider )
                {
                    settingProvider.Serialize( obj );
                }
            }

            File.WriteAllText( IOPath.Combine( _profilePath, DEFAULT_SETTINGS_FILENAME ), obj.ToString() );
        }

        public static void Load( string startupPath, Options options )
        {
            BaseViewModel[] instances = BaseViewModel.Instances;

            _profilePath = IOPath.Combine( startupPath, "Profiles" );

            if ( !Directory.Exists( _profilePath ) )
            {
                Directory.CreateDirectory( _profilePath );
            }

            JObject json = new JObject();

            if ( File.Exists( IOPath.Combine( _profilePath, DEFAULT_SETTINGS_FILENAME ) ) )
            {
                json = JObject.Parse(File.ReadAllText(IOPath.Combine(_profilePath, DEFAULT_SETTINGS_FILENAME)));
            }

            CurrentOptions.Name = json["Name"]?.ToObject<string>() ?? DEFAULT_SETTINGS_FILENAME;

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

        public static string[] GetProfiles()
        {
            return Directory.EnumerateFiles( _profilePath, "*.json" ).ToArray();
        }
    }
}