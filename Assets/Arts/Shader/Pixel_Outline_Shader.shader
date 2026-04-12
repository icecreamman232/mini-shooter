// PixelArtOutline_URP2D.shader
// Unity 6 · Universal Render Pipeline · 2D Renderer
// Works with atlas textures. Outline thickness is in pixel units.

Shader "Custom/PixelArtOutline_URP2D"
{
    Properties
    {
        _MainTex      ("Sprite Texture", 2D)    = "white" {}
        _Color        ("Tint",           Color) = (1,1,1,1)
        _OutlineColor ("Outline Color",  Color) = (1,1,1,1)
        _OutlineSize  ("Outline Size (px)", Integer) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType"            = "Transparent"
            "RenderPipeline"        = "UniversalPipeline"
            "Queue"                 = "Transparent"
            "IgnoreProjector"       = "True"
            "PreviewType"           = "Plane"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            // This tag is REQUIRED for URP 2D Renderer to include this pass
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag

            // URP core — replaces UnityCG.cginc
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // Declared inside CBUFFER for SRP Batcher compatibility
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _MainTex_TexelSize; // auto-filled: .xy = 1/w, 1/h (atlas space)
                half4  _Color;
                half4  _OutlineColor;
                int    _OutlineSize;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                half4  color      : COLOR;       // per-vertex sprite tint
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                half4  color       : COLOR;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv          = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color       = IN.color * _Color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // One pixel step in atlas UV space, scaled by outline thickness
                float2 texelStep  = _MainTex_TexelSize.xy * (float)_OutlineSize;

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // 4-directional neighbour alpha samples
                half aR = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2( texelStep .x,  0)).a;
                half aL = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(-texelStep .x,  0)).a;
                half aU = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2( 0,  texelStep .y)).a;
                half aD = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2( 0, -texelStep .y)).a;

                // Outline = neighbour is opaque AND current pixel is transparent
                half outline = step(0.01, aR + aL + aU + aD) * (1.0 - step(0.01, col.a));

                half4 result = lerp(col * IN.color, _OutlineColor, outline);
                result.a     = max(col.a, outline);

                return result;
            }
            ENDHLSL
        }
    }
}