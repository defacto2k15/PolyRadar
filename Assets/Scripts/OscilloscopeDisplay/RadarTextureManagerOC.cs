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
        public OscilloscopeIntensityTextureContainerOC IntensityTextureContainer;
        public Material RadarTextureUpdaterMaterial;

        [Range(0,360)]
        public float BeamAngleInDegrees;

        [Range(0,1)]
        public float AnnealingMultiplierPerSecond;
        [Range(-1,1)]
        public float AnnealingOffsetPerSecond;

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

                    var displayingMaterial = GetComponent<MeshRenderer>().material;
                    displayingMaterial.SetTexture("_BackgroundTexture", patternColorTexture);
                    var battlegroundTextures = BattlegroundMasterScript.RenderBattleground();
                    displayingMaterial.SetTexture("_BattlegroundMarkersTexture", battlegroundTextures.MarkersTexture);
                }
                , update: () =>
                {
                    RadarIndicatorObject.transform.localRotation = Quaternion.Euler(RadarIndicatorObject.transform.localEulerAngles.x, -BeamAngleInDegrees - 90,
                        RadarIndicatorObject.transform.localEulerAngles.z);
                    AddBeamDataToOscilloscope();
                    var battlegroundTextures = BattlegroundMasterScript.RenderBattleground();
                    RadarTextureUpdaterMaterial.SetTexture("_BattlegroundColorTexture", battlegroundTextures.ColorTexture);
                    RadarTextureUpdaterMaterial.SetTexture("_BattlegroundDepthTexture", battlegroundTextures.DepthTexture);
                    RadarTextureUpdaterMaterial.SetTexture("_BattlegroundMarkersTexture", battlegroundTextures.MarkersTexture);
                    _previousBeamAngleInDegrees = BeamAngleInDegrees;
                });
        }

        private void AddBeamDataToOscilloscope()
        {
            var angle = Mathf.Repeat(BeamAngleInDegrees, 360);
            if (angle > 360 / 2f)
            {
                angle -= 360f;
            }

            RadarTextureUpdaterMaterial.SetFloat("_BeamAngleInDegrees", angle);
            RadarTextureUpdaterMaterial.SetFloat("_BeamAngleInDegreesDelta", BeamAngleInDegrees - _previousBeamAngleInDegrees);

            UpdateAnnealingData();
            IntensityTextureContainer.ApplyTransformingMaterial(RadarTextureUpdaterMaterial);
        }

        private void UpdateAnnealingData()
        {
            var currentFps = 1f / Time.deltaTime;

            var perFrameAnnealingSpeedMultiplier = Mathf.Pow(AnnealingMultiplierPerSecond, 1 / currentFps);
            var perFrameAnnealingSpeedOffset = AnnealingOffsetPerSecond / currentFps;

            RadarTextureUpdaterMaterial.SetFloat("_AnnealingSpeedMultiplier", perFrameAnnealingSpeedMultiplier);
            RadarTextureUpdaterMaterial.SetFloat("_AnnealingSpeedOffset", perFrameAnnealingSpeedOffset);
        }
    }
}
