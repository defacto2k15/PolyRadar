﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.RadarBattleground
{
    public class BattlegroundMasterScriptOC : MonoBehaviour
    {
        public Vector2Int BattlegroundTargetSize;
        public Camera BattlegroundCamera;

        private RenderTexture _battlegroundTargetColorTexture;
        private RenderTexture _battlegroundTargetDepthTexture;

        public void Start()
        {
            _battlegroundTargetColorTexture = new RenderTexture(BattlegroundTargetSize.x, BattlegroundTargetSize.y, 0, RenderTextureFormat.ARGB32);
            _battlegroundTargetColorTexture.Create();
            _battlegroundTargetDepthTexture = new RenderTexture(BattlegroundTargetSize.x, BattlegroundTargetSize.y, 24, RenderTextureFormat.RFloat);
            _battlegroundTargetDepthTexture.Create();
            BattlegroundCamera.depthTextureMode = DepthTextureMode.Depth;
            BattlegroundCamera.SetTargetBuffers(new[] {_battlegroundTargetColorTexture.colorBuffer, _battlegroundTargetDepthTexture.colorBuffer},
                _battlegroundTargetDepthTexture.depthBuffer);
            BattlegroundCamera.enabled = false;
        }

        public bool InitializationComplete => _battlegroundTargetColorTexture != null;

        public BattlegroundTargetTextures RenderBattleground()
        {
            BattlegroundCamera.enabled = true;
            BattlegroundCamera.Render();
            BattlegroundCamera.enabled = false;

            return new BattlegroundTargetTextures()
            {
                ColorTexture = _battlegroundTargetColorTexture,
                DepthTexture = _battlegroundTargetDepthTexture
            };
        }
    }

    public class BattlegroundTargetTextures
    {
        public RenderTexture ColorTexture;
        public RenderTexture DepthTexture;
    }
}
