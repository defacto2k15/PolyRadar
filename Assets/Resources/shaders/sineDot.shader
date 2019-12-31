Shader "Custom/SineDot"
{
    Properties
    {
		 [PerRendererData]_Color("_Color", Vector) = (0.0,1.0,0.0,1.0)
		 _BloomMultiplier("_BloomMultiplier",Range(0,10)) = 1.0
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
            };

			float4 _Color;
			float _MarginSize;
			float _BloomMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_TARGET
            {
				float bloomIntensity = 1-saturate(length(i.uv - 0.5) * 2);
				float3 baseColor = _Color.xyz * (1-step(bloomIntensity,0));

				float4 finalColor = float4(baseColor*bloomIntensity*_BloomMultiplier, bloomIntensity);

				return finalColor;  
            }
            ENDCG
        }
    }
}
