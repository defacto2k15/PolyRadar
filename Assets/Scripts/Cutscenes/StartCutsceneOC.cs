using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Assets.Scripts.Cutscenes
{
    [RequireComponent(typeof(PlayableDirector))]
    class StartCutsceneOC : MonoBehaviour
    {
        private PlayableDirector _playableDirector;

        void Start()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            _playableDirector.played += (x) => Debug.Log("Played");
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                _playableDirector.Play();
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                _playableDirector.time = _playableDirector.duration;
            }
        }
    }
}
