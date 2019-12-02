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
        public OscilloscopeIntensityTextureOC IntensityTextureOc;
        public Material AnnealingMaterial;
        [Range(0,1)]
        public float AnnealingMultiplierPerSecond;
        [Range(-1,1)]
        public float AnnealingOffsetPerSecond;

        public void Update()
        {
            var intensityTexture = IntensityTextureOc.IntensityTexture;
            var workCopy = IntensityTextureOc.RetrieveIntensityTextureCopy();

            AnnealingMaterial.SetTexture("_OscilloscopeIntensityTex", workCopy);
            AnnealingMaterial.SetFloat("_AnnealingSpeedMultiplier",AnnealingMultiplierPerSecond);
            AnnealingMaterial.SetFloat("_AnnealingSpeedOffset", AnnealingOffsetPerSecond);
            Graphics.Blit(workCopy, intensityTexture, AnnealingMaterial);
        }
    }
}
