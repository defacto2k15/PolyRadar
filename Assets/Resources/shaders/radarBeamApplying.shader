Shader "Custom/RadarBeamApplying"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "blue" {}
		_UpdateAngleDistanceInDegrees("_UpdateAngleDistanceInDegrees", Range(0,360)) = 30
        _BeamIntensityTexture("BeamIntensityTexture", 2D) = "blue" {}
		_BeamAngleInDegrees("BeamAngleInDegrees", Range(0,360)) = 0
		_BeamAngleInDegreesDelta("BeamAngleInDegreesDelta", Range(0,360)) = 0
		_BeamIndicatorSizeInDegrees("BeamIndicatorSizeInDegrees", Range(0,360)) = 6
		_RadialNoiseMultiplier("RadialNoiseMultiplier", Vector) = (1.0,1.0,1.0,0.0)
		_CartesianNoiseMultiplier("CartesianNoiseMultiplier", Vector) = (1.0, 1.0, 1.0, 0.0)
		_BeamIndicatorSize("BeamIndicatorSize",Range(0,1)) = 0.02
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
			#include "ClassicNoise2D.hlsl"

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
			float _BeamIndicatorSize;
			float4 _RadialNoiseMultiplier;
			float4 _CartesianNoiseMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

			float sampleBeamIntensityTexture(float r, float phi) {
				return tex2D(_BeamIntensityTexture, float2(0, r)).r;
			}

			float2 toCartesian(float2 rAndPhi) {
				return float2(rAndPhi.x*cos(rAndPhi.y), rAndPhi.x*sin(rAndPhi.y));
			}

            fixed4 frag (v2f i) : SV_Target
            {
				float2 centeredUv = (i.uv - 0.5) * 2;
				float r = length(centeredUv);
				float phi = atan2(centeredUv.y , centeredUv.x);

				float2 originalColor = tex2D(_MainTex, i.uv).rg;
				float intensity = originalColor.r;
				
				float angleDifference = (phi - radians(_BeamAngleInDegrees));
				float prevFrameDelta = _BeamAngleInDegreesDelta;
				if (sign(prevFrameDelta) != 0) {
					if (sign(prevFrameDelta) != sign(angleDifference)) {

						float angleLength = abs(angleDifference);
						float oldAreaMultiplier = saturate(1 - angleLength / radians(_UpdateAngleDistanceInDegrees));

						float radialNoiseIntensity = cnoise(float2(r*_RadialNoiseMultiplier.x, phi*_RadialNoiseMultiplier.y))*_RadialNoiseMultiplier.z;
						float2 cartesianCoords = float2(312.123, -521.31234) + toCartesian(float2(r, phi));
						float cartesianNoiseIntensity = cnoise(float2(cartesianCoords.x*_CartesianNoiseMultiplier.x, cartesianCoords.y*_CartesianNoiseMultiplier.y))* _CartesianNoiseMultiplier.z;
						float beamIntensity = sampleBeamIntensityTexture(r, phi);

						intensity = max(intensity, max(beamIntensity,max(cartesianNoiseIntensity, radialNoiseIntensity))*oldAreaMultiplier);
					}
				}

				float beamIndicatorIntensity = 0;
				if (abs(angleDifference)*r < _BeamIndicatorSize){
					beamIndicatorIntensity = 1;
				}

				return float4(intensity, beamIndicatorIntensity,0,1);
            }
            ENDCG
        }
    }
}
