Shader "Custom/BloomPostProcessing" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Threshold("Threshold", Range(0,100)) = 0.4
		_SoftThreshold("SoftThreshold", Range(0,1)) = 0.1
		_BloomIntensity("BloomIntensity", Range(0,10)) = 1
			_Debug("Debug",Range(0,5)) = 1
	}

	CGINCLUDE
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4 _MainTex_TexelSize;
		sampler2D _SourceTex;
		half _Threshold;
		half _SoftThreshold;
		half _BloomIntensity;
		float _Debug;

		struct VertexData {
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct Interpolators {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		Interpolators VertexProgram (VertexData v) {
			Interpolators i;
			i.pos = UnityObjectToClipPos(v.vertex);
			i.uv = v.uv;
			return i;
		}

		half4 Prefilter (half4 c) {
			float k = 0;
			if (c.a > _Threshold) {
				k = 1;
			}
			return float4(c.rgb, c.a*k);
			//half brightness = c.a;
			//half knee = _Threshold * _SoftThreshold;
			//half soft = brightness - _Threshold + knee;
			//soft = clamp(soft, 0, 2 * knee);
			//soft = soft * soft / (4 * knee + 0.00001);
			//half contribution = max(soft, brightness - _Threshold);
			//contribution /= max(brightness, 0.00001);
			//return float4(c.rgb,  contribution);
		}

		float4 Sample (float2 uv, float bloomIntensityOffset) {
			float4 s = tex2D(_MainTex, uv);
			s.a = max(0, s.a + bloomIntensityOffset);
			return s;
		}

		float4 SampleBox (float2 uv, float delta, float bloomIntensityOffset) {
			float4 o = _MainTex_TexelSize.xyxy * float2(-delta, delta).xxyy;

			float4 samples[4];
			samples[0] = Sample(uv + o.xy, bloomIntensityOffset);
			samples[1] = Sample(uv + o.zy, bloomIntensityOffset);
			samples[2] = Sample(uv + o.xw, bloomIntensityOffset);
			samples[3] = Sample(uv + o.zw, bloomIntensityOffset);
			float bloomIntensitySum = samples[0].a + samples[1].a + samples[2].a + samples[3].a;
			float3 outColor = (samples[0].rgb*samples[0].a + samples[1].rgb*samples[1].a + samples[2].rgb*samples[2].a + samples[3].rgb*samples[3].a) / 4;// (bloomIntensitySum);
			return float4(pow(outColor,_Debug), pow(bloomIntensitySum,0.25) );
		}
	ENDCG

	SubShader {
		Cull Off
		ZTest Always
		ZWrite Off

		Pass { // Box down prefilter
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram

				half4 FragmentProgram (Interpolators i) : SV_Target {
					return Prefilter(SampleBox(i.uv, 1, -5));
				}
			ENDCG
		}
		Pass { // BoxDown
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram

				half4 FragmentProgram (Interpolators i) : SV_Target {
					return SampleBox(i.uv, 1, 0);
				}
			ENDCG
		}
		Pass { // BoxUp
			Blend One One
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram

				half4 FragmentProgram (Interpolators i) : SV_Target {
					return SampleBox(i.uv, 0.5, 0);
				}
			ENDCG
		}
		Pass { // Apply bloom
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram

				half4 FragmentProgram (Interpolators i) : SV_Target {
					half4 c = tex2D(_SourceTex, i.uv);
					c.rgb += _BloomIntensity*SampleBox(i.uv, 0.5, 0);
					return c;
				}
			ENDCG
		}
		Pass { // Debug
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram

				half4 FragmentProgram (Interpolators i) : SV_Target {
					return Prefilter(SampleBox(i.uv, 1, -5)).a;
					//return half4(_BloomIntensity*SampleBox(i.uv, 0.5), 1);
					//return tex2D(_SourceTex,i.uv).a-10;
				}
			ENDCG
		}
	}
}