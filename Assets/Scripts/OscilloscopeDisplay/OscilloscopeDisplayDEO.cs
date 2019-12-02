using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OscilloscopeDisplay
{
    public class OscilloscopeDisplayDEO : MonoBehaviour
    {
        public OscilloscopeIntensityTextureContainerOC IntensityTextureContainerOc;
        public Material MouseFillingMaterial;

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mouseUv = new Vector2(Input.mousePosition.x / ((float) Screen.width), Input.mousePosition.y / ((float) Screen.height));
                FillRadarIntensityWithMousePositionDot(mouseUv);
            }
        }

        private void FillRadarIntensityWithMousePositionDot(Vector2 mouseUv)
        {
            MouseFillingMaterial.SetVector("_MouseUvPosition", mouseUv);
            IntensityTextureContainerOc.ApplyTransformingMaterial(MouseFillingMaterial);
        }
    }
}
