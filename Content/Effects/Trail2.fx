sampler uImage0 : register(s0);//颜色素材
sampler uImage1 : register(s1);//纹理素材
sampler uImage2 : register(s2);//拖尾素材

float4x4 uTransform;
float4 uColor0;//亮度高时附加的颜色
float4 uColor1;//亮度低时附加的颜色
float2 uOffset;//纹理偏移
float uAlpha;//图像透明度
float uTime;

struct VSInput {
	float2 Pos : POSITION0;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};

struct PSInput {
	float4 Pos : SV_POSITION;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};

float4 PixelShaderFunction(PSInput input) : COLOR0
{
	float3 coord = input.Texcoord;
	float4 color0 = tex2D(uImage0, float2(coord.x, coord.y));
	float4 color1 = tex2D(uImage1, float2(coord.x, coord.y));
	float4 color2 = tex2D(uImage2, float2(coord.x, coord.y) + uOffset * uTime);
	float4 color = color0 * color1 * color2 * (color2.r * uColor0 + (1 - color2.r) * uColor1);
	if (color.r < .0001) return float4(0, 0, 0, 0);
	return input.Color * color * uAlpha;

}

PSInput VertexShaderFunction(VSInput input)  {
	PSInput output;
	output.Color = input.Color;
	output.Texcoord = input.Texcoord;
	output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
	return output;
}


technique Technique1 {
	pass ColorBar {
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}