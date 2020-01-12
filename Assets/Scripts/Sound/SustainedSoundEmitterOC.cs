using UnityEngine;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SustainedSoundEmitterOC : MonoBehaviour
    {
        public SustainedSoundKind Kind;

        public void EndSound()
        {
            Destroy(this.gameObject);
        }
    }
}