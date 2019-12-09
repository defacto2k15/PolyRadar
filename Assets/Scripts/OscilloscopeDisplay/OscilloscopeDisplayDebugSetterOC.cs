using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.RadarBattleground;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.OscilloscopeDisplay
{
    public class OscilloscopeDisplayDebugSetterOC : MonoBehaviour
    {
        public BattlegroundMasterScriptOC BattlegroundMasterScript;
        public OscilloscopeIntensityTextureContainerOC IntensityTextureContainerOc;

        public void Update()
        {
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", IntensityTextureContainerOc.ActiveTexture);
            var texPack = BattlegroundMasterScript.RenderBattleground();
            GetComponent<MeshRenderer>().material.SetTexture("_BattlegroundColorTex", texPack.ColorTexture);
            GetComponent<MeshRenderer>().material.SetTexture("_BattlegroundDepthTex", texPack.DepthTexture);
        }
    }
}
