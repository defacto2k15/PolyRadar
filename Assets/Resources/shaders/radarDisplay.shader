Shader "Custom/RadarDisplay"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "blue" {}
		_IntensityAdding("IntensityAdding", Range(0,10)) = 0
		_RadarColor("RadarColor", Vector) = (0.0,1.0,0.0,0.0)
		_LerpToWhiteFactor("LerpToWhiteFactor", Range(0,3)) = 1.0
		_IndicatorIntensity("IndicatorIntensity", Range(0,1)) = 0.1

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
            float4 _MainTex_ST;
			float _IntensityAdding;
			float4 _RadarColor;
			float _LerpToWhiteFactor;
			float _IndicatorIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			float invLerpSaturated(float val, float min, float max) {
				float delta = max - min;
				return saturate((val - min) / delta);
			}


            float4 frag (v2f i) : SV_Target
            {
				float4 sampledColor = tex2D(_MainTex, i.uv);
				return sampledColor;

				float intensity = max(sampledColor.x, sampledColor.a*_IndicatorIntensity);
				//return intensity;
				float3 baseColor = lerp(_RadarColor.rgb, 1, intensity*_LerpToWhiteFactor);
				
				float4 col= float4(0,0,0,1);
				col.rgb = baseColor*intensity;
				col.rgb *= (1 + _IntensityAdding);

				float distanceToCenter = length(i.uv - 0.5) * 2;
				col = lerp(0, col, invLerpSaturated(distanceToCenter, 1, 0.95));
                return col;
            }
            ENDCG
        }
    }
}
