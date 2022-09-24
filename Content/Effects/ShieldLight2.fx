sampler uImage0 : register(s0);//贴图
sampler uImage1 : register(s1);//纹理

float3 uColor;
float2 uOffset;
float2 uSize;//绘制区域尺寸
float2 uImageSize;//盾贴图原尺寸
float uAlpha;
float uFrame;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float2 pixel = 1 / uImageSize; //盾贴图像素大小
	float2 realPos = pixel * uSize * float2(0, uFrame);//当前帧在着色器里的坐标
	float2 coords2 = coords - realPos;//纹理坐标

	float2 center = float2(.5, coords2.y + pixel.y * uSize.y * .5);//float2(.5, .5);
	float2 offset = coords2 - center;
	float2 rpos = offset * float2(uSize.x / uSize.y, 1);
	float dist = length(rpos);

	float4 color = float4(uColor.r, uColor.g, uColor.b, 0);
	float4 c1 = tex2D(uImage0, coords);
	float4 c2 = tex2D(uImage1, uOffset + center + offset * max(.3, dist));

	if (c1.r < .1) return float4(0, 0, 0, 0);
	return (c1 + c2 * color) * uAlpha;
}

technique Technique1
{
	pass ShieldLight
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}