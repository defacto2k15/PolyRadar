Shader "Custom/brightYEdgeMonocolourSS"
{
    Properties
    {
        [PerRendererData] _Color ("Color", Color) = (1,1,1,1)
		 _BloomMultiplier("_BloomMultiplier",Range(0,10)) = 1.0
		_EdgePowFactor("EdgePowFactor",Range(0,4)) = 1.0	
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_MainTex("MainTex",2D) = "pink" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

		float _BloomMultiplier;
		float _EdgePowFactor;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 uv = IN.uv_MainTex;
			o.Alpha = 1;

				float3 baseColor = _Color.xyz;
				float4 finalColor = float4(baseColor*_BloomMultiplier, 1);
				float2 distanceToEdges = float2(min(uv.x, 1 - uv.x), min(uv.y, 1 - uv.y))*2;
				float edgeFactor = distanceToEdges.y;
				edgeFactor = pow(edgeFactor, _EdgePowFactor);

				o.Albedo = (1-	edgeFactor) * _Color;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
