Shader "Custom/RadarTextureUpdater"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "blue" {}
        _BeamIntensityTexture("BeamIntensityTexture", 2D) = "blue" {}
		_BeamAngleInDegrees("BeamAngleInDegrees", Range(0,360)) = 0
		_BeamAngleInDegreesDelta("BeamAngleInDegreesDelta", Range(0,360)) = 0
		_RadialNoiseMultiplier("RadialNoiseMultiplier", Vector) = (1.0,1.0,1.0,0.0)
		_CartesianNoiseMultiplier("CartesianNoiseMultiplier", Vector) = (1.0, 1.0, 1.0, 0.0)
		_BeamIndicatorSize("BeamIndicatorSize",Range(0,1)) = 0.02

		_AnnealingSpeedMultiplier("AnnealingSpeedMultiplier", Range(0,1)) = 0.9
		_AnnealingSpeedOffset("AnnealingSpeedMultiplier", Range(-1,1)) = -0.01

		_OcclusionHeightMap("OcclusionHeightMap", 2D) = "blue" {}
		_OcclusionEdges("_OcclusionEdges", 2D) = "blue" {}
		_BattlegroundPatternTexture("_BattlegroundPatternTexture", 2D) = "blue" {}

		_OcclusionEdgesMipmapLevel("_OcclusionEdgesMipmapLevel", Int) = 0

		_BattlegroundColorTexture("BattlegroundColorTexture", 2D) = "blue"{}
		_BattlegroundDepthTexture("BattlegroundDepthTexture", 2D) = "blue"{}
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

            sampler2D _MainTex;
            sampler2D _BeamIntensityTexture;
			float _BeamAngleInDegrees;
			float _BeamAngleInDegreesDelta;
			float _BeamIndicatorSize;
			float4 _RadialNoiseMultiplier;
			float4 _CartesianNoiseMultiplier;

			float _AnnealingSpeedMultiplier;
			float _AnnealingSpeedOffset;

			sampler2D _OcclusionHeightMap;
			sampler2D _OcclusionEdges;
			sampler2D _BattlegroundPatternTexture;

			int _OcclusionEdgesMipmapLevel;

			sampler2D _BattlegroundColorTexture;
			sampler2D _BattlegroundDepthTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

			float2 toCartesian(float2 rAndPhi) {
				return float2(rAndPhi.x*cos(rAndPhi.y), rAndPhi.x*sin(rAndPhi.y));
			}

			float4 applyAnnealing(float4 i, float annealingIntensity) {
				return saturate(i* lerp(1,_AnnealingSpeedMultiplier,annealingIntensity) + _AnnealingSpeedOffset*annealingIntensity);
			}

			static const float PI = 3.141592653589793238462;

            fixed4 frag (v2f i) : SV_Target
            {
				float2 centeredUv = (i.uv - 0.5) * 2;
				float r = length(centeredUv);
				float phi = atan2(centeredUv.y , centeredUv.x);

				float4 radarSample = tex2D(_MainTex, i.uv);
				
				float angleDifference = ( radians(_BeamAngleInDegrees)-phi);
				if (angleDifference < 0) {
					angleDifference += 2 * PI;
				}
				float frameAngleDelta = radians(_BeamAngleInDegreesDelta);

				float annealingIntensity = 1;
				float3 noiseColor = float3(0, 1, 0);
				float2 polarUv = float2(r, (phi/(2*PI))+0.5);
				if (angleDifference < frameAngleDelta) {
					annealingIntensity = angleDifference / frameAngleDelta;
					float radialNoiseIntensity = cnoise(float2(r*_RadialNoiseMultiplier.x, phi*_RadialNoiseMultiplier.y))*_RadialNoiseMultiplier.z;
					float2 cartesianCoords = float2(312.123, -521.31234) + toCartesian(float2(r, phi));
					float cartesianNoiseIntensity = cnoise(float2(cartesianCoords.x*_CartesianNoiseMultiplier.x, cartesianCoords.y*_CartesianNoiseMultiplier.y))* _CartesianNoiseMultiplier.z;
					radarSample = max(radarSample, max(cartesianNoiseIntensity, radialNoiseIntensity) * float4(noiseColor,1));

					float4 patternColor = tex2D(_BattlegroundPatternTexture, i.uv);
					float occlusionEdge = tex2Dlod(_OcclusionEdges, float4(polarUv, 0, _OcclusionEdgesMipmapLevel)).a;
					float occlusionHeight = tex2D(_OcclusionHeightMap, polarUv);
					if (occlusionEdge > 0) {
						radarSample= float4(patternColor.rgb,1)*occlusionEdge*(patternColor.a*3);
					}

					float battlegroundDepth = tex2D(_BattlegroundDepthTexture, i.uv).r;
					if (battlegroundDepth > occlusionHeight+0.0001) {
						float4 battlegroundSample = tex2D(_BattlegroundColorTexture, i.uv);
						radarSample += float4(battlegroundSample.rgb * battlegroundSample.a, battlegroundSample.a);
					}

				}
				radarSample= applyAnnealing(radarSample, annealingIntensity);

				float beamIndicatorIntensity = 0;
				//if (abs(angleDifference)*r < _BeamIndicatorSize){
				//	beamIndicatorIntensity = (_BeamIndicatorSize-(abs(angleDifference)*r))/_BeamIndicatorSize;
				//}

				return float4(radarSample);
            }
            ENDCG
        }
    }
}
