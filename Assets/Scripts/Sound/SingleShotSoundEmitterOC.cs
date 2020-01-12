using UnityEngine;

namespace Assets.Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SingleShotSoundEmitterOC : MonoBehaviour
    {
        public SingleShotSoundKind Kind;
    }
}