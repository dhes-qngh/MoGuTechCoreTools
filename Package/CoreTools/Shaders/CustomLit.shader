Shader "Custom/CustomLit"
{
    Properties
    {
        // Specular vs Metallic workflow
        [HideInInspector] _WorkflowMode("WorkflowMode", Float) = 1.0

        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        [HideInInspector] _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

        [HideInInspector]_Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        [HideInInspector]_SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
        [HideInInspector]_SpecGlossMap("Specular", 2D) = "white" {}

        [HideInInspector][ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [HideInInspector][ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0

        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        [HideInInspector]_Parallax("Scale", Range(0.005, 0.08)) = 0.005
        [HideInInspector]_ParallaxMap("Height Map", 2D) = "black" {}

        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        _OcclusionMap("Occlusion", 2D) = "white" {}

        [HDR] _EmissionColor("EmissionColor", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        [HideInInspector]_DetailMask("Detail Mask", 2D) = "white" {}
        [HideInInspector]_DetailAlbedoMapScale("Scale", Range(0.0, 2.0)) = 1.0
        [HideInInspector]_DetailAlbedoMap("Detail Albedo x2", 2D) = "linearGrey" {}
        [HideInInspector]_DetailNormalMapScale("Scale", Range(0.0, 2.0)) = 1.0
        [HideInInspector][Normal] _DetailNormalMap("Normal Map", 2D) = "bump" {}

        [Toggle(_DISS_ON)] _DISS("_DISS", Float) = 1
        _DissolveMap("DissolveMap", 2D) = "white" {}
        [HDR]_DissolveColor("DissolveColor", Color) = (0,0,0,0)
		_Dissolve("Dissolve", Range( 0 , 1)) = 0
        [KeywordEnum(Texture,Sphere,Plane)]_DISTYPE("DisType", Float) = 0
        [Toggle(_INVERT_ON)] _INVERT("Invert", Float) = 0
        _PlaneNormal ("Rotation", Vector) = (0,1,0,1)
        _NoiseRange("NoiseRange", Range(0, 1)) = 0.3
		_OutlineWidth("OutlineWidth", Range( 0 , 0.2)) = 0
        _SphereRadius("Sphere Radius", float) = 1
        _DissolvePos ("Dissolve Pos", Vector) = (0,0,0,0)

        [Toggle(_FRES_ON)] _FRES("_FRES", Float) = 0
        [Toggle(_TRANS_ON)] _TRANS("_TRANS", Float) = 1
        [HDR]_FresnelColor("FresnelColor", Color) = (0,0,0,1)
        _Power("Power",Range(0,10)) = 5
        _FresnelMask ("FresnelMask", 2D) = "white" {}
        [Toggle(_FSTEP_ON)] _FSTEP("FresnelStep", Float) = 0
        _FresnelSlider ("FresnelSlider", Range(0, 1)) = 0
        _FresnelFeather ("FresnelFeather", Range(0, 0.5)) = 0.5
        [Toggle(_FMOVE_ON)] _FMOVE("FresnelMove", Float) = 0
        _FresnelNoise("FresnelNoise", 2D) = "white"{}
        _FresnelSpeed("FresnelSpeed", Float) = 0

        [Toggle(_CHANGEMAP_ON)] _CHANGEMAP("CHANGE MAP", Float) = 0
        [KeywordEnum(Texture,Whole)]_ChangeType("ChangeType", Float) = 0
        _NewAlbedo("New Albedo", 2D) = "white"{}
        _NewEmission("New Emission", 2D) = "white" {}
        _ChangeMask("Mask", 2D) = "white" {}
        _Change("Change",Range(0,1)) = 0

        [Toggle(_USEFLIP_ON)] _UseFlip("UseFlip", Float) = 0
        [Toggle(_AFLIP_ON)]_AFlip("Albedo Flip", Float) = 0
        [Toggle(_EFLIP_ON)]_EFlip("Emission Flip", Float) = 0
        _Col("Col", Float) = 3
        _Row("Row", Float) = 2
        [IntRange]_Flip("Flip", Range(0, 99)) = 0

        [Toggle(_MATCAP_ON)] _MatCap("MatCap", float) = 0
        _MatCapTex("MatCap Texture", 2D) = "black"{}
        [Toggle(_CUBE_ON)] _Cube("Cube", float) = 0
        _CubeMap("CubeMap", Cube) = "black"{}
        _CubeIntensity("Cube Intensity", Range(0, 1)) = 1
        _CubeMask ("CubeMask", 2D) = "white"{}
        [KeywordEnum(Add,Mul)]_MASKTYPE("MaskType", float) = 0
        
        [Toggle(_HEIGHT_ON)] _HEIGHT("Height", float) = 0
        _HeightA("HeightA", 2D) = "black"{}
        _HeightB("HeightB", 2D) = "black"{}
        _HeightNormal("HeightNormal",Range(0,50)) = 50
        _VertexHeight("VertexHeight", Float) = 0.5
        _HeightSlider("HeightSlider", Range(0,1)) = 0
        
        // SRP batching compatibility for Clear Coat (Not used in Lit)
        [HideInInspector] _ClearCoatMask("_ClearCoatMask", Float) = 0.0
        [HideInInspector] _ClearCoatSmoothness("_ClearCoatSmoothness", Float) = 0.0

        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _Cull("__cull", Float) = 2.0
        [HideInInspector][ToggleUI] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0

        [HideInInspector] [ToggleUI] _ReceiveShadows("Receive Shadows", Float) = 1.0
        // Editmode props
        [HideInInspector]_QueueOffset("Queue offset", Float) = 0.0

        // ObsoleteProperties
        [HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
        [HideInInspector] _Color("Base Color", Color) = (1, 1, 1, 1)
        [HideInInspector] _GlossMapScale("Smoothness", Float) = 0.0
        [HideInInspector] _Glossiness("Smoothness", Float) = 0.0
        [HideInInspector] _GlossyReflections("EnvironmentReflections", Float) = 0.0

        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }

    SubShader
    {
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True" "ShaderModel"="4.5"}
        LOD 300

        // ------------------------------------------------------------------
        //  Forward pass. Shades all light in a single pass. GI + emission + Fog
        Pass
        {
            // Lightmode matches the ShaderPassName set in UniversalRenderPipeline.cs. SRPDefaultUnlit and passes with
            // no LightMode tag are also rendered by Universal Render Pipeline
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _SPECULAR_SETUP

            #pragma shader_feature_local _DISS_ON
            #pragma shader_feature_local _DISTYPE_TEXTURE _DISTYPE_SPHERE _DISTYPE_PLANE
            #pragma shader_feature_local _INVERT_ON
            #pragma shader_feature_local _CHANGEMAP_ON
            #pragma shader_feature_local _CHANGETYPE_WHOLE _CHANGETYPE_TEXTURE
            #pragma shader_feature_local _USEFLIP_ON
            #pragma shader_feature_local _AFLIP_ON
            #pragma shader_feature_local _EFLIP_ON
            #pragma shader_feature_local _FRES_ON
            #pragma shader_feature_local _FSTEP_ON
            #pragma shader_feature_local _FMOVE_ON
            #pragma shader_feature_local _TRANS_ON
            #pragma shader_feature_local _MATCAP_ON
            #pragma shader_feature_local _MASKTYPE_ADD _MASKTYPE_MUL
            #pragma shader_feature_local _CUBE_ON
            #pragma shader_feature_local _HEIGHT_ON

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _LIGHT_LAYERS
            #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #pragma multi_compile _ _CLUSTERED_RENDERING

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma multi_compile_fragment _ DEBUG_DISPLAY

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #ifndef UNIVERSAL_FORWARD_LIT_PASS_INCLUDED
            #define UNIVERSAL_FORWARD_LIT_PASS_INCLUDED
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // GLES2 has limited amount of interpolators
            #if defined(_PARALLAXMAP) && !defined(SHADER_API_GLES)
            #define REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR
            #endif

            #if (defined(_NORMALMAP) || (defined(_PARALLAXMAP) && !defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR))) || defined(_DETAIL)
            #define REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
            #endif

            // keep this file in sync with LitGBufferPass.hlsl
            #ifdef _FRES_ON
                float _Power;
                float4 _FresnelColor;
                #ifdef _FSTEP_ON
                    float _FresnelSlider;
                    float _FresnelFeather;
                    sampler2D _FresnelMask;
                #endif
                #ifdef _FMOVE_ON
                    sampler2D _FresnelNoise;
                    float _FresnelSpeed;
                #endif
            #endif
            #ifdef _USEFLIP_ON
                float _Col;
                float _Row;
                float _Flip;
            #endif
            #ifdef _DISS_ON
                float4 _DissolveColor;
                float _NoiseRange;
                float _OutlineWidth;
                sampler2D _DissolveMap;
                float4 _DissolveMap_ST;
                float _Dissolve;
                #if defined(_DISTYPE_PLANE)
                    float3 _PlaneNormal;
                #endif
                #if defined(_DISTYPE_SPHERE)
                    float _SphereRadius;
                #endif
                float3 _DissolvePos;
            #endif
            #ifdef _CHANGEMAP_ON
                sampler2D _NewAlbedo;
                sampler2D _NewEmission;
                float _Change;
                #if defined(_CHANGETYPE_TEXTURE)
                    sampler2D _ChangeMask;
                    float4 _ChangeMask_ST;
                #endif
            #endif
            #ifdef _MATCAP_ON
                sampler2D _MatCapTex;
                samplerCUBE _CubeMap;
                float _CubeIntensity;
                sampler2D _CubeMask;
            #endif
            #ifdef _HEIGHT_ON
                sampler2D _HeightA;
                sampler2D _HeightB;
                float _VertexHeight;
                float _HeightSlider;
                float _HeightNormal;
            #endif
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 texcoord     : TEXCOORD0;
                float2 staticLightmapUV   : TEXCOORD1;
                float2 dynamicLightmapUV  : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 uv                       : TEXCOORD0;

            #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                float3 positionWS               : TEXCOORD1;
            #endif

                float3 normalWS                 : TEXCOORD2;
            #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
                half4 tangentWS                : TEXCOORD3;    // xyz: tangent, w: sign
            #endif

            #ifdef _ADDITIONAL_LIGHTS_VERTEX
                half4 fogFactorAndVertexLight   : TEXCOORD5; // x: fogFactor, yzw: vertex light
            #else
                half  fogFactor                 : TEXCOORD5;
            #endif

            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                float4 shadowCoord              : TEXCOORD6;
            #endif

            #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
                half3 viewDirTS                : TEXCOORD7;
            #endif

                DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 8);
            #ifdef DYNAMICLIGHTMAP_ON
                float2  dynamicLightmapUV : TEXCOORD9; // Dynamic lightmap UVs
            #endif

                float4 positionCS               : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
            {
                inputData = (InputData)0;

                #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                    inputData.positionWS = input.positionWS;
                #endif

                    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                #if defined(_NORMALMAP) || defined(_DETAIL)
                    float sgn = input.tangentWS.w;      // should be either +1 or -1
                    float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                    half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);

                    #if defined(_NORMALMAP)
                    inputData.tangentToWorld = tangentToWorld;
                    #endif
                    inputData.normalWS = TransformTangentToWorld(normalTS, tangentToWorld);
                #else
                    inputData.normalWS = input.normalWS;
                #endif

                    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
                    inputData.viewDirectionWS = viewDirWS;

                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    inputData.shadowCoord = input.shadowCoord;
                #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                    inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
                #else
                    inputData.shadowCoord = float4(0, 0, 0, 0);
                #endif
                #ifdef _ADDITIONAL_LIGHTS_VERTEX
                    inputData.fogCoord = InitializeInputDataFog(float4(input.positionWS, 1.0), input.fogFactorAndVertexLight.x);
                    inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
                #else
                    inputData.fogCoord = InitializeInputDataFog(float4(input.positionWS, 1.0), input.fogFactor);
                #endif

                #if defined(DYNAMICLIGHTMAP_ON)
                    inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.dynamicLightmapUV, input.vertexSH, inputData.normalWS);
                #else
                    inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.vertexSH, inputData.normalWS);
                #endif

                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                inputData.shadowMask = SAMPLE_SHADOWMASK(input.staticLightmapUV);

                #if defined(DEBUG_DISPLAY)
                #if defined(DYNAMICLIGHTMAP_ON)
                inputData.dynamicLightmapUV = input.dynamicLightmapUV;
                #endif
                #if defined(LIGHTMAP_ON)
                inputData.staticLightmapUV = input.staticLightmapUV;
                #else
                inputData.vertexSH = input.vertexSH;
                #endif
                #endif
            }
           
            float Remap(float In, float2 InMinMax, float2 OutMinMax)
            {
                return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }
            
            #ifdef _HEIGHT_ON
                float SampleBlendedHeight(float2 uv)
                {
                    float h1 = tex2Dlod(_HeightA, float4(uv,0,0)).r;
                    float h2 = tex2Dlod(_HeightB, float4(uv,0,0)).r;
                    return lerp(h1, h2, _HeightSlider) * _VertexHeight;
                }
            #endif
            
            ///////////////////////////////////////////////////////////////////////////////
            //                  Vertex and Fragment functions                            //
            ///////////////////////////////////////////////////////////////////////////////

            // Used in Standard (Physically Based) shader
            Varyings LitPassVertex(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                #ifdef _HEIGHT_ON
                    float2 step = 0.01;
                    float h_center = SampleBlendedHeight(input.texcoord.xy);
                    input.positionOS.y += h_center;
                    // 采样四个邻近点计算梯度
                    float h_x = SampleBlendedHeight(input.texcoord.xy + float2(step.x, 0));
                    float h_y = SampleBlendedHeight(input.texcoord.xy + float2(0, step.y));
                    // 计算梯度（切线空间）
                    float2 gradient = float2(h_x - h_center, h_y - h_center);
                    gradient *= _HeightNormal;  // 控制凹凸强度
                    // 构建局部法线（朝上）
                    input.normalOS = normalize(float3(gradient.x, 1.0, gradient.y));
                #endif
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                // normalWS and tangentWS already normalize.
                // this is required to avoid skewing the direction during interpolation
                // also required for per-vertex lighting and SH evaluation
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);

                half fogFactor = 0;
                #if !defined(_FOG_FRAGMENT)
                    fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
                #endif

                output.uv.xy = TRANSFORM_TEX(input.texcoord, _BaseMap);
                output.uv.zw = input.staticLightmapUV;
                // already normalized from normal transform to WS.
                output.normalWS = normalInput.normalWS;
                #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR) || defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
                    real sign = input.tangentOS.w * GetOddNegativeScale();
                    half4 tangentWS = half4(normalInput.tangentWS.xyz, sign);
                #endif
                #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
                    output.tangentWS = tangentWS;
                #endif

                #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
                    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(vertexInput.positionWS);
                    half3 viewDirTS = GetViewDirectionTangentSpace(tangentWS, output.normalWS, viewDirWS);
                    output.viewDirTS = viewDirTS;
                #endif

                    OUTPUT_LIGHTMAP_UV(input.staticLightmapUV, unity_LightmapST, output.staticLightmapUV);
                #ifdef DYNAMICLIGHTMAP_ON
                    output.dynamicLightmapUV = input.dynamicLightmapUV.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                    OUTPUT_SH(output.normalWS.xyz, output.vertexSH);
                #ifdef _ADDITIONAL_LIGHTS_VERTEX
                    output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
                #else
                    output.fogFactor = fogFactor;
                #endif

                #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                    output.positionWS = vertexInput.positionWS;
                #endif

                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    output.shadowCoord = GetShadowCoord(vertexInput);
                #endif

                output.positionCS = vertexInput.positionCS;

                return output;
            }

            // Used in Standard (Physically Based) shader
            half4 LitPassFragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                #if defined(_PARALLAXMAP)
                    #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
                        half3 viewDirTS = input.viewDirTS;
                    #else
                        half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                        half3 viewDirTS = GetViewDirectionTangentSpace(input.tangentWS, input.normalWS, viewDirWS);
                    #endif
                    ApplyPerPixelDisplacement(viewDirTS, input.uv.xy);
                #endif

                #ifdef _DISS_ON
                    float Dissolve = 1;
                    float DissolveOutline = 0;
                    float2 uv_DissolveMap = input.uv.xy * _DissolveMap_ST.xy + _DissolveMap_ST.zw;
                    float noise = tex2D( _DissolveMap, uv_DissolveMap ).r;
                    
                    #if defined(_DISTYPE_SPHERE)
                        float distance = length(input.positionWS - _DissolvePos);
                        noise = Remap(noise, float2(0,1), float2(-_NoiseRange, _NoiseRange));
                        float control = noise + _SphereRadius;
                        #ifdef _INVERT_ON
                            Dissolve = step(control, distance);
                            DissolveOutline = step(distance - _OutlineWidth, control);
                        #else
                            Dissolve = step(distance, control);
                            DissolveOutline = step(control, distance + _OutlineWidth);
                        #endif
                        
                    #elif defined(_DISTYPE_PLANE)
                        
                        float distance = dot((input.positionWS - _DissolvePos), _PlaneNormal);
                        noise = Remap(noise, float2(0,1), float2(-_NoiseRange, _NoiseRange));
                        float control = noise;
                        Dissolve = step(distance ,control);
                        DissolveOutline = step(control, distance + _OutlineWidth);
                    #else
                        float clampResult = clamp(noise, 0.01, 0.99);
                        float mask = (_Dissolve * (1 + _OutlineWidth) - _OutlineWidth);
                        Dissolve = step(mask, clampResult);
                        DissolveOutline = (Dissolve - step((mask + _OutlineWidth), clampResult));
                    #endif
                #endif

                #ifdef _CHANGEMAP_ON
                float change = 0;
                    #if defined(_CHANGETYPE_WHOLE)
                        change = (1 - _Change);
                    #else
                        float2 maskUV = input.uv.xy * _ChangeMask_ST.xy + _ChangeMask_ST.zw;
                        change = saturate(tex2D(_ChangeMask, input.uv.xy).r - _Change);
                    #endif
                #endif

                SurfaceData surfaceData;
                half2 emissionUV = input.uv.xy;
                
                
                #ifdef _DISS_ON
                    _BaseColor.a *= Dissolve;
                #endif
                InitializeStandardLitSurfaceData(input.uv.xy, surfaceData);

                #ifdef _USEFLIP_ON
                    float total = _Col * _Row;
                    float colOffset = 1.0f / _Col;
                    float rowOffset = 1.0f / _Row;
                    float speed = _Flip * 1.0;
                    float2 tilling = float2(colOffset, rowOffset);
                    float index = round(fmod(speed + 0.0, total));
                    index += (index < 0) ? total : 0;
                    float linearIndex = round(fmod(index, _Col));
                    float offsetx = linearIndex * colOffset;
                    float indexToy = round(fmod((index - linearIndex) / _Col, _Row));
                    indexToy = (int)(_Row - 1) - indexToy;
                    float offsety = indexToy * rowOffset;
                    float2 offset = float2(offsetx, offsety);
                    half2 flipbookUV = input.uv.xy * tilling + offset;
                    #ifdef _AFLIP_ON
                        surfaceData.albedo = (_BaseColor * SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, flipbookUV)).rgb;
                    #endif
                    #ifdef _EFLIP_ON
                        surfaceData.emission = (_EmissionColor * SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, flipbookUV)).rgb;
                    #endif
                #endif
                
                InputData inputData;
                InitializeInputData(input, surfaceData.normalTS, inputData);
                SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv.xy, _BaseMap);
                #ifdef _CHANGEMAP_ON
                    half4 newAlbedo = tex2D(_NewAlbedo, input.uv.xy);
                    half4 newEmission = tex2D(_NewEmission ,emissionUV) * _EmissionColor;
                    surfaceData.albedo = lerp(surfaceData.albedo, newAlbedo, change);
                    surfaceData.emission = lerp(surfaceData.emission, newEmission, change);
                #endif

                
                #ifdef _DBUFFER
                    ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
                #endif

                #ifdef _FRES_ON
                    float3 worldViewDir = normalize(_WorldSpaceCameraPos.xyz - input.positionWS);
                    float Fcontrol = 1;
                    #ifdef _FSTEP_ON
                        float fresnelMask = tex2D(_FresnelMask, input.uv.zw).r;
                        float FSlider = Remap(_FresnelSlider, float2(0,1), float2(_FresnelSlider - _FresnelFeather, 1));
                        Fcontrol = smoothstep(FSlider, FSlider + _FresnelFeather, fresnelMask);
                    #endif
                    #ifdef _FMOVE_ON
                        Fcontrol *= tex2D(_FresnelNoise, input.uv.xy + _Time.y * _FresnelSpeed).r;
                    #endif
                    float fresnel = saturate(pow(1.0 - dot(inputData.normalWS, worldViewDir), _Power)) * Fcontrol;
                    surfaceData.emission += fresnel * _FresnelColor;
                    #ifdef _TRANS_ON
                        surfaceData.alpha *= fresnel;
                    #endif
                #endif

                #ifdef _DISS_ON
                    surfaceData.emission += _DissolveColor.rgb * DissolveOutline;
                #endif      
                half4 color = UniversalFragmentPBR(inputData, surfaceData);

                #ifdef _MATCAP_ON
                    float3 viewNormal = mul((float3x3)UNITY_MATRIX_V, inputData.normalWS);
                    float2 matcap_uv = viewNormal.xy * 0.5 + 0.5;
                    float4 matcapColor = tex2D(_MatCapTex, matcap_uv);
                    #ifdef _CUBE_ON
                        float3 ViewDirWS = normalize(_WorldSpaceCameraPos.xyz - input.positionWS);
                        float3 worldRefl = reflect(-ViewDirWS, inputData.normalWS);
                        float4 cubeColor = texCUBE(_CubeMap, worldRefl) * _CubeIntensity;
                        #ifdef _FRES_ON
                            cubeColor *= fresnel;
                        #endif
                        matcapColor += cubeColor; 
                    #endif
                    float matcapMask = tex2D(_CubeMask, input.uv.xy).r;
                    #if defined(_MASKTYPE_ADD)
                        color.rgb += matcapColor.rgb * matcapMask;
                    #elif defined(_MASKTYPE_MUL)
                        color.rgb = lerp(color.rgb, matcapColor.rgb, matcapMask);
                    #endif
                #endif
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                color.a = OutputAlpha(color.a, _Surface);
                
                return color;
            }
            #endif

            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local _DISS_ON
            #pragma shader_feature_local _DISTYPE_TEXTURE
            #pragma shader_feature_local _HEIGHT_ON

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            // -------------------------------------
            // Universal Pipeline keywords

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #ifndef UNIVERSAL_SHADOW_CASTER_PASS_INCLUDED
            #define UNIVERSAL_SHADOW_CASTER_PASS_INCLUDED

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            // Shadow Casting Light geometric parameters. These variables are used when applying the shadow Normal Bias and are set by UnityEngine.Rendering.Universal.ShadowUtils.SetupShadowCasterConstantBuffer in com.unity.render-pipelines.universal/Runtime/ShadowUtils.cs
            // For Directional lights, _LightDirection is used when applying shadow Normal Bias.
            // For Spot lights and Point lights, _LightPosition is used to compute the actual light direction because it is different at each shadow caster geometry vertex.
            float3 _LightDirection;
            float3 _LightPosition;

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 texcoord     : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 positionCS   : SV_POSITION;
            };
            #ifdef _DISS_ON
            #if defined(_DISTYPE_TEXTURE)
                float _Dissolve;
                float _OutlineWidth;
                sampler2D _DissolveMap;
                float4 _DissolveMap_ST;
            #endif
            #endif
            #ifdef _HEIGHT_ON
                sampler2D _HeightA;
                sampler2D _HeightB;
                float _VertexHeight;
                float _HeightSlider;
                float _HeightNormal;
            #endif
            
            float4 GetShadowPositionHClip(Attributes input)
            {
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

            #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                float3 lightDirectionWS = normalize(_LightPosition - positionWS);
            #else
                float3 lightDirectionWS = _LightDirection;
            #endif

                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

            #if UNITY_REVERSED_Z
                positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
            #else
                positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
            #endif

                return positionCS;
            }
            #ifdef _HEIGHT_ON
                float SampleBlendedHeight(float2 uv)
                {
                    float h1 = tex2Dlod(_HeightA, float4(uv,0,0)).r;
                    float h2 = tex2Dlod(_HeightB, float4(uv,0,0)).r;
                    return lerp(h1, h2, _HeightSlider) * _VertexHeight;
                }
            #endif
            Varyings ShadowPassVertex(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                #ifdef _HEIGHT_ON
                    float2 step = 0.01;
                    float h_center = SampleBlendedHeight(input.texcoord.xy);
                    input.positionOS.y += h_center;
                #endif
                output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
                output.positionCS = GetShadowPositionHClip(input);
                return output;
            }

            half4 ShadowPassFragment(Varyings input) : SV_TARGET
            {
                #ifdef _DISS_ON
                #if defined(_DISTYPE_TEXTURE)
                float mask = (_Dissolve * (1 + _OutlineWidth) - _OutlineWidth);
				float2 uv_DissolveMap = input.uv.xy * _DissolveMap_ST.xy + _DissolveMap_ST.zw;
				float clampResult = clamp( tex2D( _DissolveMap, uv_DissolveMap ).r , 0.01 , 0.99 );
				float Dissolve = step( mask , clampResult );
                _BaseColor.a *= Dissolve;
                #endif
                #endif
                Alpha(SampleAlbedoAlpha(input.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap)).a, _BaseColor, _Cutoff);
                return 0;
            }
            #endif
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ColorMask R
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Shader Stages
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            // -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ParallaxMapping.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #ifndef UNIVERSAL_DEPTH_ONLY_PASS_INCLUDED
            #define UNIVERSAL_DEPTH_ONLY_PASS_INCLUDED

            #if defined(LOD_FADE_CROSSFADE)
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
            #endif

            #if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
            #define _DETAIL
            #endif

            struct Attributes
            {
                float4 position     : POSITION;
                float3 normal       : NORMAL;
                float2 texcoord     : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 positionCS   : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings DepthOnlyVertex(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                #if defined(CURVEDWORLD_IS_INSTALLED) && !defined(CURVEDWORLD_DISABLED_ON)
                    CURVEDWORLD_TRANSFORM_VERTEX(input.position)
                #endif

                output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
                output.positionCS = TransformObjectToHClip(input.position.xyz);
                return output;
            }

            half DepthOnlyFragment(Varyings input) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                #if defined(_ALPHATEST_ON)
                    Alpha(SampleAlbedoAlpha(input.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap)).a, _BaseColor, _Cutoff);
                #endif

                #if defined(LOD_FADE_CROSSFADE)
                    LODFadeCrossFade(input.positionCS);
                #endif

                return 0;
            }
            #endif
            ENDHLSL
        }
        
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.StylizedLitShader"
}
