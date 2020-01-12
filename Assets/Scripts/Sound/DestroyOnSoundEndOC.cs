using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class DestroyOnSoundEndOC : MonoBehaviour
    {
        private AudioSource _source;

        void Start()
        {
            _source = GetComponent<AudioSource>();
            StartCoroutine(DestroyOnAudioEnd());
        }

        private  IEnumerator DestroyOnAudioEnd()
        {
            if (!_source.isPlaying)
            {
                yield return new WaitUntil(() => _source.isPlaying);
            }

            yield return new WaitForSeconds(_source.clip.length);
            GameObject.Destroy(this.gameObject);
        }
    }
}
