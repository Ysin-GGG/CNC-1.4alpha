sampler uImage0 : register(s0);
sampler uImage1 : register(s1);//纹理

float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;//屏幕分辨率
float2 uScreenPosition;//屏幕坐标
float2 uTargetPosition;//滤镜中心
float2 uImageOffset;//纹理偏移度
float2 uDirection;
float2 uZoom;
float2 uImageSize0;
float2 uImageSize1;//纹理尺寸
float uIntensity;//扭曲强度上限(以tr的坐标单位为准)
float uProgress;//扭曲率（0%~100%）
float uOpacity;//纹理透明度
float uTime;
float uDist;//扭曲效果半径(以tr的坐标单位为准)

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float2 pixel = float2(1, 1) / uScreenResolution;//像素大小
	float2 position = uTargetPosition / uScreenResolution;//滤镜坐标转换（x,y = 0~1）
	float2 offset = coords - position;
	float dist = min(length(offset), length(float2(uIntensity, uIntensity) / uScreenResolution));//计算像素到放大中心的距离
	float4 color = tex2D(uImage0, coords);
	float4 trace = tex2D(uImage1, coords + uImageOffset * uTime);
	float intensity = trace.r * uProgress;
	//if (length(offset * uScreenResolution) <= uDist)
		color = tex2D(uImage0, position + offset * .99) + trace.r; 
	return color;
}

technique Technique1
{
	pass Absorption
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}