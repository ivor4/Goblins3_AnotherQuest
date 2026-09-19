Shader "Custom/LabLiquidShader"
{
    Properties
    {
        _MainColor ("Color del Líquido", Color) = (0.5, 0.1, 0.2, 0.85)
        _RimColor ("Color de Resalte / Borde", Color) = (0.8, 0.3, 0.4, 1.0)
        _FlowSpeed ("Velocidad del Flujo", Float) = 3.0
        _WaveFreq ("Frecuencia de Onda", Float) = 8.0
        _WaveDistort ("Fuerza de Distorsión", Range(0, 0.2)) = 0.05
        _EdgeFalloff ("Nitidez del Borde", Range(0.5, 5.0)) = 1.8
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True" 
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // Color heredado del Particle System
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            fixed4 _MainColor;
            fixed4 _RimColor;
            float _FlowSpeed;
            float _WaveFreq;
            float _WaveDistort;
            float _EdgeFalloff;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // u: ancho del ribbon (0 a 1), v: longitud del chorro (0 a 1)
                float u = i.uv.x;
                float v = i.uv.y;

                // Animación de flujo vertical acompasada por el tiempo
                float flowTime = _Time.y * _FlowSpeed;

                // Ondulación senoidal combinada
                float wave1 = sin(v * _WaveFreq - flowTime);
                float wave2 = cos(v * (_WaveFreq * 1.5) - (flowTime * 1.3));
                float combinedWave = (wave1 + wave2 * 0.5) * _WaveDistort;

                // Distorsión del eje transversal (U)
                float distortedU = u + combinedWave;

                // Distancia normalizada desde el centro del chorro (0 en centro, 1 en bordes)
                float distFromCenter = abs(distortedU - 0.5) * 2.0;

                // Transparencia suavizada en los extremos del chorro
                float alphaMask = 1.0 - smoothstep(0.7, 1.0, distFromCenter);

                // Variación de transparencia a lo largo del cuerpo del líquido
                float waveAlpha = 0.8 + 0.2 * sin(v * (_WaveFreq * 2.0) - flowTime);

                // Resalte / destello en el centro para dar sensación de volumen 3D
                float innerHighlight = pow(1.0 - saturate(distFromCenter), 2.5);

                // Mezcla de colores: base + borde/reflejo interior
                fixed4 finalColor = lerp(_MainColor, _RimColor, innerHighlight);

                // Aplicar el color y alpha de la partícula
                finalColor.rgb *= i.color.rgb;
                finalColor.a *= _MainColor.a * alphaMask * waveAlpha * i.color.a;

                return finalColor;
            }
            ENDCG
        }
    }
}
