Shader "Hidden/CircleMaskEffect"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _Radius ("Radius", Float) = 0.2
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "DisableBatching"="True" }
        Pass
        {
            Name "CircleMaskPass"
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float _Radius;
            float2 _Center;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                float dist = distance(uv, _Center);

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

                if (dist > _Radius)
                {
                    // 흑백 변환 (luminance)
                    float gray = dot(col.rgb, float3(0.3, 0.59, 0.11));
                    col.rgb = float3(gray, gray, gray);
                }

                return col;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
