Shader "Custom/RadarDepth"
{
    Properties
    {
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float4 worldSpace : ANY_WORLDSPACE;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldSpace = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

			float4 _GlobalRadarPosition;

            float4 frag (v2f i) : SV_TARGET
            {
				return  length(_GlobalRadarPosition.xz - i.worldSpace.xz);
            }
            ENDCG
        }
    }
}
