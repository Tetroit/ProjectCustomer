#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

void MainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out float Attenuation)
{
#ifdef SHADERGRAPH_PREVIEW
     Direction = normalize(float3(1.0f, 1.0f, 0.0f));
     Color = 1.0f;
     Attenuation = 1.0f;
#else
    Light mainLight = GetMainLight();
    Direction = mainLight.direction;
    Color = mainLight.color;
    Attenuation = mainLight.distanceAttenuation;
#endif
}

void ExtraLight_float(float3 WorldPos, float3 WorldNormal, out float3 Color)
{
#ifdef SHADERGRAPH_PREVIEW
     Color = 0.0f;
#else
    Color = float3(0,0,0);
    int lightCount = GetAdditionalLightsCount();
    for (int i = 0; i < lightCount; i++)
    {
        Light light = GetAdditionalLight(i, WorldPos);
        float3 color = dot(light.direction, WorldNormal);
        color = clamp(color, 0, 1);
        color *= light.color;
        Color += color;
    }
#endif
}
#endif