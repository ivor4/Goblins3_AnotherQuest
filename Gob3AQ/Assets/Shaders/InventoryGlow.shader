Shader "Custom/2D Outline Vertex"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Cull Back // Importante: dibujar ambos lados del mesh

        // ===================================
        // PASS 1: EL BORDE BLANCO (Escalado)
        // ===================================

        Pass
        {
            ZWrite Off // No escribir en el Z-Buffer, dibujar "detrás"
            Blend SrcAlpha OneMinusSrcAlpha // Para que se vea la transparencia del sprite

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            static const fixed3 _OutlineColor = fixed3(1,1,1); // Color blanco para el borde
            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = _OutlineColor.rgb; // Cambiamos el color al del borde
                clip(col.a - 0.5); // Solo dibujar píxeles con suficiente opacidad
                return col; // Siempre pinta del color del borde
            }
            ENDCG
        }  
    }
}
