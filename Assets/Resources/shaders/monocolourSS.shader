Shader "Custom/monocolourSS"
{
    Properties
    {
        [PerRendererData] _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
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

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			o.Albedo = _Color;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
