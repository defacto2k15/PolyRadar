using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Sound;
using UnityEngine;

namespace Assets.Scripts.Rocket
{
    public class RocketSoundOC : MonoBehaviour
    {
        public SoundSourceMasterOC SoundMaster;
        private SustainedSoundEmitterOC _idleAudioSource;

        public void StartIdleSound()
        {
            _idleAudioSource = SoundMaster.StartSustainedSound(SustainedSoundKind.RocketIdle);
        }

        public void StartRocketStartSound()
        {
            SoundMaster.StartOneShotSound(SingleShotSoundKind.RocketStart);
        }

        void OnDestroy()
        {
            _idleAudioSource.EndSound();
        }

        public void StartRocketExplosionSound()
        {
            SoundMaster.StartOneShotSound(SingleShotSoundKind.RocketExplosion);
        }
    }
}
