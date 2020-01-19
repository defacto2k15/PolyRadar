using System;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class PerpetualSoundEmitterOC : MonoBehaviour
    {
        public PerpetualSoundKind Kind;
        private SoundVolumeMultiplier _multiplier;

        void Awake()
        {
            _multiplier = new SoundVolumeMultiplier(GetComponent<AudioSource>(), 1);
        }

        public void EndSound()
        {
            Destroy(this.gameObject);
        }

        public void ChangeVolumeMultiplication(float newMultiplier)
        {
            _multiplier.ChangeMultiplication(newMultiplier);
        }
    }
}