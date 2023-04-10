Shader "Unlit/ObjectShadow"
{
    Properties {
                _xOffSet("_xOffSet",Float) = 1
    }
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

                float _xOffSet;
                
                sampler2D _PlayerTex;
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
                    float4 offset;
                    offset.x = _xOffSet*v.vertex.y;
                    offset.yzw = float3(0,0,0);
                    o.vertex = UnityObjectToClipPos(v.vertex+offset);
                    o.uv = v.uv;
                    return o;
                }
                
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