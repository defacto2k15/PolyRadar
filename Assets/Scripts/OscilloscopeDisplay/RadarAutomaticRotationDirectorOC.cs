using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OscilloscopeDisplay
{
    public class RadarAutomaticRotationDirectorOC : MonoBehaviour
    {
        [Range(0,360)]
        public float RotationSpeedInDegreesPerSecond;
        public RadarBeamManagerOC BeamManager;

        public void Update()
        {
            BeamManager.BeamAngleInDegrees += RotationSpeedInDegreesPerSecond * Time.deltaTime;
        }
    }
}
