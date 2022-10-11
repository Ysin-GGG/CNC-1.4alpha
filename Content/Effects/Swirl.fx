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
float uStrength;//强度（旋转的角度）
float uRange;//影响半径（16f/图格）

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	return color;
}

technique Technique1
{
	pass Swirl
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}