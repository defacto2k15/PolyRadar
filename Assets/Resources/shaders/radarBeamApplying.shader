Shader "Custom/RadarBeamApplying"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "blue" {}
		_UpdateAngleDistanceInDegrees("_UpdateAngleDistanceInDegrees", Range(0,360)) = 30
        _BeamIntensityTexture("BeamIntensityTexture", 2D) = "blue" {}
		_BeamAngleInDegrees("BeamAngleInDegrees", Range(0,360)) = 0
		_BeamAngleInDegreesDelta("BeamAngleInDegreesDelta", Range(0,360)) = 0
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
            sampler2D _BeamIntensityTexture;
			float _BeamAngleInDegrees;
			float _BeamAngleInDegreesDelta;
			float _UpdateAngleDistanceInDegrees;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 centeredUv = (i.uv - 0.5) * 2;
				float r = length(centeredUv);
				float phi = atan2(centeredUv.y , centeredUv.x);

				float intensity = tex2D(_MainTex, i.uv).r;
				
				float angleDifference = (phi - radians(_BeamAngleInDegrees));
				float prevFrameDelta = _BeamAngleInDegreesDelta;
				//if (sign(angleDelta) != sign(_BeamAngleInDegreesDelta)) {
				if (sign(prevFrameDelta) != 0) {
					if (sign(prevFrameDelta) != sign(angleDifference)) {
						float angleLength = abs(angleDifference);
						intensity = max(intensity, (1 - angleLength / radians(_UpdateAngleDistanceInDegrees)));
					}
				}

				return intensity;
            }
            ENDCG
        }
    }
}
