using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarBattleground;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.OscilloscopeDisplay
{
    public class RadarTextureManagerOC : MonoBehaviour
    {
        public GameObject RadarIndicatorObject;
        public BattlegroundMasterScriptOC BattlegroundMasterScript;
        public RadarIntensityTextureContainerOC IntensityTextureContainer;
        public Material RadarTextureUpdaterMaterial;
        public MeshRenderer MeshRenderer;

        [Range(0,360)]
        public float BeamAngleInDegrees;

        [Range(0,1)]
        public float AnnealingMultiplierPerSecond;
        [Range(-1,1)]
        public float AnnealingOffsetPerSecond;

        public TimeProviderGo TimeProvider;

        private float _previousBeamAngleInDegrees;

        private WaitingUpdateLoopBox _updateLoopBox;

        public void Update()
        {
            WaitingUpdateLoopBox.Update(ref _updateLoopBox
                , condition: () => BattlegroundMasterScript.InitializationComplete 
                , initialization: () =>
                {
                    var battlegroundOcclusionTexturesPack = BattlegroundMasterScript.PatternMapsManager.BattlegroundOcclusionTextures;
                    RadarTextureUpdaterMaterial.SetTexture("_OcclusionHeightMap", battlegroundOcclusionTexturesPack.OcclusionHeightMap);
                    RadarTextureUpdaterMaterial.SetTexture("_OcclusionEdges", battlegroundOcclusionTexturesPack.OcclussionEdges);
                    var patternColorTexture = BattlegroundMasterScript.PatternMapsManager.PatternColorTexture;
                    RadarTextureUpdaterMaterial.SetTexture("_BattlegroundPatternTexture", patternColorTexture);

                    MeshRenderer.material.SetTexture("_BattlegroundBackgroundTexture", patternColorTexture);
                    var battlegroundTextures = BattlegroundMasterScript.RenderBattleground();
                    MeshRenderer.material.SetTexture("_BattlegroundMarkersTexture", battlegroundTextures.MarkersTexture);
                }
                , update: () =>
                {
                    RadarIndicatorObject.transform.localRotation = Quaternion.Euler(RadarIndicatorObject.transform.localEulerAngles.x, -BeamAngleInDegrees - 90,
                        RadarIndicatorObject.transform.localEulerAngles.z);
                    AddBeamDataToOscilloscope();

                    BattlegroundMasterScript.UpdateWithBeamSetting(new RadarBeamSetting(_previousBeamAngleInDegrees, BeamAngleInDegrees - _previousBeamAngleInDegrees));
                    var battlegroundTextures = BattlegroundMasterScript.RenderBattleground();
                    RadarTextureUpdaterMaterial.SetTexture("_BattlegroundColorTexture", battlegroundTextures.ColorTexture);
                    RadarTextureUpdaterMaterial.SetTexture("_BattlegroundDepthTexture", battlegroundTextures.DepthTexture);
                    RadarTextureUpdaterMaterial.SetTexture("_BattlegroundMarkersTexture", battlegroundTextures.MarkersTexture);
                    _previousBeamAngleInDegrees = BeamAngleInDegrees;

                    MeshRenderer.material.SetTexture("_MainTex", IntensityTextureContainer.ActiveTexture);
                });
        }

        private void AddBeamDataToOscilloscope()
        {
            var angle = Mathf.Repeat(BeamAngleInDegrees, 360);
            if (angle > 360 / 2f)
            {
                angle -= 360f;
            }

            MeshRenderer.material.SetFloat("_BeamAngleInDegrees", angle);
            RadarTextureUpdaterMaterial.SetFloat("_BeamAngleInDegrees", angle);
            RadarTextureUpdaterMaterial.SetFloat("_BeamAngleInDegreesDelta", BeamAngleInDegrees - _previousBeamAngleInDegrees);

            UpdateAnnealingData();
            IntensityTextureContainer.ApplyTransformingMaterial(RadarTextureUpdaterMaterial);
        }

        private void UpdateAnnealingData()
        {
            var currentFps = 1f / TimeProvider.DeltaTime;

            var perFrameAnnealingSpeedMultiplier = Mathf.Pow(AnnealingMultiplierPerSecond, 1 / currentFps);
            var perFrameAnnealingSpeedOffset = AnnealingOffsetPerSecond / currentFps;

            RadarTextureUpdaterMaterial.SetFloat("_AnnealingSpeedMultiplier", perFrameAnnealingSpeedMultiplier);
            RadarTextureUpdaterMaterial.SetFloat("_AnnealingSpeedOffset", perFrameAnnealingSpeedOffset);
        }
    }

    public class RadarBeamSetting
    {
        private float _beamStart;
        private float _beamDelta;

        public RadarBeamSetting(float beamStart, float beamDelta)
        {
            _beamStart = Mathf.Repeat(beamStart,360);
            _beamDelta = beamDelta;
            Debug.Log(_beamDelta);
        }

        public bool AngleIsInRange(float angle)
        {
            var start = _beamStart;
            var delta = _beamDelta;

            if (delta < 0)
            {
                delta = -delta;
                start = start - delta;
                if (start < 0)
                {
                    start = start + 360;
                }
            }

            angle = Mathf.Repeat(angle, 360);
            var b1 = start < angle && (start+delta)>= angle;
            if (start + delta > 360)
            {
                angle = angle + 360;
                b1 |= start < angle && (start+delta)>= angle;
            }

            return b1;
        }
    }
}
