Shader "Universal Render Pipeline/CompleteToonShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ToonThreshold("Toon Threshold", Range(0.0, 1.0)) = 0.5
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0.0, 0.05)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        // Outline Pass
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "UniversalForward" }
            Cull Front
            ZWrite Off
            ColorMask RGB

            HLSLPROGRAM
            #pragma vertex vertOutline
            #pragma fragment fragOutline
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct VaryingsOutline
            {
                float4 positionWS : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            VaryingsOutline vertOutline(Attributes IN)
            {
                VaryingsOutline OUT;
                float3 normalWorld = TransformObjectToWorldNormal(IN.normalOS);
                OUT.positionWS = UnityObjectToClipPos(IN.positionOS + normalWorld * _OutlineWidth);
                return OUT;
            }

            half4 fragOutline(VaryingsOutline IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }

        // Main Pass
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct AttributesMain
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct VaryingsMain
            {
                float4 positionWS : SV_POSITION;
                float2 uv : TEXCOORD0;
                NdotLInterpolators ndotlInterpolators;
            };

            float _ToonThreshold;
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            VaryingsMain vert(AttributesMain IN)
            {
                VaryingsMain OUT;
                OUT.positionWS = UnityObjectToClipPos(IN.positionOS);
                OUT.uv = IN.uv;
                OUT.ndotlInterpolators = PrepareNdotLInterpolators(IN.normalOS, IN.positionOS);
                return OUT;
            }

            half4 frag(VaryingsMain IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                half ndotl = ComputeNdotL(IN.ndotlInterpolators);
                half toonShade = step(_ToonThreshold, ndotl);
                return half4(color.rgb * toonShade, color.a);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
