Shader "Custom/CollisionDebug" {

	SubShader
	{
		Pass
		{

		Tags{"RenderType" = "Transparent" "Queue" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull front

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 wPos : TEXCOORD0;
				float3 normal : NORMAL;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.wPos = mul(UNITY_MATRIX_MV,v.vertex);
				o.normal = v.normal;
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				float3 camDir = normalize(_WorldSpaceCameraPos - i.wPos.xyz);
				float edge = 1 - dot(camDir, i.normal);
				edge *= edge;
				edge *= 0.5;
				return float4(0.2, 0.2, 0.2, edge);
			}
			ENDCG
		}
	}
}
