Shader "Custom/DreamShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveStrength ("Wave Strength", Range(0, 0.02)) = 0.003
        _WaveSpeed ("Wave Speed", Range(0, 10)) = 0.6
        _VignetteIntensity ("Vignette Intensity", Range(0, 2)) = 0.35
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            static const float3 GrayWeights = float3(0.2126, 0.7152, 0.0722);
            sampler2D _MainTex;
            float _WaveStrength;
            float _WaveSpeed;
            float _VignetteIntensity;

            fixed4 frag (v2f_img i) : SV_Target
            {
                // 1. Ondulación
                float2 uv = i.uv;
                uv.x += sin(uv.y * 10 + _Time.y * _WaveSpeed) * _WaveStrength;
                uv.y += cos(uv.x * 10 + _Time.y * _WaveSpeed) * _WaveStrength;

                // 2. Muestreo
                fixed4 col = tex2D(_MainTex, uv);

                // 3. Cálculo de distancia circular y máscara de desaturación
                float dist = distance(i.uv, float2(0.5, 0.5));
                float vignette = smoothstep(0.6, 0.5 - 0.3 * _VignetteIntensity, dist);
    
                // 4. Conversión a gris
                float gray = dot(col.rgb, GrayWeights);
                fixed4 graycol = fixed4(gray, gray, gray, col.a);
    
                // 5. Mezcla final
                return lerp(graycol, col, vignette);
            }
            ENDCG
        }
    }
}
