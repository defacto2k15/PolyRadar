using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.OscilloscopeDisplay
{
    public class RadarBeamManagerOC : MonoBehaviour
    {
        public OscilloscopeIntensityTextureContainerOC IntensityTextureContainer;
        public Material RadarBeamApplyingMaterial;
        public int BeamIntensityTextureLength = 0;
        public float RadarWholeRotationTimeInSeconds = 2;

        [Range(0,360)]
        public float BeamAngleInDegrees;

        [Range(0, 0.2f)] public float BeamIndicatorSize;

        private float _previousBeamAngleInDegrees;
        private Texture2D _beamIntensityTexture;

        public void Start()
        {
            _beamIntensityTexture = new Texture2D(1, BeamIntensityTextureLength, TextureFormat.ARGB32, false);
        }

        public void Update()
        {
            var beamIntensityArray = CollectBeamIntensityData();
            FillBeamIntensityTexture(beamIntensityArray);
            AddBeamDataToOscilloscope();
            _previousBeamAngleInDegrees = BeamAngleInDegrees;
        }

        private float[] CollectBeamIntensityData()
        {
            var outArray = new float[BeamIntensityTextureLength];
            for (int i = 0; i < BeamIntensityTextureLength; i++)
            {
                outArray[i] = (Mathf.Abs(i - BeamIntensityTextureLength / 2) / ((float) BeamIntensityTextureLength)) * 2;
            }

            return outArray;
        }


        private void FillBeamIntensityTexture(float[] beamIntensityArray)
        {
            for (int i = 0; i < BeamIntensityTextureLength; i++)
            {
                _beamIntensityTexture.SetPixel(0,i, new Color(beamIntensityArray[i],0,0,1));
            }
            _beamIntensityTexture.Apply();
        }

        private void AddBeamDataToOscilloscope()
        {
            var angle = Mathf.Repeat(BeamAngleInDegrees, 360);
            if (angle > 360 / 2f)
            {
                angle -= 360f;
            }

            var updateDistance = 360 * RadarWholeRotationTimeInSeconds * Time.deltaTime;
            RadarBeamApplyingMaterial.SetFloat("_UpdateAngleDistanceInDegrees", updateDistance);
            RadarBeamApplyingMaterial.SetFloat("_BeamAngleInDegrees", angle);
            RadarBeamApplyingMaterial.SetFloat("_BeamAngleInDegreesDelta", BeamAngleInDegrees - _previousBeamAngleInDegrees);
            RadarBeamApplyingMaterial.SetTexture("_BeamIntensityTexture", _beamIntensityTexture);
            RadarBeamApplyingMaterial.SetFloat("_BeamIndicatorSize",BeamIndicatorSize);
            IntensityTextureContainer.ApplyTransformingMaterial(RadarBeamApplyingMaterial);
        }
    }
}
