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
        private Texture2D _intensityTexture;
        private RenderTexture _intensityTextureWorkCopy;

        public void Start()
        {
            _intensityTexture = new Texture2D(Size.x, Size.y, TextureFormat.ARGB32, true);
            _intensityTextureWorkCopy = new RenderTexture(Size.x, Size.y, 0, RenderTextureFormat.ARGB32);
            _intensityTextureWorkCopy.Create();
        }

        public Texture2D IntensityTexture => _intensityTexture;

        public Texture RetriveIntensityTextureCopy()
        {
            Graphics.Blit(_intensityTexture, _intensityTextureWorkCopy);
            return _intensityTextureWorkCopy;
        }
    }
}
