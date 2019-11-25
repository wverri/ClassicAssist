using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using Assistant;
using ClassicAssist.UI.Controls;
using ClassicAssist.UI.Views;

namespace ClassicAssist.UI.ViewModels
{
    public class DebugViewModel : BaseViewModel
    {
        private readonly Dispatcher _dispatcher;
        private ObservableCollection<HexDumpControl> _items = new ObservableCollection<HexDumpControl>();
        private bool _topmost = true;
        private ICommand _viewPlayerEquipmentCommand;

        public DebugViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            Engine.PacketReceivedEvent += OnPacketReceivedEvent;
            Engine.PacketSentEvent += OnPacketSentEvent;
        }

        public ObservableCollection<HexDumpControl> Items
        {
            get => _items;
            set => SetProperty( ref _items, value );
        }

        public bool Topmost
        {
            get => _topmost;
            set => SetProperty( ref _topmost, value );
        }

        public ICommand ViewPlayerEquipmentCommand =>
            _viewPlayerEquipmentCommand ??
            ( _viewPlayerEquipmentCommand = new RelayCommand( ViewPlayerEquipment, o => true ) );

        private void OnPacketSentEvent( byte[] data, int length )
        {
            _dispatcher.Invoke( () =>
            {
                HexDumpControl hd = new HexDumpControl( "Outgoing Packet", data );

                Items.Add( hd );
            } );
        }

        private void OnPacketReceivedEvent( byte[] data, int length )
        {
            _dispatcher.Invoke( () =>
            {
                HexDumpControl hd = new HexDumpControl( "Incoming Packet", data );

                Items.Add( hd );
            } );
        }

        private void ViewPlayerEquipment( object obj )
        {
            EntityCollectionViewer window = new EntityCollectionViewer
            {
                DataContext = new EntityCollectionViewerViewModel( Engine.Player.Equipment ) { Topmost = Topmost }
            };

            window.Show();
        }
    }
}