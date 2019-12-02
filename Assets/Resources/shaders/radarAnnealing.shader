Shader "Custom/RadarAnnealing"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "blue" {}
		_AnnealingSpeedMultiplier("AnnealingSpeedMultiplier", Range(0,1)) = 0.9
		_AnnealingSpeedOffset("AnnealingSpeedMultiplier", Range(-1,1)) = -0.01
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
			float _AnnealingSpeedMultiplier;
			float _AnnealingSpeedOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float originalIntensity = tex2D(_MainTex, i.uv).r;
				return saturate(originalIntensity * _AnnealingSpeedMultiplier + _AnnealingSpeedOffset);
            }
            ENDCG
        }
    }
}
