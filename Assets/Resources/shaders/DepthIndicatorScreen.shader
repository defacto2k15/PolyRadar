Shader "Custom/depthIndicatorScreen"
{
    Properties
    {
		_MainTex("MainTex",2D) = "white"{}
        _LineColor("LineColor", Color) = (1,1,1,1)
		_BackgroundTexture("BackgroundTexture", 2D) = "black"{}
        _DepthLineTexture("DepthLineTexture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_ColorMultiplier("ColorMultiplier",Range(0,5)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

		sampler2D _MainTex;
		float4 _LineColor;
		sampler2D _DepthLineTexture;
		sampler2D _BackgroundTexture;
        half _Glossiness;
        half _Metallic;
		float _ColorMultiplier;
		
        struct Input
        {
            float2 uv_MainTex;
        };

			float invLerp(float i, float2 range) {
				return (i - range.x) / (range.y - range.x);
			}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 uv = IN.uv_MainTex;
			float lineIntensity = 1-tex2D(_DepthLineTexture, float2(1-uv.x, uv.y)).r;


			float2 backgroundUv = float2(invLerp(uv.x, float2(0, 2))+0.4, uv.y);
			float3 backgroundColor = tex2D(_BackgroundTexture, backgroundUv).rgb;
			o.Albedo = max(lineIntensity*_LineColor.rgb * _ColorMultiplier,  backgroundColor);
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
