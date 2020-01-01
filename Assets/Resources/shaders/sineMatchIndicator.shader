Shader "Custom/SineMatchIndicator"
{
	Properties
	{
		 _Color("_Color", Color) = (0.0,1.0,0.0,1.0)
		 _UvCutoff("_UvCutoff", Range(0,1)) = 1
		 _AlphaPowerFactor("_AlphaMultiplier",Range(0,10)) = 1.0
		_AlphaTexture("AlphaTexture",2D) = "blue" {}
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
			float _UvCutoff;
			float _AlphaPowerFactor;
			sampler2D _AlphaTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_TARGET
            {
				float  alpha = tex2D(_AlphaTexture, i.uv).a;
				alpha = pow(alpha, _AlphaPowerFactor);
				float3 baseColor = _Color.xyz *alpha;
				alpha *= step(i.uv.y, _UvCutoff);

				return float4(baseColor, alpha);  
            }
            ENDCG
        }
    }
}
