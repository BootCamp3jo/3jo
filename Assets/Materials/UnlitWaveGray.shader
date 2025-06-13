Shader "Custom/UnlitWaveGray"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PlayerPos ("Player Position", Vector) = (0,0,0,0)
        _WaveRadius ("Wave Radius", Float) = 0
        _GrayStrength ("Gray Strength", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float2 _PlayerPos;
            float _WaveRadius;
            float _GrayStrength;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float dist = distance(i.worldPos.xy, _PlayerPos);
                float gray = dot(col.rgb, float3(0.3,0.59,0.11));
                float3 grayCol = float3(gray, gray, gray);

                // 플레이어 위치 기준 파장 내부면 회색조 보간
                if(dist < _WaveRadius)
                {
                    col.rgb = lerp(col.rgb, grayCol, _GrayStrength);
                }

                return col;
            }
            ENDCG
        }
    }
}
