sampler uImage0 : register(s0);//主体
sampler uImage1 : register(s1);//刀尖
sampler uImage2 : register(s2);//纹理

float4x4 uTransform;
float4 uColor;//主色调
float4 uColor0;//主体
float4 uColor1;//刀尖
float4 uColor2;//纹理
float uAlpha;
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
	float y = uTime + coord.x;
	float4 color = float4(uColor.r, uColor.g, uColor.b, 1);
	float4 c1 = tex2D(uImage0, float2(coord.x, coord.y)) * uColor0;
	float4 c2 = tex2D(uImage1, float2(coord.x, coord.y)) * uColor1;
	float4 c3 = tex2D(uImage2, float2(y, coord.y)) * uColor2;
	return (c1 * c3 + c2) * input.Color * color * uAlpha;
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