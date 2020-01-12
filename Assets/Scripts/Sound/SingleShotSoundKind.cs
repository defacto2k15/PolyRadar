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

        TargetIsFoe,
        TargetIsFriend,
        
        ModeChange

    }
        
    public enum PerpetualSoundKind
    {
        RocketIdle,
        RadarBackground,

        OscilloscopeBackground,
        SynchronizedLines,
        OscilloscopeMisaligned,
        OscilloscopeAligned,

    }

    public enum SustainedSoundKind
    {
        RocketDirectionChange,
        //SynchronizedLines,
        //OscilloscopeMisaligned,
        //OscilloscopeAligned,
    }
}
