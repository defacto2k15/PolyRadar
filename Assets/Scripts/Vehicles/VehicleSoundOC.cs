using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Sound;
using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class VehicleSoundOC : MonoBehaviour
    {
        public SoundSourceMasterOC SoundMaster;

        public void StartBlipSound()
        {
            SoundMaster.StartOneShotSound(SingleShotSoundKind.Blip);
        }

        public void StartVehicleExplosionSound()
        {
            SoundMaster.StartOneShotSound(SingleShotSoundKind.TargetExplosion);
        }
    }
}
