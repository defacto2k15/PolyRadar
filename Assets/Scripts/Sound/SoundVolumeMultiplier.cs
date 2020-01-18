using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Sound
{
    public class SoundVolumeMultiplier
    {
        private AudioSource _source;
        private float _currentMultiplicationValue;

        public SoundVolumeMultiplier(AudioSource source, float currentMultiplicationValue)
        {
            Assert.IsNotNull(source);
            _source = source;
            _currentMultiplicationValue = currentMultiplicationValue;
        }

        public void ChangeMultiplication(float newMultiplier)
        {
            Assert.IsTrue(newMultiplier > 0.05f);
            var changeFactor = newMultiplier / _currentMultiplicationValue;
            _currentMultiplicationValue = newMultiplier;
            _source.volume *= changeFactor;
        }
    }
}