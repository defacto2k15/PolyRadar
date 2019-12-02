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
        public OscilloscopeIntensityTextureContainerOC IntensityTextureContainerOc;

        public void Update()
        {
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", IntensityTextureContainerOc.ActiveTexture);
        }
    }
}
