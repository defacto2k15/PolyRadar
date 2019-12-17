Shader "Custom/RocketMarker"
{
    Properties
    {
		_AlphaTexture("AlphaTexture",2D) = "blue" {}
		 [PerRendererData]_Color("_Color", Vector) = (0.0,1.0,0.0,1.0)
		 _ColorMultiplier("_ColorMultiplier",Range(0,100)) = 1.0
		_AlphaPowerFactor("AlphaPowerFactor",Range(0,20)) = 1.0
		_BlinkingIntensityRange("BlinkingIntensityRange", Vector) = (0.5, 1.5, 0.0, 0.0)
		_BlinkingFrequency("BlinkingFrequency", Range(0,10)) = 1.0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
				float worldSpaceY : ANY_Y;
            };

			float4 _Color;
			float _ColorMultiplier;
			sampler2D _AlphaTexture;
			float _AlphaPowerFactor;
			float4 _BlinkingIntensityRange;
			float _BlinkingFrequency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldSpaceY = mul( unity_ObjectToWorld, v.vertex).y;
				o.uv = v.uv;
                return o;
            }

			float invLerp(float i, float2 range) {
				return (i - range.x) / (range.y - range.x);
			}

			struct RenderTargets {
				float4 Color : SV_TARGET0;
				float Depth : SV_TARGET1;
			};

            RenderTargets frag (v2f i)
            {
				float  alpha = tex2D(_AlphaTexture, i.uv).a;
				alpha = pow(alpha, _AlphaPowerFactor);
				float3 baseColor = _Color.xyz *alpha*_ColorMultiplier;

				float blinkingIntensity = lerp(_BlinkingIntensityRange.x, _BlinkingIntensityRange.y, (sin(_Time[2] * _BlinkingFrequency) + 1)*0.5);

				float4 finalColor = float4(baseColor.xyz *= blinkingIntensity, alpha);

				RenderTargets target;
				target.Color =  finalColor;
				target.Depth =  i.worldSpaceY;
				return target;
            }
            ENDCG
        }
    }
}
