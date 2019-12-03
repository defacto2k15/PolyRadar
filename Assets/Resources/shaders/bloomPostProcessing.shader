Shader "Custom/BloomPostProcessing" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Threshold("Threshold", Range(0,10)) = 0.4
		_SoftThreshold("SoftThreshold", Range(0,1)) = 0.1
		_BloomIntensity("BloomIntensity", Range(0,10)) = 1
	}

	CGINCLUDE
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4 _MainTex_TexelSize;
		sampler2D _SourceTex;
		half _Threshold;
		half _SoftThreshold;
		half _BloomIntensity;

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

		half3 Prefilter (half3 c) {
			half brightness = max(c.r, max(c.g, c.b));
			half knee = _Threshold * _SoftThreshold;
			half soft = brightness - _Threshold + knee;
			soft = clamp(soft, 0, 2 * knee);
			soft = soft * soft / (4 * knee + 0.00001);
			half contribution = max(soft, brightness - _Threshold);
			contribution /= max(brightness, 0.00001);
			return c * contribution;
		}

		half3 Sample (float2 uv) {
			return tex2D(_MainTex, uv).rgb;
		}

		half3 SampleBox (float2 uv, float delta) {
			float4 o = _MainTex_TexelSize.xyxy * float2(-delta, delta).xxyy;
			half3 s =
				Sample(uv + o.xy) + Sample(uv + o.zy) +
				Sample(uv + o.xw) + Sample(uv + o.zw);
			return s * 0.25f;
		}
	ENDCG

	SubShader {
		Cull Off
		ZTest Always
		ZWrite Off

		Pass {
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram

				half4 FragmentProgram (Interpolators i) : SV_Target {
					return half4(Prefilter(SampleBox(i.uv, 1)),1);
				}
			ENDCG
		}
		Pass {
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram

				half4 FragmentProgram (Interpolators i) : SV_Target {
					return half4(SampleBox(i.uv, 1),1);
				}
			ENDCG
		}
		Pass {
			Blend One One
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram

				half4 FragmentProgram (Interpolators i) : SV_Target {
					return half4(SampleBox(i.uv, 0.5),1);
				}
			ENDCG
		}
		Pass { 
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram

				half4 FragmentProgram (Interpolators i) : SV_Target {
					half4 c = tex2D(_SourceTex, i.uv);
					c.rgb += _BloomIntensity*SampleBox(i.uv, 0.5);
					return c;
				}
			ENDCG
		}
		Pass { // 4
			CGPROGRAM
				#pragma vertex VertexProgram
				#pragma fragment FragmentProgram

				half4 FragmentProgram (Interpolators i) : SV_Target {
					return half4(_BloomIntensity*SampleBox(i.uv, 0.5), 1);
				}
			ENDCG
		}
	}
}