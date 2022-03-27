//来自https://www.cnblogs.com/kekec/p/14762727.html
//四维float可代表rgba和xyzw，但不宜混用
float1 uFloat1;//一维
float2 uFloat2;//二维
float3 uFloat3;//三维
float4 uFloat4;//四维
int1 uInt1;//一维
int2 uInt2;//二维
int3 uInt3;//三维
int4 uInt4;//四维
sampler uImage0 : register(s0);//采样器
sampler uImage1 : register(s1);//用于从C++传递采样器设置给HLSL
sampler uImage2 : register(s2);//128个
Texture1D uTex1D : register(t0);//着色器资源视图 (SRV)，
Texture2D uTex2D2 : register(t1);//用于从C++传递只读内存块或纹理数据给HLSL
Texture3D uTex2D3 : register(t2);//128个
SamplerState uSampler : register(s3);//贴图采样器
SamplerComparisonState uSamplerComparison1 : register(s4);//阴影贴图采样器

float4x4 uRect;//矩阵:4个float4
/*
float4 VertexShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);//获取当前纹理坐标的颜色(rgba)
	return color;
}
*/
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0//TexCoord=纹理坐标（u,v）， Color=漫反射/镜面反射颜色
{
	float4 color = tex2D(uImage0, coords);//获取当前纹理坐标的颜色(rgba)
	return color;
}


technique Technique1 {
	pass TestName {
		//VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}