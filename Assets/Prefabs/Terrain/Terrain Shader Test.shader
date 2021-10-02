Shader "Custom/Terrain Shader Test"
{
    Properties
    {
        _MainTex("_MainTexture", 2D) = "white" {}
        _KernelSize("Kernel Size (N)", Int) = 7
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            Pass
            {
                CGPROGRAM
                #pragma vertex vert_img
                #pragma fragment frag

                #include "UnityCG.cginc"

                sampler2D _MainTexture;
                float2 _MainTex_TexelSize;

                int _KernelSize;

                fixed4 frag(v2f_img i) : SV_Target
                {
                    fixed3 tex = tex2D(_MainTexture, i.uv);

                    return fixed4(tex, 1.0);
                }
                ENDCG
            }
        }
}
