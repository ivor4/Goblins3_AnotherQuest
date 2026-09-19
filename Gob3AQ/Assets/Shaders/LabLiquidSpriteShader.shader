Shader "Custom/LabLiquidSpriteShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)

        [Header(Distortion Settings)]
        _WaveSpeed ("Velocidad de Onda", Float) = 2.5
        _WaveFreq ("Frecuencia Espacial", Float) = 12.0
        _WaveStrength ("Intensidad de Distorsion", Range(0.0, 0.2)) = 0.03
        
        [Header(Direction and Flow)]
        _FlowDirection ("Direccion del Flujo (XY)", Vector) = (0.0, -0.5, 0, 0)
        _Turbulence ("Turbulencia Cruzada", Range(0.0, 2.0)) = 0.8

        [Header(Alpha Modulation)]
        _MinAlpha ("Alfa Minimo (Huecos)", Range(0.0, 1.0)) = 0.35
        _MaxAlpha ("Alfa Maximo (Densidad)", Range(0.0, 1.0)) = 1.0
        _AlphaContrast ("Contraste de Turbulencia", Range(0.5, 4.0)) = 1.5
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
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float2 worldPos : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _RendererColor;
            float4 _Flip;

            float _WaveSpeed;
            float _WaveFreq;
            float _WaveStrength;
            float4 _FlowDirection;
            float _Turbulence;

            float _MinAlpha;
            float _MaxAlpha;
            float _AlphaContrast;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                IN.vertex.xy *= _Flip.xy;

                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                
                float4 worldPos = mul(unity_ObjectToWorld, IN.vertex);
                OUT.worldPos = worldPos.xy;

                OUT.color = IN.color * _Color * _RendererColor;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float time = _Time.y * _WaveSpeed;

                // 1. Coordenadas base en espacio de mundo y flujo temporal
                float2 coords = IN.worldPos * _WaveFreq;
                coords += _FlowDirection.xy * time;

                // 2. Ondas cruzadas
                float waveX = sin(coords.y + time) + cos(coords.x * 0.5 + time * 1.3) * _Turbulence;
                float waveY = cos(coords.x + time * 0.8) + sin(coords.y * 0.7 + time * 1.1) * _Turbulence;

                // 3. Offset de distorsión UV
                float2 distortion = float2(waveX, waveY) * _WaveStrength;
                float2 distortedUV = IN.texcoord + distortion;
                fixed4 c = tex2D(_MainTex, distortedUV) * IN.color;

                // 4. Modulación de Alfa por turbulencia interna:
                // Normalizamos el patrón de ondas cruzadas a un rango [0, 1]
                float rawDensity = (waveX + waveY) / (2.0 * (1.0 + _Turbulence));
                float normalizedDensity = saturate(rawDensity * 0.5 + 0.5);

                // Aplicamos contraste y mapeamos entre _MinAlpha y _MaxAlpha
                float densityFactor = pow(normalizedDensity, _AlphaContrast);
                float alphaModifier = lerp(_MinAlpha, _MaxAlpha, densityFactor);

                // Aplicamos la variación sobre el alfa propio del sprite
                c.a *= alphaModifier;

                // 5. Pre-multiplied alpha
                c.rgb *= c.a;

                return c;
            }
            ENDCG
        }
    }
}
