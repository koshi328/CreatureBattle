Shader "Custom/Quad" {
	Properties
	{
		_Color("Color",Color) = (1,1,1,1)
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
		float d = max((i.localPos.x * i.localPos.x) , (i.localPos.y * i.localPos.y));

	float t = _Time.x * 4;
	t = 1.0 - (t - 0.6 * floor(t / 0.6));
	float wave = pow(d + t, 10);
	if (wave >= 0.6)wave = 0.0;
	return float4(_Color.rgb, pow(d + 0.75, 15) + wave);
	}
		ENDCG
	}
	}
}
