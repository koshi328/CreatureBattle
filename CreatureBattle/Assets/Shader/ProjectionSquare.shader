Shader "Custom/ProjectionSquare" {
	SubShader
	{
		Pass
		{
		Tags{ "RenderType" = "Opaque" "LightMode" = "forwardBase" }
		LOD 200
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fwdbase
#include "UnityCG.cginc"
#include "AutoLight.cginc"

			struct v2f
			{
				float4 pos:SV_POSITION;
				float2 uv :TEXCOORD0;
				float4 wPos:TEXCOORD1;
				float3 lightDir:TEXCOORD2;
				float3 normal : TEXCOORD3;
				LIGHTING_COORDS(5, 6)
			};

			float3 mod(float3 a, float b)
			{
				a.x = a.x - b * floor(a.x / b);
				a.y = a.y - b * floor(a.y / b);
				a.z = a.z - b * floor(a.z / b);
				return a;
			}


			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				o.wPos = mul(unity_ObjectToWorld, v.vertex);
				o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
				o.normal = normalize(v.normal).xyz;

				TRANSFER_VERTEX_TO_FRAGMENT(o);
				TRANSFER_SHADOW(o);
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				float atten = LIGHT_ATTENUATION(i);
				float crossLine = 0.0;
				float3 pos = mod(abs(i.wPos.xyz), 2.5);
				float d = length(max(abs(pos) - 2.4, 0.0));
				if (d > 0.01)
				{
					crossLine = 1.0;
				}
				
				float light = dot(i.lightDir, i.normal) * 0.5 + 0.5;
				light *= (atten + 1) * 0.5;
				float4 c = float4(0.2, 0.2, 0.2, 1);
				c *= light + crossLine;
				return c;
			}
			ENDCG
		}

	}
	FallBack "Diffuse"
}