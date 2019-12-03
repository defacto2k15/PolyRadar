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

        [Range(0, 10)]
        public float Threshold = 1;

        [Range(0, 1)] public float SoftTreshold = 0.5f;
        [Range(0, 10)]
	    public float BloomIntensity = 1;

        public bool Debug;

        private const int BoxDownPrefilterPass = 0;
        private const int BoxDownPass= 1;
        private const int BoxUpPass = 2;
        private const int ApplyBloomPass = 3;
        private const int DebugBloomPass = 4;

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var texSize = new Vector2Int(source.width, source.height);
            BloomMaterial.SetFloat("_Threshold", Threshold);
            BloomMaterial.SetFloat("_SoftThreshold", SoftTreshold);
            BloomMaterial.SetFloat("_Intensity", BloomIntensity);

            var tempTexArray = new RenderTexture[BlurringIterationsCount];
            tempTexArray[0] = RenderTexture.GetTemporary(texSize.x, texSize.y, 0, source.format);
            var currentDestination = tempTexArray[0];
            Graphics.Blit(source, currentDestination, BloomMaterial, BoxDownPrefilterPass);
            var currentSource = currentDestination;

            var i = 1;
            for (; i < BlurringIterationsCount; i++)
            {
                texSize = new Vector2Int(texSize.x/2, texSize.y/2);
                if (texSize.x < 2 || texSize.y < 2)
                {
                    break;
                }
                tempTexArray[i] =  RenderTexture.GetTemporary(texSize.x, texSize.y, 0, source.format);
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
                Graphics.Blit(currentSource, destination, BloomMaterial, DebugBloomPass);
            }
            else
            {
                BloomMaterial.SetTexture("_SourceTex", source);
                Graphics.Blit(currentSource, destination, BloomMaterial, ApplyBloomPass);
            }

            foreach (var tempTex in tempTexArray.Where(c=>c != null))
            {
                RenderTexture.ReleaseTemporary(tempTex);
            }
        }
    }
}
