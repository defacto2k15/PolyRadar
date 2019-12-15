Shader "Custom/BrightEdgeMarker"
{
    Properties
    {
		 [PerRendererData]_Color("_Color", Vector) = (0.0,1.0,0.0,1.0)
		 _MarginSize("MarginSize", Range(0,15)) = 0.05
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

			float4 _Color;
			float _MarginSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

			float deriv(float i) {
				return max(abs(ddx(i)), abs(ddy(i)));
			}

            float4 frag (v2f i) : SV_TARGET
            {
				float2 uv = i.uv;
				float2 screenDependentMarginSize = float2(_MarginSize * deriv(uv.x), _MarginSize * deriv(uv.y));

				float2 distanceToMargin = float2(
					min( uv.x, 1-uv.x ) / screenDependentMarginSize.x,
					min( uv.y, 1-uv.y ) / screenDependentMarginSize.y);

				bool xNearMargin = uv.x < screenDependentMarginSize.x || uv.x > (1-screenDependentMarginSize.x);// || i.uv.x > maxThreshold.x;
				bool yNearMargin = uv.y < screenDependentMarginSize.y || uv.y > (1-screenDependentMarginSize.y);// || i.uv.x > maxThreshold.x;
				float bloomIntensity = 1- min(distanceToMargin.x, distanceToMargin.y);

				float3 baseColor = _Color.xyz * step(0,bloomIntensity);

				float4 finalColor = float4(baseColor, 1);

				return finalColor;
            }
            ENDCG
        }
    }
}
