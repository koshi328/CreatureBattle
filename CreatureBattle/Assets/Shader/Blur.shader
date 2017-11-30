Shader "Custom/Blur" {
	Properties
	{
		_MainTex("MainTex",2D) = "white"{}
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

	sampler2D _MainTex;
	sampler2D _SubTex;


	float4 frag(v2f_img i) : COLOR
	{
		float4 main = tex2D(_MainTex,i.uv);
		float gray = main.r * 0.1 + main.g * 0.6 + main.b * 0.3;
		return float4(gray, gray, gray, 1.0);
	}
			ENDCG
		}
	}
}
