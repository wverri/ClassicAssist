using System.Collections.ObjectModel;
using ClassicAssist.Data.Dress;

namespace ClassicAssist.Data.Scavenger
{
    public class ScavengerManager
    {
        private static ScavengerManager _instance;
        private static readonly object _instanceLock = new object();

        public ObservableCollection<ScavengerEntry> Items { get; set; }

        private ScavengerManager()
        {
            
        }

        public static ScavengerManager GetInstance()
        {
            // ReSharper disable once InvertIf
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ScavengerManager();
                    }
                }
            }

            return _instance;
        }

    }
}