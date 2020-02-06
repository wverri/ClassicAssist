using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ClassicAssist.Launcher.Properties;
using Application = System.Windows.Application;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ClassicAssist.Launcher
{
    public class MainViewModel : BaseViewModel
    {
        private ICommand _checkforUpdateCommand;
        private ObservableCollection<string> _clientPaths = new ObservableCollection<string>();
        private ObservableCollection<string> _dataPaths = new ObservableCollection<string>();
        private ICommand _selectClientPathCommand;
        private ICommand _selectDataPathCommand;
        private string _selectedClientPath;
        private string _selectedDataPath;
        private ShardEntry _selectedShard;
        private ObservableCollection<ShardEntry> _shardEntries = new ObservableCollection<ShardEntry>();
        private ICommand _showShardsWindowCommand;
        private ICommand _startCommand;

        public MainViewModel()
        {
            ShardManager manager = ShardManager.GetInstance();

            manager.Shards.CollectionChanged += ( sender, args ) => { ShardEntries = manager.Shards; };

            ShardEntries = manager.Shards;
        }

        public ICommand CheckForUpdateCommand =>
            _checkforUpdateCommand ?? ( _checkforUpdateCommand = new RelayCommand( CheckForUpdate, UpdaterExists ) );

        public ObservableCollection<string> ClientPaths
        {
            get => _clientPaths;
            set => SetProperty( ref _clientPaths, value );
        }

        public ObservableCollection<string> DataPaths
        {
            get => _dataPaths;
            set => SetProperty( ref _dataPaths, value );
        }

        public ICommand SelectClientPathCommand =>
            _selectClientPathCommand ?? ( _selectClientPathCommand = new RelayCommand( SelectClientPath ) );

        public ICommand SelectDataPathCommand =>
            _selectDataPathCommand ?? ( _selectDataPathCommand = new RelayCommand( SelectDataPath ) );

        public string SelectedClientPath
        {
            get => _selectedClientPath;
            set => SetProperty( ref _selectedClientPath, value );
        }

        public string SelectedDataPath
        {
            get => _selectedDataPath;
            set => SetProperty( ref _selectedDataPath, value );
        }

        public ShardEntry SelectedShard
        {
            get => _selectedShard;
            set => SetProperty( ref _selectedShard, value );
        }

        public ObservableCollection<ShardEntry> ShardEntries
        {
            get => _shardEntries;
            set => SetProperty( ref _shardEntries, value );
        }

        public ICommand ShowShardsWindowCommand =>
            _showShardsWindowCommand ?? ( _showShardsWindowCommand = new RelayCommand( ShowShardsWindow, o => true ) );

        public ICommand StartCommand =>
            _startCommand ?? ( _startCommand = new RelayCommandAsync( Start,
                o => !string.IsNullOrEmpty( SelectedClientPath ) && !string.IsNullOrEmpty( SelectedDataPath ) ) );

        private static bool UpdaterExists( object arg )
        {
            return File.Exists( Path.Combine( Environment.CurrentDirectory, "ClassicAssist.Updater.exe" ) );
        }

        private static void CheckForUpdate( object obj )
        {
            string updaterPath = Path.Combine( Environment.CurrentDirectory, "ClassicAssist.Updater.exe" );

            if ( File.Exists( updaterPath ) )
            {
                Process.Start( updaterPath );
            }
        }

        private void SelectClientPath( object obj )
        {
            OpenFileDialog ofd =
                new OpenFileDialog
                {
                    CheckFileExists = true,
                    Multiselect = false,
                    Filter = "ClassicUO.exe|ClassicUO.exe",
                    Title = Resources.Select_a_client
                };

            bool? result = ofd.ShowDialog();

            if ( !result.HasValue || !result.Value )
            {
                return;
            }

            if ( !ClientPaths.Contains( ofd.FileName ) )
            {
                ClientPaths.Add( ofd.FileName );
            }

            SelectedClientPath = ofd.FileName;
        }

        private void SelectDataPath( object obj )
        {
            FolderBrowserDialog folderBrowserDialog =
                new FolderBrowserDialog
                {
                    Description = Resources.Select_your_Ultima_Online_directory, ShowNewFolderButton = false
                };
            DialogResult result = folderBrowserDialog.ShowDialog();

            if ( result != DialogResult.OK )
            {
                return;
            }

            if ( !DataPaths.Contains( folderBrowserDialog.SelectedPath ) )
            {
                DataPaths.Add( folderBrowserDialog.SelectedPath );
            }

            SelectedDataPath = folderBrowserDialog.SelectedPath;
        }

        /*
         * Command line parameter documentation...
         * https://github.com/andreakarasho/ClassicUO/wiki/Distribuite-ClassicUO
         * https://github.com/andreakarasho/ClassicUO/wiki/Launch-Arguments
         */
        private async Task Start( object obj )
        {
            IPAddress ip = await ResolveAddress( SelectedShard.Address );

            if ( ip == null )
            {
                MessageBox.Show( Resources.Unable_to_resolve_shard_hostname_ );
                return;
            }

            StringBuilder args = new StringBuilder();

            args.Append( $"-plugins \"{Path.Combine( Environment.CurrentDirectory, "ClassicAssist.dll" )}\" " );
            args.Append( $"-ip \"{ip}\" -port \"{SelectedShard.Port}\" " );
            args.Append( $"-uopath \"{SelectedDataPath}\" " );

            ProcessStartInfo psi = new ProcessStartInfo
            {
                WorkingDirectory =
                    Path.GetDirectoryName( SelectedClientPath ) ?? throw new InvalidOperationException(),
                FileName = SelectedClientPath,
                Arguments = args.ToString(),
                UseShellExecute = true
            };

            Process p = Process.Start( psi );

            if ( !p.HasExited )
            {
                Application.Current.Shutdown( 0 );
            }
        }

        private static async Task<IPAddress> ResolveAddress( string hostname )
        {
            IPHostEntry hostentry = await Dns.GetHostEntryAsync( hostname );

            return hostentry?.AddressList[0];
        }

        private void ShowShardsWindow( object obj )
        {
            ShardsWindow window = new ShardsWindow();
            window.ShowDialog();

            if ( window.DataContext is ShardsViewModel vm && vm.DialogResult == DialogResult.OK )
            {
                SelectedShard = vm.SelectedShard;
            }
        }
    }
}