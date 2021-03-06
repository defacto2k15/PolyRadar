﻿Shader "Custom/BrightCenterProp"
{
    Properties
    {
		 [PerRendererData]_Color("_Color", Vector) = (0.0,1.0,0.0,1.0)
		 _BloomMultiplier("_BloomMultiplier",Range(0,10)) = 1.0
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
				float worldSpaceY : ANY_Y;
            };

			float4 _Color;
			float _MarginSize;
			float _BloomMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldSpaceY = mul( unity_ObjectToWorld, v.vertex).y;
				o.uv = v.uv;
                return o;
            }

			struct RenderTargets {
				float4 Color : SV_TARGET0;
				float Depth : SV_TARGET1;
			};

            RenderTargets frag (v2f i)
            {
				float bloomIntensity = 1-saturate(length(i.uv - 0.5) * 2);
				float3 baseColor = _Color.xyz * (1-step(bloomIntensity,0));

				float4 finalColor = float4(baseColor*bloomIntensity*_BloomMultiplier, 1);

				RenderTargets target;
				target.Color =  finalColor;
				target.Depth =  i.worldSpaceY;
				return target;
            }
            ENDCG
        }
    }
}
