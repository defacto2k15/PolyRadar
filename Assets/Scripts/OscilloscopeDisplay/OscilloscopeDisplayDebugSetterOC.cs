using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.OscilloscopeDisplay
{
    public class OscilloscopeDisplayDebugSetterOC : MonoBehaviour
    {
        public OscilloscopeIntensityTextureOC IntensityTextureOc;
        private bool _textureWasSet = false;

        public void Update()
        {
            if (!_textureWasSet)
            {
                _textureWasSet = true;
                Assert.IsNotNull(IntensityTextureOc.IntensityTexture);
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", IntensityTextureOc.IntensityTexture);
            }
        }
    }
}
