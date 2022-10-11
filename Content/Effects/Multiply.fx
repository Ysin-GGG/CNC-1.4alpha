sampler uImage0 : register(s0);//贴图
sampler uImage1 : register(s1);//纹理

float4 uColor;
float2 uImageSize0;//贴图大小
float2 uImageSize1;//纹理大小
float2 uOffset;
float uAlpha;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	if (!any(color)) return float4(0, 0, 0, 0);
	float4 color2 = tex2D(uImage1, coords * (uImageSize0 / uImageSize1) + uOffset);
	return uColor * uAlpha * color * color2;
}

technique Technique1
{
	pass Multiply
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}