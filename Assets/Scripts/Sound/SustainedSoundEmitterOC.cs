using UnityEngine;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SustainedSoundEmitterOC : MonoBehaviour
    {
        public float InertDuration;
        public  SustainedSoundKind Kind;
        private float _lastUpdateTime;
        private AudioSource _source;

        void Start()
        {
            _source = GetComponent<AudioSource>();
            _source.Pause();
        }

        public void Play()
        {
            _lastUpdateTime = Time.time;
            if (!_source.isPlaying)
            {
                _source.Play();
            }
        }

        void Update()
        {
            if (Time.time - _lastUpdateTime > InertDuration)
            {
                _source.Stop();
            }
        }
    }
}