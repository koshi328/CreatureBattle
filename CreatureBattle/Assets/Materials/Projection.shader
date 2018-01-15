Shader "Custom/Projection" {
	Properties
	{
		_MainTex("MainTex",2D) = "white"{}
		_ProjectionTex("ProjectionTex",2D) = "white"{}
	}
	SubShader
	{
		Pass
		{
		Tags{"RenderType"="Opaque" "LightMode"="ForwardBase"}
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fwdbase
#include "UnityCG.cginc"
#include "AutoLight.cginc"

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 projPos : TEXCOORD1;
			float3 lightDir : TEXCOORD2;
			float3 normal : TEXCOORD3;
			LIGHTING_COORDS(5, 6)
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
			TRANSFER_VERTEX_TO_FRAGMENT(o);
			TRANSFER_SHADOW(o);
			return o;
		}

		float4 frag(v2f i): COLOR
		{
			float atten = LIGHT_ATTENUATION(i);
			float4 proj = tex2D(_ProjectionTex,i.projPos.xy);
			if (i.projPos.x < 0 || i.projPos.x > 1.0f || i.projPos.y < 0 || i.projPos.y > 1.0f)
			{
				proj = float4(0, 0, 0, 0);
			}
			float4 color = tex2D(_MainTex,i.uv);
			float3 lightDir = float3(0, 1, 0);
			float light = dot(i.normal, lightDir) * 0.5 + 0.5;
			color *= light * atten;
			color += proj;
		return color;
		}
		ENDCG
		}
	}

	FallBack "Diffuse"
}
