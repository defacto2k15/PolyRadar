Shader "Custom/BrightEdgeProp"
{
    Properties
    {
		 [PerRendererData]_Color("_Color", Vector) = (0.0,1.0,0.0,1.0)
		 _MarginSize("MarginSize", Range(0,15)) = 0.05
		 _MarginTransitionWidth("MarginTransitionWidth", Range(0,1)) = 0.1
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
			float _MarginTransitionWidth;

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

			float deriv(float i) {
				return max(abs(ddx(i)), abs(ddy(i)));
			}

			float invLerp(float i, float2 range) {
				return (i - range.x) / (range.y - range.x);

			}

            RenderTargets frag (v2f i)
            {
				float2 uv1 = i.uv;
				float2 uv2 = 1-i.uv;
				float distanceToMargin = min(
					min(uv1.x/deriv(uv1.x), uv1.y/deriv(uv2.y)),
					min(uv2.x/deriv(uv2.x), uv2.y/deriv(uv2.y))
				);

				float2 transitionRange =  float2(1 - _MarginTransitionWidth / 2, 1 + _MarginTransitionWidth / 2);

				float4 finalColor = 0;
				finalColor =lerp(_Color,0, invLerp(distanceToMargin/_MarginSize, transitionRange));
				//if (distanceToMargin < _MarginSize) {
				//	finalColor = _Color;
				//}

				RenderTargets target;
				target.Color =  finalColor;
				target.Depth =  i.worldSpaceY;
				return target;
            }
            ENDCG
        }
    }
}
