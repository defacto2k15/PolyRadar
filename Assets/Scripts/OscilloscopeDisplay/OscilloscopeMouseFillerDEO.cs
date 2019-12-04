using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OscilloscopeDisplay
{
    public class OscilloscopeMouseFillerDEO : MonoBehaviour
    {
        public OscilloscopeIntensityTextureContainerOC IntensityTextureContainerOc;
        public Material MouseFillingMaterial;
        [Range(0,1)]
        public float DotSize;

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
            MouseFillingMaterial.SetFloat("_DotSize", DotSize);
            IntensityTextureContainerOc.ApplyTransformingMaterial(MouseFillingMaterial);
        }
    }
}
