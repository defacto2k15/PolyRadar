using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Sound
{
    public enum SingleShotSoundKind
    {
        Blip,
        TargetChange,
        RocketStart,
        RocketExplosion,
        TargetExplosion,
        RocketDirectionChange,

        TargetIsFoe,
        TargetIsFriend,
        
        ModeChange

    }
        
    public enum SustainedSoundKind
    {
        RocketIdle,
        RadarBackground,

        OscilloscopeBackground,
        SynchronizedLines,
        OscilloscopeMisaligned,
        OscilloscopeAligned,

    }
}
