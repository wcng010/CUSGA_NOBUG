Shader "Unity Shaders/MyShader/PlayerShadow"
{
    SubShader
        {
            // No culling or depth
            Cull Off ZWrite Off ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Tags { "Queue"="Transparent" "RenderType"="Opaque" }
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
                };
    
                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };
                // 顶点着色获取
                v2f vert (appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.vertex.x+=0.1;
                    o.vertex.y-=0.1;
                    o.uv = v.uv;
                    o.uv.y = 1 - o.uv.y;
                    return o;
                }
                
                sampler2D _PlayerTex;
                
                fixed4 frag (v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_PlayerTex, i.uv);
                    col.rgb = 1 - step(0,col.a);
                    return col;
                }
                ENDCG
            }
        }
}
