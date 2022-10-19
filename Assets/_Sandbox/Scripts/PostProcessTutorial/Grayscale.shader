Shader "Hidden/Custom/Grayscale"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/Colors.hlsl"
        //#include "UnityShaderVariables.cginc"
        #pragma target 3.0

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
        TEXTURE2D_SAMPLER2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture);
        TEXTURE2D_SAMPLER2D(_CameraMotionVectorsTexture, sampler_CameraMotionVectorsTexture);

    #if SOURCE_GBUFFER
        TEXTURE2D_SAMPLER2D(_CameraGBufferTexture2, sampler_CameraGBufferTexture2);
    #endif

        float4 _MainTex_TexelSize;
        float4 _Params;
        float _Blend;
        float4 _NearColor;
        float4 _DistanceColor;
        float4x4 _camToWorld;
        float _normal;

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            // #if SOURCE_GBUFFER
            //     float3 norm = SAMPLE_TEXTURE2D(_CameraGBufferTexture2, sampler_CameraGBufferTexture2, i.texcoordStereo).xyz * 2.0 - 1.0;
            //     float3 n = mul((float3x3)unity_WorldToCamera, norm);
            // // #else
            //     float4 cdn = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.texcoordStereo);
            //     float3 n = DecodeViewNormalStereo(cdn) * float3(1.0, 1.0, -1.0);
            // #endif

            // #if UNITY_COLORSPACE_GAMMA
            //     n = LinearToSRGB(n);
            // #endif

                float d = SAMPLE_DEPTH_TEXTURE_LOD(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoordStereo, 0);
                d *= _Blend;
                d = clamp(d, 0 , 1);


                float depth;
                //float4 n;
                //float4 cameraToWorld = Matrix4x4(cam.transform.position, cam.transform.rotation, Vector3.one).inverse;

                float3 normal;

                float4 cdn = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.texcoord);
                
                //DecodeDepthNormal(cdn, depth, normal);
                float3 n = DecodeViewNormalStereo(cdn) * float3(1.0, 1.0, 0);
                //n = n = mul((float3x3)_camToWorld, n);

                
                

                // float4 color = lerp(cdn, _DistanceColor, 1-d);

                if(n.x < 0)
                {
                    n.x *= -1;
                }

                // float up = dot(float3(0,1,0), normal);

                n.x *= _Blend;
                n.x = clamp(n.x, 0,1);

                return float4(n.xxx, 1);
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}