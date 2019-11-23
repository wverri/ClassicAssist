using System;

namespace ClassicAssist.UO.Data
{
    public enum Notoriety : byte
    {
        Invalid,
        Innocent,
        Ally,
        Attackable,
        Criminal,
        Enemy,
        Murderer,
        Invulnerable,
        Unknown
    }

    [Flags]
    public enum MobileStatus : byte
    {
        None = 0x00,
        Frozen = 0x01,
        Female = 0x02,
        Flying = 0x04,
        Invulnerable = 0x08,
        IgnoreMobiles = 0x10,
        Unknown = 0x20,
        WarMode = 0x40,
        Hidden = 0x80,
    }
}