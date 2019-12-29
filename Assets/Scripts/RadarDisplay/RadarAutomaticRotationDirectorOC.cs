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
        [Range(0,2000)]
        public float RotationSpeedInDegreesPerSecond;
        public RadarTextureManagerOC TextureManager;
        public TimeProviderGo TimeProvider;

        public void Update()
        {
            TextureManager.BeamAngleInDegrees += RotationSpeedInDegreesPerSecond * TimeProvider.DeltaTime;
        }
    }
}
