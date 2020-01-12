using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    public class SoundSourceMasterOC : MonoBehaviour
    {
        public List<SingleShotSoundEmitterOC> SingleShotSoundEmitterPrefabs;
        public List<SustainedSoundEmitterOC> SustainedSoundEmitterPrefabs;

        public void StartOneShotSound(SingleShotSoundKind kind)
        {
            var prefab = SingleShotSoundEmitterPrefabs.First(c => c.Kind == kind);
            Instantiate(prefab, this.transform, true);
        }

        public SustainedSoundEmitterOC StartSustainedSound(SustainedSoundKind kind)
        {
            var prefab = SustainedSoundEmitterPrefabs.First(c => c.Kind == kind);
            return Instantiate(prefab, this.transform, true).GetComponent<SustainedSoundEmitterOC>();
        }
    }
}
