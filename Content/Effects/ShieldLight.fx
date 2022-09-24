sampler uImage0 : register(s0);//贴图
sampler uImage1 : register(s1);//纹理

float3 uColor;//纹理颜色
float2 uOffset;//纹理偏移
float2 uSize;//绘制区域
float2 uCenter;//放大中心
float uDist;//最小放大距离
float uAlpha;//透明度

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float2 offset = coords - uCenter;//获取当前像素到放大中心的向量
	float2 rpos = offset * float2(uSize.x / uSize.y, 1);//长宽比修正
	float dist = length(rpos);//计算像素到放大中心的距离

	float4 color = float4(uColor.r, uColor.g, uColor.b, 0);
	float4 c1 = tex2D(uImage0, coords);
	float4 c2 = tex2D(uImage1, uCenter + uOffset + offset * max(uDist, dist));

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