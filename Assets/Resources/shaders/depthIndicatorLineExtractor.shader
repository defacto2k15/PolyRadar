Shader "Custom/DistanceIndicatorLineExtractor"
{
    Properties
    {
		 _DepthTexture("DepthTexture", 2D) = "blue"{}
		 _BloomMultiplier("_BloomMultiplier",Range(0,10)) = 1.0
		 _NoiseIntensity("_NoiseIntensity", Range(0,10)) = 1.0
		 _NoiseFrequency("_NoiseFrequency", Range(0,10)) = 1.0
		 _DebugScalar("DebugScalar", Range(0, 2)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "ClassicNoise2D.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

			sampler2D _DepthTexture;
			float _BloomMultiplier;
			float _NoiseIntensity;
			float _NoiseFrequency;
			float _DebugScalar;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

			float2 coordsToUv(int2 coords) {
				return (coords / 1024.0) + 1 / 2048;
			}

			float invLerp(float i, float2 range) {
				return (i - range.x) / (range.y - range.x);
			}

            float4 frag (v2f i) : SV_TARGET
            {
				float2 uv = i.uv;
				uv.x = invLerp(uv.x, float2(0.2, 0.8));
				int2 coords = round(uv * 1023);

				int samplesCount = 5;

				int lodLevel = 3;
				int lodSquareLength = pow(2, lodLevel);
				int perSampleJumpLength = 4;

				int samplesXPositions[5];

				int centerSampleXPosition = (1024 / 2) - (lodSquareLength / 2) - 1;
				samplesXPositions[0] = centerSampleXPosition - lodSquareLength*perSampleJumpLength*2;
				samplesXPositions[1] = centerSampleXPosition - lodSquareLength*perSampleJumpLength*1;
				samplesXPositions[2] = centerSampleXPosition;
				samplesXPositions[3] = centerSampleXPosition + lodSquareLength*perSampleJumpLength*1;
				samplesXPositions[4] = centerSampleXPosition + lodSquareLength*perSampleJumpLength*2;

				float samplesWeights[5];
				samplesWeights[0] = 0.25;
				samplesWeights[1] = 0.5;
				samplesWeights[2] = 1;
				samplesWeights[3] = 0.5;
				samplesWeights[4] = 0.25;

				float samplesWeightsSum = 0;

				float sum = 0;
				for (int i = 0; i < samplesCount; i++) {

					float2 centeredUv = coordsToUv(int2(samplesXPositions[i], coords.y));

					//centeredUv.x = saturate(centeredUv.x*(8 / 1024.0));
					float sampleWeight = samplesWeights[i];
					samplesWeightsSum += sampleWeight;
					sum += sampleWeight*tex2Dlod(_DepthTexture, float4(centeredUv, 0, lodLevel)).r;
				}

				float depth = tex2Dlod(_DepthTexture, float4(uv, 0, 0)).r;
				//return  depth/5;// step(depth, _DebugScalar);
				float maxDistance = 5;
				float depthIntensity = sum /  maxDistance / samplesWeightsSum;

				depthIntensity += cnoise(float2(uv.y*_NoiseFrequency, _Time[3] * _NoiseFrequency)) * _NoiseIntensity;

				float lineWidth = 30;
				float distanceToLineCenter = abs(uv.x - depthIntensity);
				float dDistance = max(abs(ddx(distanceToLineCenter)),  abs(ddy(distanceToLineCenter)));
				return (distanceToLineCenter / (lineWidth*dDistance));
            }
            ENDCG
        }
    }
}
