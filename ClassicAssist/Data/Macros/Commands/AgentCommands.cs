using System.Linq;
using ClassicAssist.Data.Dress;
using ClassicAssist.Resources;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class AgentCommands
    {
        [CommandsDisplay( Category = "Agents", Description = "Dress all items in the specified dress agent." )]
        public static void Dress( string name )
        {
            DressManager manager = DressManager.GetInstance();

            DressAgentEntry dressAgentEntry = manager.Items.FirstOrDefault( dae => dae.Name == name );

            if ( dressAgentEntry == null )
            {
                UOC.SystemMessage( string.Format( Strings.Unknown_dress_agent___0___, name ) );
                return;
            }

            dressAgentEntry.Action( dressAgentEntry );
        }

        [CommandsDisplay( Category = "Agents", Description = "Undress all items in the specified dress agent." )]
        public static void Undress( string name )
        {
            DressManager manager = DressManager.GetInstance();

            DressAgentEntry dressAgentEntry = manager.Items.FirstOrDefault( dae => dae.Name == name );

            if ( dressAgentEntry == null )
            {
                UOC.SystemMessage( string.Format( Strings.Unknown_dress_agent___0___, name ) );
                return;
            }

            dressAgentEntry.Undress();
        }
    }
}