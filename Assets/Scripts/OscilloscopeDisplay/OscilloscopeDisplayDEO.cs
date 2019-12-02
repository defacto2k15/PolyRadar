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
        public OscilloscopeIntensityTextureOC IntensityTextureOc;

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var oscilloscopeIntensityTexture = IntensityTextureOc.IntensityTexture;
                var mouseUv = new Vector2(Input.mousePosition.x / ((float) Screen.width), Input.mousePosition.y / ((float) Screen.height));
                var texCoords = new Vector2Int(Mathf.RoundToInt(mouseUv.x * oscilloscopeIntensityTexture.width),
                    Mathf.RoundToInt(mouseUv.y * oscilloscopeIntensityTexture.height));
                oscilloscopeIntensityTexture.SetPixel(texCoords.x, texCoords.y, new Color(255,0,0,1));
                oscilloscopeIntensityTexture.Apply();
            }
        }
    }
}
