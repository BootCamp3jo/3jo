Shader "Unlit/URP2DShockWave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _WaveTime ("Wave Time", Float) = 0
        _Speed ("Speed", Float) = 1.0          // 진행 속도
        _WaveWidth ("Wave Width", Float) = 0.1 // 파동 폭
        _WaveCount ("Wave Count", Float) = 3   // 파동 개수
        _WaveGap ("Wave Gap", Float) = 0.3     // 파동 간격
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            Name "ShockWavePass"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _Center;
            float _WaveTime;
            float _Speed;
            float _WaveWidth;
            float _WaveCount;
            float _WaveGap;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                float2 dir = uv - _Center.xy;
                float dist = length(dir);

                float wave = 0.0;

                [unroll(10)]
                for (int i = 0; i < 10; i++)
                {
                    if (i >= (int)_WaveCount) break;

                    float waveRadius = _WaveTime * _Speed - i * _WaveGap;
                    float diff = abs(dist - waveRadius);

                    // 고정 왜곡량 (0.02), 부드러운 경계
                    wave += smoothstep(_WaveWidth, 0.0, diff) * 0.02;
                }

                uv += normalize(dir) * wave;

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                return col;
            }
            ENDHLSL
        }
    }
}
