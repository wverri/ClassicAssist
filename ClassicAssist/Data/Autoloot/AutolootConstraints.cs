using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ClassicAssist.Annotations;

namespace ClassicAssist.Data.Autoloot
{
    public class AutolootConstraints : INotifyPropertyChanged
    {
        private int _cliloc;
        private AutolootConstraintType _constraintType;
        private string _name;
        private string _operator = "==";
        private int _value;

        public int Cliloc
        {
            get => _cliloc;
            set => SetProperty( ref _cliloc, value );
        }

        public AutolootConstraintType ConstraintType
        {
            get => _constraintType;
            set => SetProperty( ref _constraintType, value );
        }

        public string Name
        {
            get => _name;
            set => SetProperty( ref _name, value );
        }

        public string Operator
        {
            get => _operator;
            set => SetProperty( ref _operator, value );
        }

        public int Value
        {
            get => _value;
            set => SetProperty( ref _value, value );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        // ReSharper disable once RedundantAssignment
        public virtual void SetProperty<T>( ref T obj, T value, [CallerMemberName] string propertyName = "" )
        {
            obj = value;
            OnPropertyChanged( propertyName );
            CommandManager.InvalidateRequerySuggested();
        }
    }
}