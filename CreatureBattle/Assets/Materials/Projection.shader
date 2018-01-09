Shader "Custom/Projection" {
	Properties
	{
		_MainTex("MainTex",2D) = "white"{}
	}
	SubShader
	{
		Pass
		{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "AutoLight.cginc"

		struct v2f
		{
			float4 pos : SV_POSITION;
			float3 normal : NORMAL;
			float2 uv : TEXCOORD0;
			float4 projPos : TEXCOORD1;
		};

	sampler2D _MainTex;
	sampler2D _ProjectionTex;
	float4x4 _ProjVP;

		v2f vert(appdata_base v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.normal = v.normal;
			o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
			o.projPos = mul(_ProjVP, mul(unity_ObjectToWorld, v.vertex));
			return o;
		}

		float4 frag(v2f i): COLOR
		{
			float4 proj = tex2D(_ProjectionTex,i.projPos.xy);
			float4 color = tex2D(_MainTex,i.uv);
			float3 lightDir = float3(0, 1, 0);
			float light = dot(i.normal, lightDir) * 0.5 + 0.5;
			color *= light;
			color += proj;
		return color;
		}
		ENDCG
		}
	}
	FallBack "Diffuse"
}
