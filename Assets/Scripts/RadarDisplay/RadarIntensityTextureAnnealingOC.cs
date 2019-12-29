using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OscilloscopeDisplay
{
    public class RadarIntensityTextureAnnealingOC : MonoBehaviour
    {
        public RadarIntensityTextureContainerOC IntensityTextureContainerOc;
        public Material AnnealingMaterial;
        [Range(0,1)]
        public float AnnealingMultiplierPerSecond;
        [Range(-1,1)]
        public float AnnealingOffsetPerSecond;

        public TimeProviderGo TimeProvider;

        public void Update()
        {
            var currentFps = 1f / TimeProvider.DeltaTime;

            var perFrameAnnealingSpeedMultiplier = Mathf.Pow(AnnealingMultiplierPerSecond, 1 / currentFps);
            var perFrameAnnealingSpeedOffset = AnnealingOffsetPerSecond / currentFps;

            AnnealingMaterial.SetFloat("_AnnealingSpeedMultiplier", perFrameAnnealingSpeedMultiplier);
            AnnealingMaterial.SetFloat("_AnnealingSpeedOffset", perFrameAnnealingSpeedOffset);
            IntensityTextureContainerOc.ApplyTransformingMaterial(AnnealingMaterial);
        }
    }
}
