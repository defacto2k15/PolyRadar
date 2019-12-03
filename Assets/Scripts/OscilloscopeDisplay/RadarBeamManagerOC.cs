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
        public GameObject RadarIndicatorObject;
        public OscilloscopeIntensityTextureContainerOC IntensityTextureContainer;
        public Material RadarBeamApplyingMaterial;
        public int BeamIntensityTextureLength = 0;
        public float RadarWholeRotationTimeInSeconds = 2;

        [Range(0,360)]
        public float BeamAngleInDegrees;

        private float _previousBeamAngleInDegrees;
        private Texture2D _beamIntensityTexture;

        [Range(0, 200)] public float DebugDistanceToClosestTargetMultiplier = 20;

        [Range(0, 20)] public float DebugTargetIntensityMultiplier = 1;

        public void Start()
        {
            _beamIntensityTexture = new Texture2D(1, BeamIntensityTextureLength, TextureFormat.ARGB32, false);
        }

        public void Update()
        {
            RadarIndicatorObject.transform.localRotation = Quaternion.Euler(RadarIndicatorObject.transform.localEulerAngles.x, -BeamAngleInDegrees-90, RadarIndicatorObject.transform.localEulerAngles.z);
            var beamIntensityArray = CollectBeamIntensityData();
            FillBeamIntensityTexture(beamIntensityArray);
            AddBeamDataToOscilloscope();
            _previousBeamAngleInDegrees = BeamAngleInDegrees;
        }

        private float[] CollectBeamIntensityData()
        {
            var outArray = new float[BeamIntensityTextureLength];
            var debugRadarTargets = new List<Vector2>()
            {
                new Vector2(0.3f, 0.3f),
                new Vector2(0.3f, 0.5f),
                //new Vector2(0.7f, 0.8f),
                //new Vector2(-0.7f, 0.8f),
                //new Vector2(-0.7f, -0.8f),
                //new Vector2(-0.7f, 0.1f),
            };
            for (int i = 0; i < BeamIntensityTextureLength; i++)
            {
                var rAndPhi = new Vector2(i/((float)BeamIntensityTextureLength), BeamAngleInDegrees*Mathf.Deg2Rad);
                var coords = MathUtils.PolarToCartesian(rAndPhi);
                var distanceToClosestTarget = debugRadarTargets.Select(c => Vector2.Distance(coords, c)).Min();
                var intensityFromClosest = Mathf.Max(0, 1 - distanceToClosestTarget*DebugDistanceToClosestTargetMultiplier) * DebugTargetIntensityMultiplier;

                outArray[i] = intensityFromClosest;
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
            IntensityTextureContainer.ApplyTransformingMaterial(RadarBeamApplyingMaterial);
        }
    }
}
