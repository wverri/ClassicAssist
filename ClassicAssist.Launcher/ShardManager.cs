using System.CodeDom;
using System.Collections.ObjectModel;

namespace ClassicAssist.Launcher
{
    public class ShardManager
    {
        private static ShardManager _instance;
        private static readonly object _lock = new object();
        public ObservableCollection<ShardEntry> Shards { get; set; } = new ObservableCollection<ShardEntry>();

        private const string RUNUO_REGEX = @".*Clients=(\d+),.*";
        private ShardManager()
        {
            Shards.Add( new ShardEntry
            {
                Name = "UOGamers: Demise",
                Address = "login.uodemise.com",
                Port = 2593,
                IsPreset = true,
                StatusRegex = RUNUO_REGEX
            } );

            Shards.Add( new ShardEntry
            {
                Name = "UOGamers: Hybrid",
                Address = "login.uohybrid.com",
                Port = 2593,
                IsPreset = true,
                StatusRegex = RUNUO_REGEX
            } );

            Shards.Add( new ShardEntry
            {
                Name = "Heritage UO",
                Address = "play.trueuo.com",
                Port = 2593,
                IsPreset = true,
                StatusRegex = RUNUO_REGEX
            } );

            Shards.Add( new ShardEntry
            {
                Name = "UO Forever",
                Address = "login.uoforever.com",
                Port = 2599,
                IsPreset = true,
                StatusRegex = RUNUO_REGEX
            } );

            Shards.Add( new ShardEntry
            {
                Name = "UO Outlands",
                Address = "play.uooutlands.com",
                Port = 2593,
                IsPreset = true,
                HasStatusProtocol = false
            } );
        }

        public static ShardManager GetInstance()
        {
            // ReSharper disable once InvertIf
            if ( _instance == null )
            {
                lock ( _lock )
                {
                    if ( _instance != null )
                    {
                        return _instance;
                    }

                    _instance = new ShardManager();
                    return _instance;
                }
            }

            return _instance;
        }
    }
}