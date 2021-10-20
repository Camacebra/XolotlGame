Shader "Unlit/CameraFX"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _param("Param.", Float) = 0.5
        _fade("Fade", Float) = 1
        _p("Propety", Float) = 0
    }
        SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform float _param;
            uniform float _fade;
            uniform float _p;
            #define PP (_p * _p)
            float4 frag(v2f_img i) : COLOR {
                i.uv = round(500 * PP * i.uv)/(500 * PP);
                float4 color = tex2D (_MainTex, i.uv);
                color *= _fade;
                return lerp(float4(0,0,0,1), color, saturate(3*_p));
            }
            ENDCG
        }
    }
}
