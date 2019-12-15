Shader "Custom/BattlegroundMonocolor"
{
    Properties
    {
		 [PerRendererData]_Color("_Color", Vector) = (0.0,1.0,0.0,1.0)
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
				float worldSpaceY : ANY_Y;
            };

			float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldSpaceY = mul( unity_ObjectToWorld, v.vertex).y;
                return o;
            }

			struct RenderTargets {
				float4 Color : SV_TARGET0;
				float Depth : SV_TARGET1;
			};

            RenderTargets frag (v2f i)
            {
				RenderTargets target;
				target.Color =  _Color;
				target.Depth =  i.worldSpaceY;
				return target;
            }
            ENDCG
        }
    }
}
