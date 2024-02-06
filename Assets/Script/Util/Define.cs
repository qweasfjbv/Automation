using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum TerrainEnum { 
    BLOCK = -1,
    BUILDABLE = 0,
    COPPER = 1,
    IRON,
    GOLD,
    COAL,
    QUARTZ,
        ROCK
    }

    public enum SoundType {
        BUILD,
        BUILDFAIL,
        BUTTON1,
        SUCCESS,
        TIMER
    }

    public enum FactoryType {
        AIR,
        ASSEMBLER,
        DRILL,
        REFINERY,
        SMELTER,
        TRANSPORT,
        WELDING
    }


}
