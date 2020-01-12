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
        public List<PerpetualSoundEmitterOC> PerpetualSoundEmitterPrefabs;
        public List<SustainedSoundEmitterOC> SustainedSoundEmitterPrefabs;
        private List<SustainedSoundEmitterOC> SustainedSoundEmitters;

        public void Start()
        {
            SustainedSoundEmitters = SustainedSoundEmitterPrefabs.Select(c => Instantiate(c, transform, false).GetComponent<SustainedSoundEmitterOC>()).ToList();
        }

        public void StartOneShotSound(SingleShotSoundKind kind)
        {
            var prefab = SingleShotSoundEmitterPrefabs.First(c => c.Kind == kind);
            Instantiate(prefab, this.transform,false);
        }

        public PerpetualSoundEmitterOC StartPerpetualSound(PerpetualSoundKind kind)
        {
            var prefab = PerpetualSoundEmitterPrefabs.First(c => c.Kind == kind);
            return Instantiate(prefab, this.transform, false).GetComponent<PerpetualSoundEmitterOC>();
        }

        public void PlaySustainedSound(SustainedSoundKind kind)
        {
            SustainedSoundEmitters.First(c => c.Kind == kind).Play();
        }
    }
}
