Shader "Custom/RadarWithMouseFilling"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "blue" {}
		_MouseUvPosition("MouseUvPosition", Vector) = (0.0, 0.0, 0.0, 0.0)
		_DotSize("DotSize", Range(0,1)) = 0.1
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
			float4 _MouseUvPosition;
			float _DotSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 originalColors = tex2D(_MainTex, i.uv).rg;
				float intensity = originalColors.r;

				float distanceToMouse = length(i.uv - _MouseUvPosition.xy);
				float byDistanceIntensity = saturate(1 - distanceToMouse / _DotSize);

				return float4( max(intensity, byDistanceIntensity), originalColors.g,0,1);
            }
            ENDCG
        }
    }
}
