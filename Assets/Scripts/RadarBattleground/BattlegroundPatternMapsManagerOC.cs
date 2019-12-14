using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.RadarBattleground
{
    public class BattlegroundPatternMapsManagerOC : MonoBehaviour
    {
        public ComputeShader RFloatTransportComputeShader;
        public float EdgeCarryFraction = 0.05f;
        public float EdgeThreshold = 0.0001f;
        public Vector2 BattlegroundWorldSpaceSize;
        public Vector2Int MasterHeightMapSize;
        private HeightmapRepresentationRelocator _relocator;

        private BattlegroundOcclusionTexturesPack _battlegroundOcclusionTexturesPack;
        private RenderTexture _patternColorTexture;

        public void GeneratePatternMaps(BattlegroundTargetTextures battlegroundTargetTextures)
        {
            _battlegroundOcclusionTexturesPack = GenerateOcclusionTextures(battlegroundTargetTextures);
            _patternColorTexture = CopyOfColorTexture(battlegroundTargetTextures);
        }

        private RenderTexture CopyOfColorTexture(BattlegroundTargetTextures battlegroundTargetTextures)
        {
            var oldColorTexture = battlegroundTargetTextures.ColorTexture;
            var newColorTexture = new RenderTexture(oldColorTexture.width, oldColorTexture.height, 0, oldColorTexture.format);
            newColorTexture.Create();
            Graphics.Blit(oldColorTexture, newColorTexture);
            return newColorTexture;
        }

        private BattlegroundOcclusionTexturesPack GenerateOcclusionTextures(BattlegroundTargetTextures battlegroundTargetTextures)
        {
            var battlegroundTargetSize = new Vector2Int(battlegroundTargetTextures.DepthTexture.width, battlegroundTargetTextures.DepthTexture.height);

            if (_relocator == null)
            {
                _relocator = new HeightmapRepresentationRelocator(RFloatTransportComputeShader);
            }

            var edgesTexture = new Texture2D(MasterHeightMapSize.x, MasterHeightMapSize.y, TextureFormat.Alpha8, true);
            var heightMapArray = _relocator.TextureToArray(battlegroundTargetTextures.DepthTexture);
            var masterHeightMapArray = new HeightmapArray(new float[MasterHeightMapSize.x*MasterHeightMapSize.y], MasterHeightMapSize);

            var edgeCarryInPixels =  Mathf.RoundToInt(EdgeCarryFraction*MasterHeightMapSize.x);
            for (var y = 0; y < MasterHeightMapSize.y; y++)
            {
                float phi = (y / (float) MasterHeightMapSize.y) * 2 * Mathf.PI - Mathf.PI;
                float maxHeightInLine = float.MinValue;
                int currentEdgeCarry = 0;
                for (var x = 0; x < MasterHeightMapSize.x; x++)
                {
                    float r = (x / (float) MasterHeightMapSize.x);

                    var uv = MathUtils.PolarToCartesian(new Vector2(r, phi));
                    var uvIn01 = (uv + new Vector2(1, 1)) * 0.5f;
                    var coords = uvIn01.MemberwiseMultiply(battlegroundTargetSize);

                    var newHeight = heightMapArray.SampleWithFilter(coords);
                    bool isEdgeDetected = false;
                    if (newHeight > maxHeightInLine)
                    {
                        if (Mathf.Abs(newHeight - maxHeightInLine) > EdgeThreshold)
                        {
                            isEdgeDetected = true;
                            currentEdgeCarry = edgeCarryInPixels+1;
                        }
                        maxHeightInLine = newHeight;
                    }
                    if (currentEdgeCarry > 0)
                    {
                        isEdgeDetected = true;
                    }
                    currentEdgeCarry--;

                    masterHeightMapArray.SetPixel(new Vector2Int(x, y), maxHeightInLine);
                    if (isEdgeDetected )
                    {
                        edgesTexture.SetPixel(x, y, new Color(1, 1, 1, 1));
                    }
                    else
                    {
                        edgesTexture.SetPixel(x,y, new Color(0,0,0,0));
                    }
                }


            }
            edgesTexture.Apply(true);

                var flatSize = new Vector2(BattlegroundWorldSpaceSize.x, BattlegroundWorldSpaceSize.y);
                var rect = new Rect(transform.position.x - flatSize.x/2, transform.position.z-flatSize.y/2, flatSize.x, flatSize.y);

            return new BattlegroundOcclusionTexturesPack()
            {
                OcclusionHeightMap = _relocator.ArrayToTexture(masterHeightMapArray),
                OcclussionEdges = edgesTexture,
                OcclusionHeightmapArraySampler = new HeightmapArrayFromWorldSpaceSampler(rect, heightMapArray)
            };
        }

        public class BattlegroundOcclusionTexturesPack
        {
            public Texture OcclusionHeightMap;
            public Texture OcclussionEdges;
            public HeightmapArrayFromWorldSpaceSampler OcclusionHeightmapArraySampler;
        }

        public BattlegroundOcclusionTexturesPack BattlegroundOcclusionTextures => _battlegroundOcclusionTexturesPack;

        public RenderTexture PatternColorTexture => _patternColorTexture;

        public bool InitializationComplete => _battlegroundOcclusionTexturesPack != null;
    }

    public class HeightmapArrayFromWorldSpaceSampler
    {
        private Rect _worldSpaceMapArea;
        private HeightmapArray _heightmapArray;

        public HeightmapArrayFromWorldSpaceSampler(Rect worldSpaceMapArea, HeightmapArray heightmapArray)
        {
            _worldSpaceMapArea = worldSpaceMapArea;
            _heightmapArray = heightmapArray;
        }

        public float Sample(Vector2 worldSpacePosition)
        {
            var uv = _worldSpaceMapArea.UvInRect(worldSpacePosition);
            return _heightmapArray.SampleWithFilterByUv(uv);
        }
    }
}
