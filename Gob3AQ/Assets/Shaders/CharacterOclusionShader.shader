Shader "Custom/CharacterOclusion"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [PerRendererData] _Color ("Tint", Color) = (1,1,1,1)
        [PerRendererData] _BlurAmount ("Blur Amount", Range(0, 1)) = 0
        
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "RenderPipeline" = "UniversalPipeline"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "SpriteUnlitBlur"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 uv       : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float4 color    : COLOR;
                float2 uv       : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float _BlurAmount;
                float4 _MainTex_TexelSize; // Unity lo rellena automáticamente con el tamaño del píxel (1/ancho, 1/alto)
            CBUFFER_END

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Multiplicador máximo de radio de blur en píxeles
                float blurRadius = _BlurAmount * 0.00025f;
                float brightness_amount = clamp(1.0 - (_BlurAmount * 0.6004), 0.0, 1.0);
                float4 brightness = float4(brightness_amount,brightness_amount,brightness_amount,1.0);

                half4 col = 0;
                float totalWeight = 0;

                // Muestreo de 9 puntos (Grid 3x3)
                // Usamos _MainTex_TexelSize para escalar correctamente según la resolución de la textura
                static const float2 offsets[9] = {
                    float2(-1, -1), float2(0, -1), float2(1, -1),
                    float2(-1,  0), float2(0,  0), float2(1,  0),
                    float2(-1,  1), float2(0,  1), float2(1,  1)
                };

                // Pesos sencillos para suavizar
                static const float weights[9] = {
                    1.0, 2.0, 1.0,
                    2.0, 4.0, 2.0,
                    1.0, 2.0, 1.0
                };

                [unroll]
                for (int j = 0; j < 9; j++)
                {
                    float2 sampleUV = i.uv + offsets[j] * blurRadius;
                    float w = weights[j];
                    col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, sampleUV) * w;
                    totalWeight += w;
                }

                col /= totalWeight;
                col *= i.color * brightness;

                return col;
            }
            ENDHLSL
        }
    }
}
