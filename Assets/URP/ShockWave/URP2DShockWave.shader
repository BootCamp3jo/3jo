Shader "Unlit/URP2DShockWave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _WaveTime ("Wave Time", Float) = 0
        _Speed ("Speed", Float) = 1.0
        _WaveWidth ("Wave Width", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "Queue"="Transparent" }
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

                // 파장 위치 (시간에 속도 곱해서 조절)
                float waveRadius = _WaveTime * _Speed;

                // 링 두께
                float waveWidth = _WaveWidth;

                // 현재 픽셀과 파장 링 중심과 차이
                float diff = abs(dist - waveRadius);

                // 부드러운 링 형태 (두께 안쪽만 왜곡)
                float wave = smoothstep(waveWidth, 0.0, diff) * 0.02;

                uv += normalize(dir) * wave;

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                return col;
            }
            ENDHLSL
        }
    }
}
