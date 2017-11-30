Shader "Custom/RayMarch" {
	Properties
	{
		_BoxSize("BoxSize",Float) = 0.0
		_Interval("Interval",Float) = 0.0
	}
	SubShader
	{
		Pass
		{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		struct v2f
		{
			float4 pos : SV_POSITION;
			float4 screenPos : TEXCOORD1;
			float4 wPos : TEXCOORD0;
			float3 normal : NORMAL;
		};
	float3 _Scale;
	float _BoxSize;
	float _Interval;
	float3 mod(float3 a, float b)
	{
		a.x = a.x - b * floor(a.x / b);
		a.y = a.y - b * floor(a.y / b);
		a.z = a.z - b * floor(a.z / b);
		return a;
	}

	float3 repeat(float3 pos, float interval)
	{
		return mod(pos, interval) - interval * 0.5;
	}

	float sdbox(float3 pos, float3 size)
	{
		return length(max(abs(pos) - size, 0.0));
	}
	float distance_func(float3 pos)
	{
		return sdbox(repeat(pos, 4 - min((abs(sin(_Time.x * 6) * 3)), 2.0)), 0.3) - 0.1;
	}

	float3 getNormal(float3 pos)
	{
		float e = 0.001;
		return normalize(float3(distance_func(float3(pos.x + e, pos.y, pos.z)) - distance_func(float3(pos.x - e, pos.y, pos.z)),
								distance_func(float3(pos.x, pos.y + e, pos.z)) - distance_func(float3(pos.x, pos.y - e, pos.z)),
								distance_func(float3(pos.x, pos.y, pos.z + e)) - distance_func(float3(pos.x, pos.y, pos.z - e))));
	}

	float3 ToLocal(float3 pos)
	{
		return mul(unity_WorldToObject, float4(pos, 1.0)).xyz * abs(_Scale);
	}

	bool InBox(float3 pos, float3 size)
	{
		return abs(pos.x) < size.x * 0.5 && abs(pos.y) < size.y * 0.5 && abs(pos.z) < size.z * 0.5;
	}

		v2f vert(appdata_base v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.screenPos = v.vertex;
			o.wPos = mul(unity_ObjectToWorld, v.vertex);
			o.normal = normalize(v.normal);
			return o;
		}

		float4 frag(v2f input) : COLOR
		{
			float3 ray = input.wPos;
			float3 rayDir = normalize(ray - _WorldSpaceCameraPos);
			float d = 0.0;
			for (int i = 0; i < 50; i++)
			{
				d = distance_func(ToLocal(ray));
				ray += d * rayDir;
				if (d < 0.001)
				{
					float3 lightDir = float3(0, 1, 0);
					float light = dot(ToLocal(normalize(lightDir)), getNormal(ToLocal(ray))) * 0.5 + 0.5;
					float4 color = float4(0.1, 0.0, 1.0, 1.0);
					return color * light;
				}
				if (!InBox(ToLocal(ray), abs(_Scale)))
				{
					break;
				}
			}
			discard;
			return float4(0, 0, 0, 1);
		}
		ENDCG
		}
	}
}
