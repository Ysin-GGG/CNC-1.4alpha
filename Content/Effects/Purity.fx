sampler uImage0 : register(s0);//贴图

float4 uColor;
float2 uImageSize;
float uAlpha;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	if (!any(tex2D(uImage0, coords))) return float4(0, 0, 0, 0);
	float2 pixel = 1 / uImageSize;
	float alpha = 0;
	float unit = 1.0 / 13.0;
	int size = 3;
	/*
	for (int i = 1; i < size; i++) if (any(tex2D(uImage0, coords + pixel * float2(i, 0)))) alpha += unit;
	for (int i = -1; i > -size; i--) if (any(tex2D(uImage0, coords + pixel * float2(i, 0)))) alpha += unit;
	for (int i = 1; i < size; i++) if (any(tex2D(uImage0, coords + pixel * float2(0, i)))) alpha += unit;
	for (int i = -1; i > -size; i--) if (any(tex2D(uImage0, coords + pixel * float2(0, i)))) alpha += unit;
	*/
	for(int i = -1; i <= 1; i++)
		for(int j = -1; j <= 1; j++)
			if (any(tex2D(uImage0, coords + pixel * float2(i, j)))) alpha += unit;
	if (any(tex2D(uImage0, coords + pixel * float2(2, 0)))) alpha += unit;
	if (any(tex2D(uImage0, coords + pixel * float2(-2, 0)))) alpha += unit;
	if (any(tex2D(uImage0, coords + pixel * float2(0, 2)))) alpha += unit;
	if (any(tex2D(uImage0, coords + pixel * float2(0, -2)))) alpha += unit;
	return uColor * uAlpha * sqrt(alpha);
}

technique Technique1
{
	pass Purity
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}