Shader "Hidden/posterize" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_numColors ("num colors", Float) = 4
	_gamma ("gamma", Float) = .9
}	

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform Texture2D _MainTex;
half4 _MainTex_ST;
uniform float _gamma; // 0.
uniform float _numColors;
SamplerState my_trilinear_clamp_sampler;	
fixed4 frag (v2f_img i) : SV_Target
{	
	fixed4 original = _MainTex.Sample(my_trilinear_clamp_sampler,i.uv);
	float3 c = float3(original.x,original.y,original.z);
  	c = pow(c, float3(_gamma, _gamma, _gamma));
  	c = c * _numColors;
  	c = floor(c);
  	c = c / _numColors;
  	c = pow(c, float3(1.0/_gamma,1.0/_gamma,1.0/_gamma));
  	fixed4 output = fixed4(c, 1.0);
	
	return output;
}
ENDCG

	}
}

Fallback off

}
