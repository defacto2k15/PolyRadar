Shader "Custom/RadarDisplay"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "blue" {}
		_BackgroundTexture("BackgroundTexture", 2D) = "blue"{}
		_BackgroundTextureIntensityMultiplier("BackgroundTextureIntensityMultiplier",Range(0,2)) = 1
		_IntensityAdding("IntensityAdding", Range(0,10)) = 0
		_RadarColor("RadarColor", Vector) = (0.0,1.0,0.0,0.0)
		_LerpToWhiteFactor("LerpToWhiteFactor", Range(0,3)) = 1.0
		_IndicatorIntensityMultiplier("IndicatorIntensityMultiplier", Range(0,10)) = 0.1
		_IndicatorColor("IndicatorColor",Vector)=(0.0,1.0,0.0,1.0)
		_RadarIntensityMultiplier("RadarIntensityMultiplier", Range(0,5)) = 1
		_BattlegroundMarkersTexture("BattlegroundMarkersTexture", 2D) = "blue"{}
		_MarkersIntensityMultiplier("MarkersIntensityMultiplier", Range(0,5)) = 1
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
			sampler2D _BackgroundTexture;
			float _BackgroundTextureIntensityMultiplier;
			float _IntensityAdding;
			float4 _RadarColor;
			float _LerpToWhiteFactor;
			float _IndicatorIntensityMultiplier;
			float3 _IndicatorColor;
			float _RadarIntensityMultiplier;
			sampler2D _BattlegroundMarkersTexture;
			float _MarkersIntensityMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

			float invLerpSaturated(float val, float min, float max) {
				float delta = max - min;
				return saturate((val - min) / delta);
			}

            float4 frag (v2f i) : SV_Target
            {
				float4 sampledBackgroundColor = tex2D(_BackgroundTexture, i.uv);
				float4 radarSample = tex2D(_MainTex, i.uv);
				float3 radarColor = radarSample.rgb;
				float indicatorIntensity = radarSample.a;

				float3 markersColor = tex2D(_BattlegroundMarkersTexture, i.uv).rgb;

				float3 backgroundInfluence = sampledBackgroundColor.rgb*sampledBackgroundColor.a* _BackgroundTextureIntensityMultiplier;
				float3 indicatorInfluence = _IndicatorColor * indicatorIntensity*_IndicatorIntensityMultiplier;
				float3 radarInfluence = radarColor * _RadarIntensityMultiplier;
				float3 markersInfluence = markersColor * _MarkersIntensityMultiplier;

				float3 finalColor = indicatorInfluence + radarInfluence + backgroundInfluence + markersInfluence;
				float maxComponent = max(max(finalColor.r, finalColor.g), finalColor.b);

				finalColor = lerp(finalColor, 1, maxComponent*_LerpToWhiteFactor);
				finalColor *= (1 + _IntensityAdding);

				float distanceToCenter = length(i.uv - 0.5) * 2;
				finalColor = lerp(0, finalColor, invLerpSaturated(distanceToCenter, 1, 0.95));
				return float4(finalColor,1);
            }
            ENDCG
        }
    }
}
