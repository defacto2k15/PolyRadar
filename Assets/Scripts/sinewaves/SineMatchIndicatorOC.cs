using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Sound;
using UnityEngine;

namespace Assets.Scripts.sinewaves
{
    public class SineMatchIndicatorOC : MonoBehaviour
    {
        public SoundSourceMasterOC SoundSourceMaster;
        public SineComparatorScript Comparator;
        private Animator _animator;
        private float _time;
        private PerpetualSoundEmitterOC _matchingSoundEmitterOc;

        public void Start()
        {
            _matchingSoundEmitterOc = SoundSourceMaster.StartPerpetualSound(PerpetualSoundKind.SynchronizedLines);
            _animator = GetComponent<Animator>();
        }

        public void Update()
        {
            var time = Mathf.Clamp01(Comparator.FrameCountSinceMatch / (float)Comparator.AttunementFrames);
            time = Mathf.Min(time, 0.999f);
            _animator.SetFloat("AnimationTime", time);
            _matchingSoundEmitterOc.ChangeVolumeMultiplication(Mathf.Clamp01(time+0.075f));
        }
    }
}
