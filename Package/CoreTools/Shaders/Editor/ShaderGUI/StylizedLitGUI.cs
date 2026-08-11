using System;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    [MovedFrom("UnityEditor.Rendering.LWRP.ShaderGUI")] public static class StylizedLitGUI
    {
        public enum WorkflowMode
        {
            Specular = 0,
            Metallic
        }

        public enum SmoothnessMapChannel
        {
            SpecularMetallicAlpha,
            AlbedoAlpha,
        }

        public static class Styles
        {
            public static GUIContent workflowModeText = EditorGUIUtility.TrTextContent("Workflow Mode",
                "Select a workflow that fits your textures. Choose between Metallic or Specular.");

            public static GUIContent specularMapText =
                EditorGUIUtility.TrTextContent("Specular Map", "Designates a Specular Map and specular color determining the apperance of reflections on this Material's surface.");

            public static GUIContent metallicMapText =
                EditorGUIUtility.TrTextContent("Metallic Map", "Sets and configures the map for the Metallic workflow.");

            public static GUIContent smoothnessText = EditorGUIUtility.TrTextContent("Smoothness",
                "Controls the spread of highlights and reflections on the surface.");

            public static GUIContent smoothnessMapChannelText =
                EditorGUIUtility.TrTextContent("Source",
                    "Specifies where to sample a smoothness map from. By default, uses the alpha channel for your map.");

            public static GUIContent highlightsText = EditorGUIUtility.TrTextContent("Specular Highlights",
                "When enabled, the Material reflects the shine from direct lighting.");

            public static GUIContent reflectionsText =
                EditorGUIUtility.TrTextContent("Environment Reflections",
                    "When enabled, the Material samples reflections from the nearest Reflection Probes or Lighting Probe.");

            public static GUIContent heightMapText = EditorGUIUtility.TrTextContent("Height Map",
                "Defines a Height Map that will drive a parallax effect in the shader making the surface seem displaced.");

            public static GUIContent occlusionText = EditorGUIUtility.TrTextContent("Occlusion Map",
                "Sets an occlusion map to simulate shadowing from ambient lighting.");

            public static readonly string[] metallicSmoothnessChannelNames = { "Metallic Alpha", "Albedo Alpha" };
            public static readonly string[] specularSmoothnessChannelNames = { "Specular Alpha", "Albedo Alpha" };

            public static GUIContent clearCoatText = EditorGUIUtility.TrTextContent("Clear Coat",
                "A multi-layer material feature which simulates a thin layer of coating on top of the surface material." +
                "\nPerformance cost is considerable as the specular component is evaluated twice, once per layer.");

            public static GUIContent clearCoatMaskText = EditorGUIUtility.TrTextContent("Mask",
                "Specifies the amount of the coat blending." +
                "\nActs as a multiplier of the clear coat map mask value or as a direct mask value if no map is specified." +
                "\nThe map specifies clear coat mask in the red channel and clear coat smoothness in the green channel.");

            public static GUIContent clearCoatSmoothnessText = EditorGUIUtility.TrTextContent("Smoothness",
                "Specifies the smoothness of the coating." +
                "\nActs as a multiplier of the clear coat map smoothness value or as a direct smoothness value if no map is specified.");
        }

        public struct LitProperties
        {
            // Surface Option Props
            public MaterialProperty workflowMode;

            // Surface Input Props
            public MaterialProperty metallic;
            public MaterialProperty specColor;
            public MaterialProperty metallicGlossMap;
            public MaterialProperty specGlossMap;
            public MaterialProperty smoothness;
            public MaterialProperty smoothnessMapChannel;
            public MaterialProperty bumpMapProp;
            public MaterialProperty bumpScaleProp;
            public MaterialProperty parallaxMapProp;
            public MaterialProperty parallaxScaleProp;
            public MaterialProperty occlusionStrength;
            public MaterialProperty occlusionMap;

            //StylizedLit
            public MaterialProperty diss;
            public MaterialProperty dissolveMap;
            public MaterialProperty dissColor;
            public MaterialProperty dissolve;
            public MaterialProperty disType;
            public MaterialProperty invert;
            public MaterialProperty noiseRange;
            public MaterialProperty outlineWidth;

            public MaterialProperty fresnel;
            public MaterialProperty fresnelColor;
            public MaterialProperty fresnelPower;
            public MaterialProperty fresnelStep;
            public MaterialProperty fresnelMask;
            public MaterialProperty fresnelSlider;
            public MaterialProperty fresnelFeather;
            public MaterialProperty fresnelMove;
            public MaterialProperty fresnelNoise;
            public MaterialProperty fresnelSpeed;

            public MaterialProperty useFlip;
            public MaterialProperty albedoFlip;
            public MaterialProperty emissionFlip;
            public MaterialProperty col;
            public MaterialProperty row;
            public MaterialProperty flip;

            public MaterialProperty changeMap;
            public MaterialProperty changeType;
            public MaterialProperty newAlbedo;
            public MaterialProperty newEmission;
            public MaterialProperty newTexMask;
            public MaterialProperty change;

            public MaterialProperty matcap;
            public MaterialProperty matcaptex;
            public MaterialProperty cube;
            public MaterialProperty cubemap;
            public MaterialProperty cubeIntensity;
            public MaterialProperty cubeMask;
            public MaterialProperty maskType;

            public MaterialProperty changeTex;
            public MaterialProperty nowTexID;
            public MaterialProperty nextTexID;
            public MaterialProperty lerpSlider;
            public MaterialProperty texArray;

            public MaterialProperty eMask;
            public MaterialProperty emissionMask;

            public MaterialProperty height;
            public MaterialProperty heightA;
            public MaterialProperty heightB;
            public MaterialProperty heightNormal;
            public MaterialProperty vertexHeight;
            public MaterialProperty heightSlider;
            
            // Advanced Props
            public MaterialProperty highlights;
            public MaterialProperty reflections;

            public MaterialProperty clearCoat;  // Enable/Disable dummy property
            public MaterialProperty clearCoatMap;
            public MaterialProperty clearCoatMask;
            public MaterialProperty clearCoatSmoothness;

            public LitProperties(MaterialProperty[] properties)
            {
                // Surface Option Props
                workflowMode = BaseShaderGUI.FindProperty("_WorkflowMode", properties, false);
                // Surface Input Props
                metallic = BaseShaderGUI.FindProperty("_Metallic", properties);
                specColor = BaseShaderGUI.FindProperty("_SpecColor", properties, false);
                metallicGlossMap = BaseShaderGUI.FindProperty("_MetallicGlossMap", properties);
                specGlossMap = BaseShaderGUI.FindProperty("_SpecGlossMap", properties, false);
                smoothness = BaseShaderGUI.FindProperty("_Smoothness", properties, false);
                smoothnessMapChannel = BaseShaderGUI.FindProperty("_SmoothnessTextureChannel", properties, false);
                bumpMapProp = BaseShaderGUI.FindProperty("_BumpMap", properties, false);
                bumpScaleProp = BaseShaderGUI.FindProperty("_BumpScale", properties, false);
                parallaxMapProp = BaseShaderGUI.FindProperty("_ParallaxMap", properties, false);
                parallaxScaleProp = BaseShaderGUI.FindProperty("_Parallax", properties, false);
                occlusionStrength = BaseShaderGUI.FindProperty("_OcclusionStrength", properties, false);
                occlusionMap = BaseShaderGUI.FindProperty("_OcclusionMap", properties, false);
                // Advanced Props
                highlights = BaseShaderGUI.FindProperty("_SpecularHighlights", properties, false);
                reflections = BaseShaderGUI.FindProperty("_EnvironmentReflections", properties, false);

                clearCoat = BaseShaderGUI.FindProperty("_ClearCoat", properties, false);
                clearCoatMap = BaseShaderGUI.FindProperty("_ClearCoatMap", properties, false);
                clearCoatMask = BaseShaderGUI.FindProperty("_ClearCoatMask", properties, false);
                clearCoatSmoothness = BaseShaderGUI.FindProperty("_ClearCoatSmoothness", properties, false);

                //stylized Lit
                diss = BaseShaderGUI.FindProperty("_DISS", properties, false);
                dissolveMap = BaseShaderGUI.FindProperty("_DissolveMap", properties, false);
                dissColor = BaseShaderGUI.FindProperty("_DissolveColor", properties, false);
                dissolve = BaseShaderGUI.FindProperty("_Dissolve", properties, false);
                disType = BaseShaderGUI.FindProperty("_DISTYPE", properties, false);
                invert = BaseShaderGUI.FindProperty("_INVERT", properties, false);
                noiseRange = BaseShaderGUI.FindProperty("_NoiseRange", properties, false);
                outlineWidth = BaseShaderGUI.FindProperty("_OutlineWidth", properties, false);

                fresnel = BaseShaderGUI.FindProperty("_FRES", properties, false);
                fresnelColor = BaseShaderGUI.FindProperty("_FresnelColor", properties, false);
                fresnelPower = BaseShaderGUI.FindProperty("_Power", properties, false);
                fresnelStep = BaseShaderGUI.FindProperty("_FSTEP", properties, false);
                fresnelMask = BaseShaderGUI.FindProperty("_FresnelMask", properties, false);
                fresnelSlider = BaseShaderGUI.FindProperty("_FresnelSlider", properties, false);
                fresnelFeather = BaseShaderGUI.FindProperty("_FresnelFeather", properties, false);
                fresnelMove = BaseShaderGUI.FindProperty("_FMOVE", properties, false);
                fresnelNoise = BaseShaderGUI.FindProperty("_FresnelNoise", properties, false);
                fresnelSpeed = BaseShaderGUI.FindProperty("_FresnelSpeed", properties, false);

                useFlip = BaseShaderGUI.FindProperty("_UseFlip", properties, false);
                albedoFlip = BaseShaderGUI.FindProperty("_AFlip", properties, false);
                emissionFlip = BaseShaderGUI.FindProperty("_EFlip", properties, false);
                col = BaseShaderGUI.FindProperty("_Col", properties, false);
                row = BaseShaderGUI.FindProperty("_Row", properties, false);
                flip = BaseShaderGUI.FindProperty("_Flip", properties, false);

                changeMap = BaseShaderGUI.FindProperty("_CHANGEMAP", properties, false);
                changeType = BaseShaderGUI.FindProperty("_ChangeType", properties, false);
                newAlbedo = BaseShaderGUI.FindProperty("_NewAlbedo", properties, false);
                newEmission = BaseShaderGUI.FindProperty("_NewEmission", properties, false);
                newTexMask = BaseShaderGUI.FindProperty("_ChangeMask", properties, false);
                change = BaseShaderGUI.FindProperty("_Change", properties, false);

                matcap = BaseShaderGUI.FindProperty("_MatCap", properties, false);
                matcaptex = BaseShaderGUI.FindProperty("_MatCapTex", properties, false);
                cube = BaseShaderGUI.FindProperty("_Cube", properties, false);
                cubemap = BaseShaderGUI.FindProperty("_CubeMap", properties, false);
                cubeIntensity = BaseShaderGUI.FindProperty("_CubeIntensity", properties, false);
                cubeMask = BaseShaderGUI.FindProperty("_CubeMask", properties, false);
                maskType = BaseShaderGUI.FindProperty("_MASKTYPE", properties, false);

                changeTex = BaseShaderGUI.FindProperty("_CHANGETEX", properties, false);
                nowTexID = BaseShaderGUI.FindProperty("_NowTexID", properties, false);
                nextTexID = BaseShaderGUI.FindProperty("_NextTexID", properties, false);
                lerpSlider = BaseShaderGUI.FindProperty("_LerpSlider", properties, false);
                texArray = BaseShaderGUI.FindProperty("_TexArray", properties, false);

                eMask = BaseShaderGUI.FindProperty("_EMASK", properties, false);
                emissionMask = BaseShaderGUI.FindProperty("_EmissionMask", properties, false);
                
                height = BaseShaderGUI.FindProperty("_HEIGHT", properties, false);
                heightA =  BaseShaderGUI.FindProperty("_HeightA", properties, false);
                heightB =  BaseShaderGUI.FindProperty("_HeightB", properties, false);
                heightNormal = BaseShaderGUI.FindProperty("_HeightNormal", properties, false);
                vertexHeight = BaseShaderGUI.FindProperty("_VertexHeight", properties, false);
                heightSlider =  BaseShaderGUI.FindProperty("_HeightSlider", properties, false);
            }
        }

        public static void Inputs(LitProperties properties, MaterialEditor materialEditor, Material material)
        {
            DoMetallicSpecularArea(properties, materialEditor, material);
            BaseShaderGUI.DrawNormalArea(materialEditor, properties.bumpMapProp, properties.bumpScaleProp);

            if (HeightmapAvailable(material))
                DoHeightmapArea(properties, materialEditor);

            if (properties.occlusionMap != null)
            {
                materialEditor.TexturePropertySingleLine(Styles.occlusionText, properties.occlusionMap,
                    properties.occlusionMap.textureValue != null ? properties.occlusionStrength : null);
            }

            // Check that we have all the required properties for clear coat,
            // otherwise we will get null ref exception from MaterialEditor GUI helpers.
            if (ClearCoatAvailable(material))
                DoClearCoat(properties, materialEditor, material);
        }

        private static bool ClearCoatAvailable(Material material)
        {
            return material.HasProperty("_ClearCoat")
                && material.HasProperty("_ClearCoatMap")
                && material.HasProperty("_ClearCoatMask")
                && material.HasProperty("_ClearCoatSmoothness");
        }

        private static bool HeightmapAvailable(Material material)
        {
            return material.HasProperty("_Parallax")
                && material.HasProperty("_ParallaxMap");
        }

        private static void DoHeightmapArea(LitProperties properties, MaterialEditor materialEditor)
        {
            materialEditor.TexturePropertySingleLine(Styles.heightMapText, properties.parallaxMapProp,
                properties.parallaxMapProp.textureValue != null ? properties.parallaxScaleProp : null);
        }

        private static bool ClearCoatEnabled(Material material)
        {
            return material.HasProperty("_ClearCoat") && material.GetFloat("_ClearCoat") > 0.0;
        }

        public static void DoClearCoat(LitProperties properties, MaterialEditor materialEditor, Material material)
        {
            materialEditor.ShaderProperty(properties.clearCoat, Styles.clearCoatText);
            var coatEnabled = material.GetFloat("_ClearCoat") > 0.0;

            EditorGUI.BeginDisabledGroup(!coatEnabled);
            {
                materialEditor.TexturePropertySingleLine(Styles.clearCoatMaskText, properties.clearCoatMap, properties.clearCoatMask);

                EditorGUI.indentLevel += 2;

                // Texture and HDR color controls
                materialEditor.ShaderProperty(properties.clearCoatSmoothness, Styles.clearCoatSmoothnessText);

                EditorGUI.indentLevel -= 2;
            }
            EditorGUI.EndDisabledGroup();
        }

        public static void DoMetallicSpecularArea(LitProperties properties, MaterialEditor materialEditor, Material material)
        {
            string[] smoothnessChannelNames;
            bool hasGlossMap = false;
            if (properties.workflowMode == null ||
                (WorkflowMode)properties.workflowMode.floatValue == WorkflowMode.Metallic)
            {
                hasGlossMap = properties.metallicGlossMap.textureValue != null;
                smoothnessChannelNames = Styles.metallicSmoothnessChannelNames;
                materialEditor.TexturePropertySingleLine(Styles.metallicMapText, properties.metallicGlossMap,
                    hasGlossMap ? null : properties.metallic);
            }
            else
            {
                hasGlossMap = properties.specGlossMap.textureValue != null;
                smoothnessChannelNames = Styles.specularSmoothnessChannelNames;
                BaseShaderGUI.TextureColorProps(materialEditor, Styles.specularMapText, properties.specGlossMap,
                    hasGlossMap ? null : properties.specColor);
            }
            DoSmoothness(materialEditor, material, properties.smoothness, properties.smoothnessMapChannel, smoothnessChannelNames);
        }

        

        public static void DoSmoothness(MaterialEditor materialEditor, Material material, MaterialProperty smoothness, MaterialProperty smoothnessMapChannel, string[] smoothnessChannelNames)
        {
            EditorGUI.indentLevel += 2;

            materialEditor.ShaderProperty(smoothness, Styles.smoothnessText);

            if (smoothnessMapChannel != null) // smoothness channel
            {
                var opaque = ((BaseShaderGUI.SurfaceType)material.GetFloat("_Surface") ==
                          BaseShaderGUI.SurfaceType.Opaque);
                EditorGUI.indentLevel++;
                EditorGUI.showMixedValue = smoothnessMapChannel.hasMixedValue;
                if (opaque)
                {
                    EditorGUI.BeginChangeCheck();
                    var smoothnessSource = (int)smoothnessMapChannel.floatValue;
                    smoothnessSource = EditorGUILayout.Popup(Styles.smoothnessMapChannelText, smoothnessSource, smoothnessChannelNames);
                    if (EditorGUI.EndChangeCheck())
                        smoothnessMapChannel.floatValue = smoothnessSource;
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.Popup(Styles.smoothnessMapChannelText, 0, smoothnessChannelNames);
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUI.showMixedValue = false;
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel -= 2;
        }

        public static SmoothnessMapChannel GetSmoothnessMapChannel(Material material)
        {
            int ch = (int)material.GetFloat("_SmoothnessTextureChannel");
            if (ch == (int)SmoothnessMapChannel.AlbedoAlpha)
                return SmoothnessMapChannel.AlbedoAlpha;

            return SmoothnessMapChannel.SpecularMetallicAlpha;
        }
        internal static void SetupSpecularWorkflowKeyword(Material material, out bool isSpecularWorkflow)
        {
            isSpecularWorkflow = false;     // default is metallic workflow
            if (material.HasProperty("_WorkflowMode"))
                isSpecularWorkflow = ((WorkflowMode)material.GetFloat("_WorkflowMode")) == WorkflowMode.Specular;
            CoreUtils.SetKeyword(material, "_SPECULAR_SETUP", isSpecularWorkflow);
        }

        public static void SetMaterialKeywords(Material material)
        {
            // Note: keywords must be based on Material value not on MaterialProperty due to multi-edit & material animation
            // (MaterialProperty value might come from renderer material property block)
            SetupSpecularWorkflowKeyword(material, out bool isSpecularWorkFlow);

            var specularGlossMap = isSpecularWorkFlow ? "_SpecGlossMap" : "_MetallicGlossMap";
            var hasGlossMap = material.GetTexture(specularGlossMap) != null;
            var opaque = ((BaseShaderGUI.SurfaceType)material.GetFloat("_Surface") ==
                          BaseShaderGUI.SurfaceType.Opaque);
            CoreUtils.SetKeyword(material, "_METALLICSPECGLOSSMAP", hasGlossMap);

            if (material.HasProperty("_SpecularHighlights"))
                CoreUtils.SetKeyword(material, "_SPECULARHIGHLIGHTS_OFF",
                    material.GetFloat("_SpecularHighlights") == 0.0f);
            if (material.HasProperty("_EnvironmentReflections"))
                CoreUtils.SetKeyword(material, "_ENVIRONMENTREFLECTIONS_OFF",
                    material.GetFloat("_EnvironmentReflections") == 0.0f);
            if (material.HasProperty("_OcclusionMap"))
                CoreUtils.SetKeyword(material, "_OCCLUSIONMAP", material.GetTexture("_OcclusionMap"));

            if (material.HasProperty("_ParallaxMap"))
                CoreUtils.SetKeyword(material, "_PARALLAXMAP", material.GetTexture("_ParallaxMap"));

            if (material.HasProperty("_SmoothnessTextureChannel"))
            {
                
                CoreUtils.SetKeyword(material, "_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A",
                    GetSmoothnessMapChannel(material) == SmoothnessMapChannel.AlbedoAlpha && opaque);
            }

            // Clear coat keywords are independent to remove possiblity of invalid combinations.
            if (ClearCoatEnabled(material))
            {
                var hasMap = material.HasProperty("_ClearCoatMap") && material.GetTexture("_ClearCoatMap") != null;
                if (hasMap)
                {
                    CoreUtils.SetKeyword(material, "_CLEARCOAT", false);
                    CoreUtils.SetKeyword(material, "_CLEARCOATMAP", true);
                }
                else
                {
                    CoreUtils.SetKeyword(material, "_CLEARCOAT", true);
                    CoreUtils.SetKeyword(material, "_CLEARCOATMAP", false);
                }
            }
            else
            {
                CoreUtils.SetKeyword(material, "_CLEARCOAT", false);
                CoreUtils.SetKeyword(material, "_CLEARCOATMAP", false);
            }
            

            //stylizedLit---------------------
            var hasBrushTex = false;
            if (material.HasProperty("_DissolveMap"))
            {
                hasBrushTex = material.GetTexture("_DissolveMap") != null;
            }
            CoreUtils.SetKeyword(material, "_DISS", hasBrushTex);

        }
    }
}
