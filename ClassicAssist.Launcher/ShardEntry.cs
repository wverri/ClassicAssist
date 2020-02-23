using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ClassicAssist.Launcher.Annotations;

namespace ClassicAssist.Launcher
{
    public class ShardEntry : INotifyPropertyChanged
    {
        private const string RUNUO_REGEX = @".*Clients=(\d+),.*";

        private string _status;
        public string Address { get; set; }
        public bool HasStatusProtocol { get; set; } = true;
        public bool IsPreset { get; set; } = false;
        public string Name { get; set; }
        public int Port { get; set; }

        public string Status
        {
            get => _status;
            set => SetProperty( ref _status, value );
        }

        public string StatusRegex { get; set; } = RUNUO_REGEX;

        public event PropertyChangedEventHandler PropertyChanged;

        // ReSharper disable once RedundantAssignment
        public virtual void SetProperty<T>( ref T obj, T value, [CallerMemberName] string propertyName = "" )
        {
            obj = value;
            OnPropertyChanged( propertyName );
            CommandManager.InvalidateRequerySuggested();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }
    }
}