﻿Shader "Custom/BrightYEdgeMonocolour"
{
    Properties
    {
		 [PerRendererData]_Color("_Color", Vector) = (0.0,1.0,0.0,1.0)
		 _BloomMultiplier("_BloomMultiplier",Range(0,10)) = 1.0
		_EdgePowFactor("EdgePowFactor",Range(0,4)) = 1.0	
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

			float4 _Color;
			float _BloomMultiplier;
			float _EdgePowFactor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_TARGET
            {
				float3 baseColor = _Color.xyz;
				float4 finalColor = float4(baseColor*_BloomMultiplier, 1);
				float2 distanceToEdges = float2(min(i.uv.x, 1 - i.uv.x), min(i.uv.y, 1 - i.uv.y))*2;
				float edgeFactor = distanceToEdges.y;
				edgeFactor = pow(edgeFactor, _EdgePowFactor);

				return (1-edgeFactor) * _Color;
            }
            ENDCG
        }
    }
}
