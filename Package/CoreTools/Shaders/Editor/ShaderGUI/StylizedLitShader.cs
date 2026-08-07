using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class StylizedLitShader : BaseShaderGUI
    {
        // Properties
        private StylizedLitGUI.LitProperties litProperties;
        private bool dissolveFold = true;
        private bool fresnelFold = true;
        private bool flipbookFold = true;
        private bool changeMapFold = true;
        private bool matcapFold = true;
        private bool changeTexFold = true;
        private bool eMaskFold = true;
        protected class StStyles       
        {
            public static readonly GUIContent dissolveTexGUI = new GUIContent ("Dissolve Texture ",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent dissolveColorGUI = new GUIContent("Dissolve Color",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent noiseRangeGUI = new GUIContent("Noise Range",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent dissolveTypeGUI = new GUIContent("Dissolve Type",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent albedoFlipGUI = new GUIContent("Albedo Flip Book",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent emissionFlipGUI = new GUIContent("Emission Flip Book",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent outlineWidthGUI = new GUIContent("Outline Width",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent fresnelMoveGUI = new GUIContent("Fresnel Move",
                "These settings describe the look and feel of the surface itself.");
            
            public static readonly GUIContent fresnelNoiseGUI = new GUIContent("Fresnel Noise",
                "These settings describe the look and feel of the surface itself.");
            
            public static readonly GUIContent fresnelSpeedGUI = new GUIContent("Fresnel Speed",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent changeMapGUI = new GUIContent("Change Map",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent newTexMaskGUI = new GUIContent("Mask",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent dissolveGUI = new GUIContent("Dissolve",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent changeTypeGUI = new GUIContent("Change Type",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent changeGUI = new GUIContent("Change",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent fresnelColorGUI = new GUIContent("Fresnel Color",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent fresnelPowerGUI = new GUIContent("Fresnel Power",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent fresnelStepGUI = new GUIContent("SmoothStep",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent fresnelMaskGUI = new GUIContent("Mask",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent fresnelSliderGUI = new GUIContent("Slider",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent fresnelFeatherGUI = new GUIContent("Feather",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent fresnelGUI = new GUIContent("Fresnel",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent invertGUI = new GUIContent("Invert",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent newAlbedoGUI = new GUIContent("New Albedo",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent newEmissionGUI = new GUIContent("New Emission",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent matCapGUI = new GUIContent("MatCap",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent matCapTexGUI = new GUIContent("MatCap Texture",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent cubeGUI = new GUIContent("Cube",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent cubeMapGUI = new GUIContent("CubeMap",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent cubeIntGUI = new GUIContent("Intensity",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent cubeMaskGUI = new GUIContent("Mask",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent maskTypeGUI = new GUIContent("Mask Type",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent changeTexGUI = new GUIContent("Change",
                "These settings describe the look and feel of the surface itself.");
            public static readonly GUIContent nowTexIDGUI = new GUIContent("Now",
                "These settings describe the look and feel of the surface itself.");
            public static readonly GUIContent nextTexIDGUI = new GUIContent("Next",
                "These settings describe the look and feel of the surface itself.");
            public static readonly GUIContent lerpSliderGUI = new GUIContent("Lerp",
                "These settings describe the look and feel of the surface itself.");
            public static readonly GUIContent texArrayGUI = new GUIContent("Texture Array",
                "These settings describe the look and feel of the surface itself.");

            public static readonly GUIContent emaskGUI = new GUIContent("EmissionUseMask",
                "These settings describe the look and feel of the surface itself.");
            public static readonly GUIContent emissionMaskGUI = new GUIContent("Emission Mask",
                "These settings describe the look and feel of the surface itself.");

            // collect properties from the material properties
        }
            public override void FindProperties(MaterialProperty[] properties)
        {
            base.FindProperties(properties);
            litProperties = new StylizedLitGUI.LitProperties(properties);
        }

        // material changed check
        public override void ValidateMaterial(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");
            SetMaterialKeywords(material, StylizedLitGUI.SetMaterialKeywords);
        }
        
        // material main surface options
        public override void DrawSurfaceOptions(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;

            // Detect any changes to the material
            EditorGUI.BeginChangeCheck();
            if (litProperties.workflowMode != null)
            {
                DoPopup(StylizedLitGUI.Styles.workflowModeText, litProperties.workflowMode, Enum.GetNames(typeof(StylizedLitGUI.WorkflowMode)));
            }
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var obj in blendModeProp.targets)
                    ValidateMaterial((Material)obj);
            }
            base.DrawSurfaceOptions(material);
        }
        private bool DrawClickableHelpBox(string Name, bool isFold)
        {
            EditorGUILayout.Space();

            // 构建帮助文本
            string helpText = Name;

            // 绘制帮助框
            Rect boxRect = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(helpText, EditorStyles.wordWrappedLabel);
            EditorGUILayout.EndVertical();

            if (Event.current.type == EventType.MouseDown && boxRect.Contains(Event.current.mousePosition))
            {
                return !isFold;
            }
            else
            {
                return isFold;
            }
        }
        public void DrawStylizedInputs(Material material)
        {
            
            if (litProperties.dissolveMap != null ) // Draw the baseMap, most shader will have at least a baseMap
            {
                dissolveFold = DrawClickableHelpBox("Dissolve", dissolveFold);
                if (dissolveFold == false)
                {
                    EditorGUILayout.Space();
                    materialEditor.TexturePropertySingleLine(StStyles.dissolveTexGUI, litProperties.dissolveMap);
                    // TODO Temporary fix for lightmapping, to be replaced with attribute tag.
                    if (material.HasProperty("_DissolveMap"))
                    {
                        material.SetTexture("_DissolveMap", litProperties.dissolveMap.textureValue);

                        var dissolveMapTiling = litProperties.dissolveMap.textureScaleAndOffset;
                        material.SetTextureScale("_DissolveMap", new Vector2(dissolveMapTiling.x, dissolveMapTiling.y));
                        material.SetTextureOffset("_DissolveMap", new Vector2(dissolveMapTiling.z, dissolveMapTiling.w));
                    }
                    if (material.GetTexture("_DissolveMap") != null)
                    {
                        materialEditor.TextureScaleOffsetProperty(litProperties.dissolveMap);
                        materialEditor.ShaderProperty(litProperties.dissColor, StStyles.dissolveColorGUI, 2);
                        materialEditor.ShaderProperty(litProperties.outlineWidth, StStyles.outlineWidthGUI, 2);
                        materialEditor.ShaderProperty(litProperties.disType, StStyles.dissolveTypeGUI, 2);
                        
                        if(material.GetFloat("_DISTYPE") > 0)
                        {
                            materialEditor.ShaderProperty(litProperties.noiseRange, StStyles.noiseRangeGUI, 2);
                            if (material.GetFloat("_DISTYPE") == 1)
                            {
                                materialEditor.ShaderProperty(litProperties.invert, StStyles.invertGUI, 2);
                            }
                        }
                        else
                        {
                            materialEditor.ShaderProperty(litProperties.dissolve, StStyles.dissolveGUI, 2);
                        }
                    }
                
                }
                EditorGUILayout.Space();
            }

            if (material.HasProperty("_FresnelColor"))
            {
                fresnelFold = DrawClickableHelpBox("Fresnel", fresnelFold);
                if (fresnelFold == false)
                {
                    materialEditor.ShaderProperty(litProperties.fresnel, StStyles.fresnelGUI, 0);
                    if (material.GetFloat("_FRES") > 0)
                    {
                        materialEditor.ShaderProperty(litProperties.fresnelColor, StStyles.fresnelColorGUI, 2);
                        materialEditor.ShaderProperty(litProperties.fresnelPower, StStyles.fresnelPowerGUI, 2);
                        materialEditor.ShaderProperty(litProperties.fresnelStep, StStyles.fresnelStepGUI, 2);
                        if (material.GetFloat("_FSTEP") > 0)
                        {
                            EditorGUI.indentLevel += 3;
                            materialEditor.TexturePropertySingleLine(StStyles.fresnelMaskGUI, litProperties.fresnelMask);
                            material.SetTexture("_FresnelMask", litProperties.fresnelMask.textureValue);
                            EditorGUI.indentLevel -= 3;
                            //materialEditor.ShaderProperty(litProperties.fresnelMask, StStyles.fresnelMaskGUI, 3);
                            materialEditor.ShaderProperty(litProperties.fresnelSlider, StStyles.fresnelSliderGUI, 3);
                            materialEditor.ShaderProperty(litProperties.fresnelFeather, StStyles.fresnelFeatherGUI, 3);
                        }
                        materialEditor.ShaderProperty(litProperties.fresnelMove, StStyles.fresnelMoveGUI, 2);
                        if (material.GetFloat("_FMOVE") > 0)
                        {
                            EditorGUI.indentLevel += 3;
                            materialEditor.TexturePropertySingleLine(StStyles.fresnelNoiseGUI, litProperties.fresnelNoise);
                            material.SetTexture("_FresnelNoise", litProperties.fresnelNoise.textureValue);
                            EditorGUI.indentLevel -= 3;
                            materialEditor.ShaderProperty(litProperties.fresnelSpeed, StStyles.fresnelSpeedGUI, 3);
                        }
                    }
                    else
                    {
                        material.SetColor("_FresnelColor", Color.black);
                    }
                }
                EditorGUILayout.Space();
            }

            if (material.HasProperty("_UseFlip"))
            {
                flipbookFold = DrawClickableHelpBox("Flip Book", flipbookFold);
                if (flipbookFold == false)
                {
                    EditorGUILayout.BeginHorizontal();
                    materialEditor.ShaderProperty(litProperties.albedoFlip, StStyles.albedoFlipGUI, 0);
                    materialEditor.ShaderProperty(litProperties.emissionFlip, StStyles.emissionFlipGUI, 0);
                    EditorGUILayout.EndHorizontal();
                    if (material.GetFloat("_AFlip") + material.GetFloat("_EFlip") > 0)
                    {
                        material.SetFloat("_UseFlip", 1);
                        EditorGUI.indentLevel += 2;
                        var flipX = material.GetFloat("_Col");
                        var flipY = material.GetFloat("_Row");
                        var book = EditorGUILayout.Vector2Field("FlipBook", new Vector2(flipX, flipY));
                        material.SetFloat("_Col", book.x);
                        material.SetFloat("_Row", book.y);
                        var totol = material.GetFloat("_Col") * material.GetFloat("_Row");
                        var f = (int)material.GetFloat("_Flip");
                        if (totol > 0)
                        {
                            material.SetFloat("_Flip", EditorGUILayout.IntSlider("Flip", f, 0, (int)totol - 1));

                        }
                        EditorGUI.indentLevel -= 2;
                    }
                }
                EditorGUILayout.Space();
            }

            if (material.HasProperty("_CHANGEMAP"))
            {
                changeMapFold = DrawClickableHelpBox("Change Map", changeMapFold);
                if (changeMapFold == false)
                {
                    materialEditor.ShaderProperty(litProperties.changeMap, StStyles.changeMapGUI, 0);
                    if (material.GetFloat("_CHANGEMAP") > 0)
                    {
                        materialEditor.TexturePropertySingleLine(StStyles.newAlbedoGUI, litProperties.newAlbedo);
                        material.SetTexture("_NewAlbedo", litProperties.newAlbedo.textureValue);
                        materialEditor.TexturePropertySingleLine(StStyles.newEmissionGUI, litProperties.newEmission);
                        material.SetTexture("_NewEmission", litProperties.newEmission.textureValue);

                        materialEditor.ShaderProperty(litProperties.changeType, StStyles.changeTypeGUI, 2);
                        EditorGUI.indentLevel += 2;
                        if (material.GetFloat("_ChangeType") == 0)
                        {
                            materialEditor.TexturePropertySingleLine(StStyles.newTexMaskGUI, litProperties.newTexMask);
                            var MaskTiling = litProperties.newTexMask.textureScaleAndOffset;
                            material.SetTextureScale("_ChangeMask", new Vector2(MaskTiling.x, MaskTiling.y));
                            material.SetTextureOffset("_ChangeMask", new Vector2(MaskTiling.z, MaskTiling.w));
                            materialEditor.TextureScaleOffsetProperty(litProperties.newTexMask);
                        }
                        EditorGUI.indentLevel -= 2;
                        materialEditor.ShaderProperty(litProperties.change, StStyles.changeGUI, 2);
                    }
                }
                EditorGUILayout.Space();
            }

            if (material.HasProperty("_MatCap"))
            {
                matcapFold = DrawClickableHelpBox("MatCap", matcapFold);
                if (matcapFold == false)
                {
                    materialEditor.ShaderProperty(litProperties.matcap, StStyles.matCapGUI, 0);
                    if (material.GetFloat("_MatCap") > 0)
                    {
                        materialEditor.TexturePropertySingleLine(StStyles.matCapTexGUI, litProperties.matcaptex);
                        //material.SetTexture("_MatCapTex", litProperties.matcaptex.textureValue);
                        EditorGUI.indentLevel += 1;
                        materialEditor.TexturePropertySingleLine(StStyles.cubeMaskGUI, litProperties.cubeMask);
                        if (material.GetTexture("_CubeMask") != null)
                        {
                            materialEditor.ShaderProperty(litProperties.maskType, StStyles.maskTypeGUI, 1);
                        }
                        materialEditor.ShaderProperty(litProperties.cube, StStyles.cubeGUI, 1);
                        EditorGUI.indentLevel += 1;
                        if (material.GetFloat("_Cube") > 0)
                        {
                            materialEditor.TexturePropertySingleLine(StStyles.cubeMapGUI, litProperties.cubemap);
                            materialEditor.ShaderProperty(litProperties.cubeIntensity, StStyles.cubeIntGUI, 2);
                        }
                        EditorGUI.indentLevel -= 2;
                    }
                }
                EditorGUILayout.Space();
            }
            if (material.HasProperty("_CHANGETEX"))
            {
                changeTexFold = DrawClickableHelpBox("ChangeTexture", changeTexFold);
                if (changeTexFold == false)
                {
                    materialEditor.ShaderProperty(litProperties.changeTex, StStyles.changeTexGUI, 0);
                    EditorGUI.indentLevel += 1;
                    if (material.GetFloat("_CHANGETEX") > 0)
                    {
                        materialEditor.ShaderProperty(litProperties.nowTexID, StStyles.nowTexIDGUI);
                        materialEditor.ShaderProperty(litProperties.nextTexID, StStyles.nextTexIDGUI);
                        materialEditor.ShaderProperty(litProperties.lerpSlider, StStyles.lerpSliderGUI);

                        materialEditor.TexturePropertySingleLine(StStyles.texArrayGUI, litProperties.texArray);
                    }
                    EditorGUI.indentLevel -= 1;
                }
                EditorGUILayout.Space();
            }

            if (material.HasProperty("_EMASK"))
            {
                eMaskFold = DrawClickableHelpBox("EmissionMask", eMaskFold);
                if (eMaskFold == false)
                {
                    materialEditor.ShaderProperty(litProperties.eMask, StStyles.emaskGUI, 0);
                    EditorGUI.indentLevel += 1;
                    
                    if (material.GetFloat("_EMASK") > 0)
                    {
                        materialEditor.TexturePropertySingleLine(StStyles.emissionMaskGUI, litProperties.emissionMask);
                        material.SetTexture("_EmissionMask", litProperties.emissionMask.textureValue);
                        var emissionMaskTiling = litProperties.emissionMask.textureScaleAndOffset;
                        material.SetTextureScale("_EmissionMask", new Vector2(emissionMaskTiling.x, emissionMaskTiling.y));
                        material.SetTextureOffset("_EmissionMask", new Vector2(emissionMaskTiling.z, emissionMaskTiling.w));
                        materialEditor.TextureScaleOffsetProperty(litProperties.emissionMask);
                    }
                    EditorGUI.indentLevel -= 1;
                }
                EditorGUILayout.Space();
            }

            ValidateMaterial(material);
        }
        private void DrawEmissionTextureProperty()
        {
            if ((emissionMapProp == null) || (emissionColorProp == null))
                return;

            using (new EditorGUI.IndentLevelScope(2))
            {
                materialEditor.TexturePropertyWithHDRColor(Styles.emissionMap, emissionMapProp, emissionColorProp, false);
            }
        }
        new void DrawEmissionProperties(Material material, bool keyword)
        {
            var emissive = true;

            if (!keyword)
            {
                DrawEmissionTextureProperty();
            }
            else
            {
                emissive = materialEditor.EmissionEnabledProperty();
                using (new EditorGUI.DisabledScope(!emissive))
                {
                    DrawEmissionTextureProperty();
                }
            }

            // If texture was assigned and color was black set color to white
            if ((emissionMapProp != null) && (emissionColorProp != null))
            {
                var hadEmissionTexture = emissionMapProp?.textureValue != null;
                var brightness = emissionColorProp.colorValue.maxColorComponent;
                if (emissionMapProp.textureValue != null && !hadEmissionTexture && brightness <= 0f)
                    emissionColorProp.colorValue = Color.white;
            }

            if (emissive)
            {
                CoreUtils.SetKeyword(material, "_EMISSION", emissive);
                // Change the GI emission flag and fix it up with emissive as black if necessary.
                materialEditor.LightmapEmissionFlagsProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel, true);
            }
            else
            {
                CoreUtils.SetKeyword(material, "_EMISSION", emissive);
                material.SetColor("_EmissionColor", Color.black);
            }
        }
        // material main surface inputs
        public override void DrawSurfaceInputs(Material material)
        {
            base.DrawSurfaceInputs(material);
            StylizedLitGUI.Inputs(litProperties, materialEditor, material);
           
            DrawEmissionProperties(material, true);
            DrawTileOffset(materialEditor, baseMapProp);
        }

        // material main advanced options
        public override void DrawAdvancedOptions(Material material)
        {

            //Stylized Lit
            EditorGUILayout.Space();
            DrawStylizedInputs(material);
            EditorGUILayout.Space();

            DrawClickableHelpBox("Other Option", true);
            //EditorGUILayout.HelpBox("Other Option", MessageType.None);
            if (litProperties.reflections != null && litProperties.highlights != null)
            {
                EditorGUI.BeginChangeCheck();
                materialEditor.ShaderProperty(litProperties.highlights, StylizedLitGUI.Styles.highlightsText);
                materialEditor.ShaderProperty(litProperties.reflections, StylizedLitGUI.Styles.reflectionsText);
                if(EditorGUI.EndChangeCheck())
                {
                    ValidateMaterial(material);
                }
            }
            
            base.DrawAdvancedOptions(material);
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }

            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
            {
                SetupMaterialBlendMode(material);
                return;
            }

            SurfaceType surfaceType = SurfaceType.Opaque;
            BlendMode blendMode = BlendMode.Alpha;
            if (oldShader.name.Contains("/Transparent/Cutout/"))
            {
                surfaceType = SurfaceType.Opaque;
                material.SetFloat("_AlphaClip", 1);
            }
            else if (oldShader.name.Contains("/Transparent/"))
            {
                // NOTE: legacy shaders did not provide physically based transparency
                // therefore Fade mode
                surfaceType = SurfaceType.Transparent;
                blendMode = BlendMode.Alpha;
            }
            material.SetFloat("_Surface", (float)surfaceType);
            material.SetFloat("_Blend", (float)blendMode);
            if (surfaceType == SurfaceType.Opaque)
            {
                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
            else
            {
                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
            if (oldShader.name.Equals("Standard (Specular setup)"))
            {
                material.SetFloat("_WorkflowMode", (float)StylizedLitGUI.WorkflowMode.Specular);
                Texture texture = material.GetTexture("_SpecGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }
            else
            {
                material.SetFloat("_WorkflowMode", (float)StylizedLitGUI.WorkflowMode.Metallic);
                Texture texture = material.GetTexture("_MetallicGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }
            ValidateMaterial(material);
        }
    }
}
