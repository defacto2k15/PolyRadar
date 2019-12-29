using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class TimeProviderGo : MonoBehaviour
    {
        private bool _timeUpdateEnabled = true;
        private float _enabledTimeSinceStart;

        public void Update()
        {
            if (_timeUpdateEnabled)
            {
                _enabledTimeSinceStart = Time.deltaTime;
            }
        }

        public float DeltaTime
        {
            get
            {
                if (_timeUpdateEnabled)
                {
                    return Time.deltaTime;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool TimeUpdateEnabled
        {
            set => _timeUpdateEnabled = value;
        }

        public float TimeSinceStart => _enabledTimeSinceStart;
    }
}
