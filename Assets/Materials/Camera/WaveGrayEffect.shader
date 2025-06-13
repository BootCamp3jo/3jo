Shader "Custom/WaveGrayEffect"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _PlayerPos ("Player Position (Screen Space)", Vector) = (0,0,0,0)
        _WaveRadius ("Wave Radius (Screen Space)", Float) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _PlayerPos;    // 화면 좌표 (0~1)
            float _WaveRadius;    // 화면 좌표 단위 반경

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;

                fixed4 col = tex2D(_MainTex, uv);

                // 플레이어 위치와 현재 픽셀의 거리 계산
                float dist = distance(uv, _PlayerPos.xy);

                if (dist <= _WaveRadius)
                {
                    // 흑백 변환 (가중치는 단순 평균)
                    float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
                    col.rgb = lerp(col.rgb, float3(gray, gray, gray), 1.0);
                }

                return col;
            }
            ENDCG
        }
    }
}
