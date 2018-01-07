Shader "Custom/Fan" {
	Properties
	{
		_Color("Color",Color) = (1,1,1,1)
		_Range("Range",Range(0.0,180.0)) = 0.0
	}
		SubShader
	{
		Pass
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		float4 _Color;
	float _Range;
	struct v2f
	{
		float4 pos : SV_POSITION;
		float4 localPos : TEXCOORD0;

	};
	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.localPos = v.vertex;
		return o;
	}

	float4 frag(v2f i) : COLOR
	{
	float d = distance(i.localPos.xyz,float3(0,0,0));
	if (d >= 0.5)discard;
	// 扇状にする
	float3 dir = normalize(i.localPos.xyz - float3(0, 0, 0));
	float angle = dot(dir, float3(0, 1, 0));
	angle = acos(angle) * 180 / 3.14;
	if (angle > _Range)discard;
	float p = 1.0 - (_Range - angle) / _Range;
	//-----------
	float t = _Time.x * 4;
	t = 1.0 - (t - 0.6 * floor(t / 0.6));
	float wave = pow(d + t, 10);
	if (wave >= 0.6)wave = 0.0;

	return float4(_Color.rgb, pow(d + 0.5,10) + wave + pow(p, 10));
	}
		ENDCG
	}
	}
}
