using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OscilloscopeDisplay
{
    public class BloomPostProcessingEffect : MonoBehaviour
    {
        public Material BloomMaterial;
        [Range(1, 16)] public int BlurringIterationsCount = 1;
        public bool BloomIsEnabled=true;
        public bool Debug;

        private const int BoxDownPrefilterPass = 0;
        private const int BoxDownPass= 1;
        private const int BoxUpPass = 2;
        private const int ApplyBloomPass = 3;
        private const int DebugBloomPass = 4;

        private RenderTexture _bloomRenderTarget;
        private RenderTexture _colorRenderTarget;
        private Vector2Int _previousScreenSize= Vector2Int.zero;

        public void Start()
        {
            _previousScreenSize = new Vector2Int(Screen.width, Screen.height);
            CreateAndSetRenderTargets();
        }

        public void OnPreRender()
        {
            var currentScreenSize = new Vector2Int(Screen.width, Screen.height);

            if (!currentScreenSize.Equals(_previousScreenSize))
            {
                RenderTexture.ReleaseTemporary(_colorRenderTarget);
                RenderTexture.ReleaseTemporary(_bloomRenderTarget);
                _previousScreenSize = currentScreenSize;
                CreateAndSetRenderTargets();
            }

        }

        private void CreateAndSetRenderTargets()
        {
            _colorRenderTarget= RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.ARGBFloat);
            _bloomRenderTarget = RenderTexture.GetTemporary(Screen.width, Screen.height, 0, RenderTextureFormat.RFloat);
            var cam = GetComponent<Camera>();
            cam.SetTargetBuffers(new[] {_colorRenderTarget.colorBuffer, _bloomRenderTarget.colorBuffer}, _colorRenderTarget.depthBuffer);
        }

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var texSize = new Vector2Int(_colorRenderTarget.width, _colorRenderTarget.height);

            var tempTexArray = new RenderTexture[BlurringIterationsCount];
            tempTexArray[0] = RenderTexture.GetTemporary(texSize.x, texSize.y, 0, _colorRenderTarget.format);
            var currentDestination = tempTexArray[0];
            Graphics.Blit(_colorRenderTarget, currentDestination, BloomMaterial, BoxDownPrefilterPass);
            //Graphics.Blit(currentDestination, destination);
            var currentSource = currentDestination;

            var i = 1;
            for (; i < BlurringIterationsCount; i++)
            {
                texSize = new Vector2Int(texSize.x / 2, texSize.y / 2);
                if (texSize.x < 2 || texSize.y < 2)
                {
                    break;
                }
                tempTexArray[i] = RenderTexture.GetTemporary(texSize.x, texSize.y, 0, _colorRenderTarget.format);
                currentDestination = tempTexArray[i];
                Graphics.Blit(currentSource, currentDestination, BloomMaterial, BoxDownPass);
                currentSource = currentDestination;
            }

            for (i -= 2; i >= 0; i--)
            {
                currentDestination = tempTexArray[i];
                Graphics.Blit(currentSource, currentDestination, BloomMaterial, BoxUpPass);
                currentSource = currentDestination;
            }

            if (Debug)
            {
                Graphics.Blit(_colorRenderTarget, destination, BloomMaterial, DebugBloomPass);
            }
            else
            {
                if (BloomIsEnabled)
                {
                    BloomMaterial.SetTexture("_SourceTex", _colorRenderTarget);
                    Graphics.Blit(currentSource, destination, BloomMaterial, ApplyBloomPass);
                }
                else
                {
                    Graphics.Blit(_colorRenderTarget, destination);
                }
            }

            foreach (var tempTex in tempTexArray.Where(c=>c != null))
            {
                RenderTexture.ReleaseTemporary(tempTex);
            }
        }
    }
}
