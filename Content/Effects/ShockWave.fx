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
float uRange;//影响范围（16f/图格）
float uDist;//影响点到中心的距离（16f/图格）

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float2 pixel = float2(1, 1) / uScreenResolution;//像素大小
	float2 offset = (coords - uPosition) * uScreenResolution;//像素点到目标坐标的向量
	float norm = length(offset);//像素点到目标坐标的长度（模）
	float rotation = acos(offset * float2(1, 0) / norm);//向量角度
	float2 unit = pixel * float2(cos(rotation), sin(rotation)) * uStrength;

	float intensity = 0;//根据距离调整扭曲强度
	float minRange = uDist - uRange;//生效半径
	float maxRange = uDist + uRange;//生效半径
	if (minRange <= norm && norm <= maxRange) intensity = sin((1 - abs(uDist - norm) / uRange) * 0.7853981);//仅在这个区间内有扭曲效果
	float4 color = tex2D(uImage0, coords - unit * intensity);
	return color;
}

technique Technique1
{
	pass ShockWave
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}