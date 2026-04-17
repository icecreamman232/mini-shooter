Shader "Custom/2D/DissolveSpriteURP_SelfContained"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0.0
        _EdgeWidth ("Edge Width", Range(0, 0.2)) = 0.05
        _EdgeColor ("Edge Color", Color) = (1, 0.6, 0.2, 1)
        
        // No external texture needed anymore!
        _NoiseTiling ("Noise Scale (higher = finer grain)", Float) = 5.0
    }

    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent" 
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
            "PreviewType" = "Plane"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Unlit"
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _Color;

            float _DissolveAmount;
            float _EdgeWidth;
            float4 _EdgeColor;
            float _NoiseTiling;

            // ===================================================================
            // PROCEDURAL 2D VALUE NOISE (self-contained, no texture required)
            // ===================================================================
            float rand(float2 co)
            {
                return frac(sin(dot(co, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float noise(float2 st)
            {
                float2 i = floor(st);
                float2 f = frac(st);

                float a = rand(i);
                float b = rand(i + float2(1.0, 0.0));
                float c = rand(i + float2(0.0, 1.0));
                float d = rand(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Sample sprite (pixel-art friendly)
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                half4 color = texColor * _Color;

                // === PROCEDURAL NOISE (replaces the old _NoiseTex) ===
                float2 noiseUV = i.uv * _NoiseTiling;
                float noiseValue = noise(noiseUV);                    // 0.0 - 1.0

                // Dissolve logic
                float cutout = noiseValue - _DissolveAmount;
                if (cutout < 0.0)
                    discard;

                // Edge glow/burn effect
                float edgeFactor = 1.0 - smoothstep(0.0, _EdgeWidth, cutout);
                color.rgb = lerp(color.rgb, _EdgeColor.rgb, edgeFactor);

                return color;
            }
            ENDHLSL
        }
    }

    FallBack "Universal Render Pipeline/2D/Sprite-Unlit"
}