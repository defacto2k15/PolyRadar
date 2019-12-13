﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Vehicles;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.RadarBattleground
{
    public class BattlegroundMasterScriptOC : MonoBehaviour
    {
        public Vector2Int BattlegroundTargetSize;
        public Camera BattlegroundCamera;
        public StaticBattlegroundPropsRootScriptOC StaticBattlegroundPropsRoot;
        public BattlegroundVehiclesRootOC VehiclesRootOC;
        public BattlegroundPatternMapsManagerOC PatternMapsManager;

        private RenderTexture _battlegroundTargetColorTexture;
        private RenderTexture _battlegroundTargetDepthTexture;
        private RenderTexture _markersTexture;

        public void Start()
        {
            _battlegroundTargetColorTexture = new RenderTexture(BattlegroundTargetSize.x, BattlegroundTargetSize.y, 0, RenderTextureFormat.ARGB32);
            _battlegroundTargetColorTexture.Create();
            _battlegroundTargetDepthTexture = new RenderTexture(BattlegroundTargetSize.x, BattlegroundTargetSize.y, 24, RenderTextureFormat.RFloat);
            _battlegroundTargetDepthTexture.Create();
            _markersTexture = new RenderTexture(BattlegroundTargetSize.x, BattlegroundTargetSize.y, 0, RenderTextureFormat.ARGB32);
            _markersTexture.Create();
            BattlegroundCamera.enabled = false;

            VehiclesRootOC.SetVehiclesEnabled(false);
            ForceColorUpdateInAllChildren();
            StaticBattlegroundPropsRoot.EnableBrightMarginMaterial();
            PatternMapsManager.GeneratePatternMaps(RenderBattleground());
            StaticBattlegroundPropsRoot.EnableBlackMaterial();
            VehiclesRootOC.SetVehiclesEnabled(true);
        }

        private void ForceColorUpdateInAllChildren()
        {
            GetComponentsInChildren<MaterialPropertyBlockColorSetterOC>().ToList().ForEach(c=>c.UpdateColor());
        }

        public bool InitializationComplete => _battlegroundTargetColorTexture != null && PatternMapsManager.InitializationComplete;

        public BattlegroundTargetTextures RenderBattleground()
        {
            BattlegroundCamera.enabled = true;
            VehiclesRootOC.SetMarkersVisible(false);
            RenderPropsView();
            VehiclesRootOC.SetMarkersVisible(true);
            RenderMarkersView();
            BattlegroundCamera.enabled =false;

            return new BattlegroundTargetTextures()
            {
                ColorTexture = _battlegroundTargetColorTexture,
                DepthTexture = _battlegroundTargetDepthTexture,
                MarkersTexture = _markersTexture
            };
        }

        private void RenderPropsView()
        {
            BattlegroundCamera.depthTextureMode = DepthTextureMode.Depth;
            BattlegroundCamera.SetTargetBuffers(new[] {_battlegroundTargetColorTexture.colorBuffer, _battlegroundTargetDepthTexture.colorBuffer},
                _battlegroundTargetDepthTexture.depthBuffer);
            BattlegroundCamera.Render();
        }

        private void RenderMarkersView()
        {
            BattlegroundCamera.SetTargetBuffers(new[] {_markersTexture.colorBuffer}, _battlegroundTargetDepthTexture.depthBuffer);
            BattlegroundCamera.Render();
        }
    }

    public class BattlegroundTargetTextures
    {
        public RenderTexture ColorTexture;
        public RenderTexture DepthTexture;
        public RenderTexture MarkersTexture;
    }
}