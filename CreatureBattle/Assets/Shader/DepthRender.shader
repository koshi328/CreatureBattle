Shader "Custom/DepthRender" {

	SubShader
	{
		Pass
		{
			CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

			sampler2D _CameraDepthTexture;
			float4 frag(v2f_img i) : COLOR
			{
				return tex2D(_CameraDepthTexture,i.uv);
			}
			ENDCG
		}
	}
}
