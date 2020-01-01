using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.sinewaves
{
    public class SineMatchIndicatorOC : MonoBehaviour
    {
        public SineComparatorScript Comparator;
        private Animator _animator;
        private float _time;

        public void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Update()
        {
            var time = Mathf.Clamp01(Comparator.FrameCountSinceMatch / (float)Comparator.AttunementFrames);
            time = Mathf.Min(time, 0.999f);
            _animator.SetFloat("AnimationTime", time);
        }
    }
}
