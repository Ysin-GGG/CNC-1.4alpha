sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float3 uColor;
float uOpacity;
float3 uSecondaryColor;
float uTime;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uImageOffset;
float uIntensity;
float uProgress;
float2 uDirection;
float2 uZoom;
float2 uImageSize0;
float2 uImageSize1;

float2 uPosition;
float uStrength;//强度

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float2 pixel = float2(1, 1) / uScreenResolution;//像素大小
	float2 offset = (coords - uPosition) * uScreenResolution;//像素点到目标坐标的向量
	float max = 8;
	float4 color = tex2D(uImage0, coords) / max;
	for (int i = 0; i < max; i++)
	{
		float intensity = (1 - uStrength) + (1 - i / max) * uStrength;
		float2 pos = offset * intensity;
		color += tex2D(uImage0, coords - pos) / max;
	}
	return color;
}

technique Technique1
{
	pass RadialBlur
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}