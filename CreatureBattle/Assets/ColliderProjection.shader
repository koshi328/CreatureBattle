Shader "Custom/ColliderProjection" {
	Properties
	{
		_MainTex("MainTex",2D) = "white"{}
		_ProjTex("ProjectionTex",2D) = "white"{}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		ZTest off
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float4 projectionPos:TEXCOORD1;
};
	float4x4 ColliderVP;
	sampler2D _MainTex;
	sampler2D _ProjTex;
	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
		o.projectionPos = mul(ColliderVP, v.vertex);
		return o;
	}

		float4 frag(v2f i) : COLOR
		{
			float4 tex = tex2D(_MainTex,i.uv);
			i.projectionPos = i.projectionPos * 0.5 + 0.5;
			float4 proj = tex2D(_ProjTex, i.projectionPos.xy);
			float4 color = tex + proj;
			return color;
		}
		ENDCG
	}
	}
	FallBack "Diffuse"
}
