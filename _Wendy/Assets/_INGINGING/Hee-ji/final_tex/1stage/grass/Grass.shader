Shader "Custom/Grass" 
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGBA)", 2D) = "white" {}
	_MaskTex("Mask (RGBA)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

		SubShader{
		//뒷면
		Tags{ "Queue" = "AlphaTest" "Ignore Projector" = "True" "RenderType" = "TransparentCutout" }//Opaque"} //TransparentCutout"} //"LightMode" = "ForwardBase"
		LOD 200

		cull Back //off

		CGPROGRAM

#pragma surface surf Standard  alphatest:_Cutoff 
		//#pragma surface surf Standard alphatest:_Cutoff fullforwardshadows keepalpha
		//#pragma surface surf Lambert addshadow alphatest:_Cutoff
#pragma target 3.0

		sampler2D _MainTex;
	sampler2D _MaskTex;

	struct Input {
		float2 uv_MainTex;
		float2 uv2_MaskTex;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;

	void surf(Input IN, inout SurfaceOutputStandard o) {

		fixed4 main = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		fixed4 mask = tex2D(_MaskTex, IN.uv2_MaskTex);

		//Multiplied by the mask.rgb in this case, because if you use a white texture as your mask,
		//you can color it black to add burn marks around the damage.
		o.Albedo = main.rgb * mask.rgb;

		//For most cases, you'd probably just want:
		//surface.Albedo = main.rgb;
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;

		o.Alpha = main.a * mask.a;
	}
	ENDCG


		//뒷면
		Tags{ "Queue" = "AlphaTest" "Ignore Projector" = "True" "RenderType" = "TransparentCutout" } //
		LOD 200

		cull Front

		CGPROGRAM

#pragma surface surf Standard alphatest:_Cutoff 
#pragma vertex vert

#pragma target 3.0

		sampler2D _MainTex;
	sampler2D _MaskTex;

	struct Input {
		float2 uv_MainTex;
		float2 uv2_MaskTex;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;

	void vert(inout appdata_full v, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input, o);
		v.normal = -v.normal;
	}

	void surf(Input IN, inout SurfaceOutputStandard o) {

		fixed4 main = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		fixed4 mask = tex2D(_MaskTex, IN.uv2_MaskTex);

		//Multiplied by the mask.rgb in this case, because if you use a white texture as your mask,
		//you can color it black to add burn marks around the damage.
		o.Albedo = main.rgb * mask.rgb;

		//For most cases, you'd probably just want:
		//surface.Albedo = main.rgb;
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;

		o.Alpha = main.a * mask.a;
	}
	ENDCG


	}
		FallBack "Diffuse"
}