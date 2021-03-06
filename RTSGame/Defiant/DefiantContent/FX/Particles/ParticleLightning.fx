﻿// Camera And World
float4x4 WVP;

// Fog Of War
sampler2D FOW : register(s0);

// Variety Of Lightning Images
sampler2D LMap : register(s1);
float Splits;

// Current Time
float Time;

struct VSI {
    float4 Position : POSITION0;
	float4x4 Instance : POSITION1;
    float2 UV : TEXCOORD0;
	float2 TimeType : TEXCOORD1;
	float4 Color : COLOR0;
};
struct VSO {
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
	float4 Color : COLOR0;
};

VSO VS(VSI input) {
	VSO output;

	// Project
	float4 wPos = mul(input.Instance, input.Position);
	output.Position = mul(wPos, WVP);

	// Pass UV
	output.UV = float2((input.TimeType.y + input.UV.x) / Splits, input.UV.y);
	output.Color = input.Color;

    return output;
}
float4 PS(VSO input) : COLOR0 {
    float4 col = tex2D(LMap, input.UV);
	clip(col.a - 0.1);
	return col * input.Color;
}

technique Default {
    pass Primary {
        VertexShader = compile vs_3_0 VS();
        PixelShader = compile ps_3_0 PS();
    }
}