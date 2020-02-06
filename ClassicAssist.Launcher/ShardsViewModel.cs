using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ClassicAssist.Launcher
{
    public class ShardsViewModel : BaseViewModel
    {
        private readonly ShardManager _manager;
        private ICommand _cancelCommand;
        private ICommand _okCommand;
        private ShardEntry _selectedShard;
        private ObservableCollection<ShardEntry> _shards;

        public ShardsViewModel()
        {
            _manager = ShardManager.GetInstance();
            Shards = _manager.Shards;

            foreach ( ShardEntry shard in Shards )
            {
                Task.Run( async () =>
                {
                    if ( !shard.HasStatusProtocol )
                    {
                        return "-";
                    }

                    string status = await GetStatus( shard );

                    return status;
                } ).ContinueWith( t =>
                {
                    if ( !string.IsNullOrEmpty( shard.StatusRegex ) )
                    {
                        Match matches = Regex.Match( t.Result, shard.StatusRegex );

                        shard.Status = matches.Success ? matches.Groups[1].Value : "-";
                    }
                    else
                    {
                        shard.Status = t.Result;
                    }

                    NotifyPropertyChanged( nameof( Shards ) );
                } );
            }
        }

        public ICommand CancelCommand => _cancelCommand ?? ( _cancelCommand = new RelayCommand( Cancel, o => true ) );

        public DialogResult DialogResult { get; set; }
        public ICommand OKCommand => _okCommand ?? ( _okCommand = new RelayCommand( OK, o => true ) );

        public ShardEntry SelectedShard
        {
            get => _selectedShard;
            set => SetProperty( ref _selectedShard, value );
        }

        public ObservableCollection<ShardEntry> Shards
        {
            get => _shards;
            set => SetProperty( ref _shards, value );
        }

        private void OK( object obj )
        {
            DialogResult = DialogResult.OK;
        }

        public async Task<string> GetStatus( ShardEntry shard )
        {
            if ( !shard.HasStatusProtocol )
            {
                return "Unknown";
            }

            TcpClient client = new TcpClient();
            await client.ConnectAsync( shard.Address, shard.Port );

            if ( !client.Connected )
            {
                return "Unknown";
            }

            byte[] packet = { 0x7F, 0x00, 0x00, 0x7F, 0xF1, 0x00, 0x04, 0xFF };
            client.Client.Send( packet );

            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];

            stream.ReadTimeout = 60000;
            await stream.ReadAsync( buffer, 0, buffer.Length );

            client.Close();

            string status = Encoding.ASCII.GetString( buffer ).TrimEnd( '\0' );

            return status;
        }

        private void Cancel( object obj )
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}