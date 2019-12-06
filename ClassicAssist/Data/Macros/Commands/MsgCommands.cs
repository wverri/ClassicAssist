using ClassicAssist.UO.Data;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class MsgCommands
    {
        private const int DEFAULT_SPEAK_HUE = 34;

        public static void Msg( string message, int hue = DEFAULT_SPEAK_HUE )
        {
            UOC.Speak( message, hue );
        }

        public static void YellMsg( string message )
        {
            UOC.Speak( message, DEFAULT_SPEAK_HUE, JournalSpeech.Yell );
        }

        public static void WhisperMsg( string message )
        {
            UOC.Speak( message, DEFAULT_SPEAK_HUE, JournalSpeech.Whisper );
        }

        public static void EmoteMsg( string message )
        {
            UOC.Speak( message, DEFAULT_SPEAK_HUE, JournalSpeech.Emote );
        }

        public static void GuildMsg( string message )
        {
            UOC.Speak( message, DEFAULT_SPEAK_HUE, JournalSpeech.Guild );
        }

        public static void AllyMsg( string message )
        {
            UOC.Speak( message, DEFAULT_SPEAK_HUE, JournalSpeech.Alliance );
        }

        public static void PartyMsg( string message )
        {
            UOC.PartyMessage( message );
        }

        public static void HeadMsg( string message, object obj = null, int hue = DEFAULT_SPEAK_HUE )
        {
            int serial = AliasCommands.ResolveSerial( obj );

            UOC.OverheadMessage( message, hue, serial );
        }
    }
}