using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OscilloscopeDisplay
{
    public class RadarIntensityTextureContainerOC : MonoBehaviour
    {
        public Vector2Int Size;

        private int _activeTextureIndex;
        private List<RenderTexture> _intensityTextures;

        public void Start()
        {
            _intensityTextures = Enumerable.Range(0,2).Select(i =>  new RenderTexture(Size.x, Size.y, 0, RenderTextureFormat.ARGBFloat)).ToList();
            _intensityTextures.ForEach(c=>c.Create());
        }

        public RenderTexture ActiveTexture => _intensityTextures[_activeTextureIndex];
        private RenderTexture WorkTexture => _intensityTextures[(_activeTextureIndex + 1) % 2];

        public void ApplyTransformingMaterial(Material mat)
        {
            Graphics.Blit(ActiveTexture, WorkTexture, mat);
            _activeTextureIndex = (_activeTextureIndex + 1) % 2;
        }
    }
}
