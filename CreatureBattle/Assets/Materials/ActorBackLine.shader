Shader "Custom/ActorBackLine" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
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
			float3 normal : NORMAL;
		};

	sampler2D _MainTex;
	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
		o.normal = v.normal;
		return o;
	}

	float4 frag(v2f i) : COLOR
	{
		float4 tex = tex2D(_MainTex,i.uv);
		return tex;
	}
		ENDCG
	}
	}
	FallBack "Diffuse"
}
