using System;

namespace ClassicAssist.Data.Regions
{
    [Flags]
    public enum RegionAttributes
    {
        None = 0,
        Guarded = 1 << 1,
        Jail = 1 << 2,
        Wilderness = 1 << 3,
        Dungeon = 1 << 4,
        Special = 1 << 5,
        Default = 1 << 6
    }
}