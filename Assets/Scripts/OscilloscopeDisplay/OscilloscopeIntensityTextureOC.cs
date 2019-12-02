using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OscilloscopeDisplay
{
    public class OscilloscopeIntensityTextureOC : MonoBehaviour
    {
        public Vector2Int Size;
        private RenderTexture _intensityTexture;
        private RenderTexture _intensityTextureWorkCopy;

        public void Start()
        {
            _intensityTexture =  new RenderTexture(Size.x, Size.y, 0, RenderTextureFormat.ARGB32);
            _intensityTexture.Create();
            _intensityTextureWorkCopy = new RenderTexture(Size.x, Size.y, 0, RenderTextureFormat.ARGB32);
            _intensityTextureWorkCopy.Create();
        }

        public RenderTexture IntensityTexture => _intensityTexture;

        public RenderTexture RetrieveIntensityTextureCopy()
        {
            Graphics.Blit(_intensityTexture, _intensityTextureWorkCopy);
            return _intensityTextureWorkCopy;
        }
    }
}
