sampler uImage0 : register(s0);//贴图
sampler uImage1 : register(s1);//纹理

float4 uColor0;//外轮廓颜色
float4 uColor1;//暗淡轮廓颜色
float2 uImageSize;//贴图尺寸
float2 uSize;//绘制尺寸
float2 uOffset;//纹理偏移
float uAlpha;//透明度
float uFrame;//贴图纵向第几帧

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float2 pixel = 1 / uImageSize;
	float2 coords2 = coords - pixel * uSize * float2(0, uFrame);//纹理坐标=贴图尺寸-贴图坐标
	float amount = tex2D(uImage1, coords2 + uOffset).r;
	float4 color2 = (uColor0 * amount + uColor1 * (1 - amount)) * uAlpha;
	float4 color = tex2D(uImage0, coords) * uAlpha;

	if (!any(color))//因为是要围着贴图轮廓加一层颜色，因此这个像素必须是空白的
	{
		if (any(tex2D(uImage0, coords + pixel * float2(1, 0)))) return color2;
		if (any(tex2D(uImage0, coords + pixel * float2(-1, 0)))) return color2;
		if (any(tex2D(uImage0, coords + pixel * float2(0, 1)))) return color2;
		if (any(tex2D(uImage0, coords + pixel * float2(0, -1)))) return color2;
		if (any(tex2D(uImage0, coords + pixel * float2(1, 1)))) return color2;
		if (any(tex2D(uImage0, coords + pixel * float2(1, -1)))) return color2;
		if (any(tex2D(uImage0, coords + pixel * float2(-1, 1)))) return color2;
		if (any(tex2D(uImage0, coords + pixel * float2(-1, -1)))) return color2;
	}
	return color;
}

technique Technique1
{
	pass HighLight
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}