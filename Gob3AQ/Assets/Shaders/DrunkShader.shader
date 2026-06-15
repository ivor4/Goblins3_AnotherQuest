Shader "Custom/DrunkShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortionStrength ("Amplitud", Range(0, 0.1)) = 0.02
        _Frequency ("Frecuencia", Range(0, 20)) = 5
        _Speed ("Velocidad", Range(0, 10)) = 1
        _ColorTint ("Color de Fade", Color) = (0.05, 0.547, 0.202, 0.106)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _DistortionStrength;
            float _Frequency;
            float _Speed;
            fixed4 _ColorTint;

            fixed4 frag (v2f_img i) : SV_Target
            {
                // Crear distorsión ondulatoria
                float2 uv = i.uv;

                if(i.uv.y > 0.9166f)
                {
                    fixed4 col = tex2D(_MainTex, uv);
                    return col;
                }
                else
                {
                    uv.x += sin(uv.y * _Frequency + _Time.y * _Speed) * _DistortionStrength;
                    uv.y += cos(uv.x * _Frequency + _Time.y * _Speed) * _DistortionStrength;

                    uv.y = min(uv.y, 0.9166f);

                    fixed4 col = tex2D(_MainTex, uv);
                
                    // Aplicar el color de "fade" (mezcla)
                    return lerp(col, _ColorTint, _ColorTint.a);
                }
            }
            ENDCG
        }
    }
}
