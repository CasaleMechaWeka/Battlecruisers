using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game2DWaterKit
{
    public class Game2DWaterShaderGUI : ShaderGUI
    {
        #region Custom Types

        enum BlendMode
        {
            Opaque = 2000,
            Transparent = 3000
        }

        enum NoiseTextureImageChannel
        {
            //red color channel
            Refractive = 0,
            //blue color channel
            Reflective = 1,
            //green color channel
            Surface = 2,
            //alpha color channel
            Water = 3
        }

        enum NoiseTextureDataTypePrecision
        {
            //FIXED (8 bits per channel)
            Low,
            //HALF (16 bits per channel)
            Medium,
            //FLOAT (32 bits per channel)
            High
        }

#if UNITY_2017_1_OR_NEWER
        enum NoiseTextureWrapMode
        {
            Repeat,
            //Mirror Texture Wrap Mode is only supported in Unity 2017 and newer
            Mirror
        }
#endif

        enum WaterColorMode
        {
            SolidColor = 0,
            GradientColor = 1
        }

        private static class Styles
        {
            public static readonly GUIContent NoiseSpeedLabel = new GUIContent("Speed", "Sets the noise texture scroll speed.");
            public static readonly GUIContent NoiseStrengthLabel = new GUIContent("Strength", "Sets the strength of the distortion.");
            public static readonly GUIContent NoiseScaleLabel = new GUIContent("Scale", "Sets the noise scale in x and y axes.");
            public static readonly GUIContent NoiseOffsetLabel = new GUIContent("Offset", "Sets the noise offset in x and y axes.");
            public static readonly GUIContent OpacityLabel = new GUIContent("Opacity", "Adjusts the transparency of the texture image.");
            public static readonly GUIContent wrapModeLabel = new GUIContent("Wrap", "Sets the texure image's wrap mode.");
            public static readonly GUIContent VisibilityLabel = new GUIContent("Visibility", "Controls the visibility of the reflection when both reflection and refraction are enabled.");
            public static readonly GUIContent NoiseTextureSizeLabel = new GUIContent("Size", "Sets the noise texture size.");
            public static readonly GUIContent NoiseTextureFilterMode = new GUIContent("Filter Mode", "Sets the noise texture filter mode to Point, Bilinear or Trilinear.");
            public static readonly GUIContent NoiseTexturePrecision = new GUIContent("Precision", "Sets the noise texture data type precision.");
#if UNITY_2017_1_OR_NEWER
            public static readonly GUIContent NoiseTextureWrapMode = new GUIContent("Wrap Mode", "Sets the noise texture wrap mode to either Repeat or Mirror.");
#endif
            public static readonly GUIContent RefractionAmountOfBending = new GUIContent("Amount Of Bending", "Controls how much the portion of the GameObject above the water is shifted relative to the image viewed under the water.");
            public static readonly GUIContent SurfaceThicknessLabel = new GUIContent("Thickness", "Sets the water's surface line thickness.");
            public static readonly GUIContent NoiseTextureSettings = new GUIContent("Noise Texture Settings");

            public static readonly string RenderingModeLabel = "Rendering Mode";
            public static readonly string TextureLabel = "Texture";
            public static readonly string ColorLabel = "Color";
            public static readonly string ColorModeLabel = "Color Mode";
            public static readonly string ColorGradientStartLabel = "Start Color";
            public static readonly string ColorGradientEndLabel = "End Color";
            public static readonly string PreviewLabel = "Preview";
            public static readonly string NoiseLabel = "Noise";
            public static readonly string BodyFoldoutLabel = "Body Properties";
            public static readonly string SurfaceFoldoutLabel = "Surface Properties";
            public static readonly string SurfaceToggleLabel = "Use Surface";
            public static readonly string ReflectionFoldoutLabel = "Reflecion Properties";
            public static readonly string ReflectionToggleLabel = "Use Reflection";
            public static readonly string RefractionFoldoutLabel = "Refraction Properties";
            public static readonly string RefractionToggleLabel = "Use Refraction";

            public static readonly string DifferentRenderModeRefractionReflectionMessage = "Refraction and Reflection properties are hidden because selected materials have different rendering modes.";
            public static readonly string TransparentRenderModeRefractionReflectionMessage = "Refraction and Reflection properties are hidden because the rendering mode is set to transparent";

            public static readonly string WaterTextureKeyword = "Water2D_WaterTexture";
            public static readonly string WaterNoiseKeyword = "Water2D_WaterNoise";
            public static readonly string ColorGradientKeyword = "Water2D_ColorGradient";

            public static readonly string SurfaceKeyword = "Water2D_Surface";
            public static readonly string SurfaceTextureKeyword = "Water2D_SurfaceTexture";
            public static readonly string SurfaceNoiseKeyword = "Water2D_SurfaceNoise";

            public static readonly string ReflectiveKeyword = "Water2D_Reflection";
            public static readonly string RefractiveKeyword = "Water2D_Refraction";
        }

        #endregion

        #region Material Properties

        //Water properties
        MaterialProperty waterColor = null;
        MaterialProperty waterColorGradientStart = null;
        MaterialProperty waterColorGradientEnd = null;
        MaterialProperty waterTexture = null;
        MaterialProperty waterTextureOpacity = null;
        MaterialProperty waterNoiseSpeed = null;
        MaterialProperty waterNoiseScaleOffset = null;
        MaterialProperty waterNoiseStrength = null;

        //Surface properties
        MaterialProperty surfaceLevel = null;
        MaterialProperty surfaceColor = null;
        MaterialProperty surfaceTexture = null;
        MaterialProperty surfaceTextureOpacity = null;
        MaterialProperty surfaceNoiseSpeed = null;
        MaterialProperty surfaceNoiseScaleOffset = null;
        MaterialProperty surfaceNoiseStrength = null;

        //refraction properties
        MaterialProperty refractionAmountOfBending = null;
        MaterialProperty refractionNoiseSpeed = null;
        MaterialProperty refractionNoiseScaleOffset = null;
        MaterialProperty refractionNoiseStrength = null;

        //reflection properties
        MaterialProperty reflectionNoiseSpeed = null;
        MaterialProperty reflectionNoiseScaleOffset = null;
        MaterialProperty reflectionNoiseStrength = null;
        MaterialProperty reflectionVisibility = null;

        /*Noise Texture:
		 *water(alpha color channel)
		 *surface(blue color channel)
		 *reflection(green color channel)
		 *refraction(red color channel)
		*/
        MaterialProperty noiseTexture = null;

        // Blending state
        MaterialProperty blendMode = null;

        // Some of the water shader keywords state
        MaterialProperty refractionKeywordState = null;
        MaterialProperty reflectionKeywordState = null;
        MaterialProperty waterNoiseKeywordState = null;
        MaterialProperty surfaceKeywordState = null;
        MaterialProperty surfaceNoiseKeywordState = null;
        MaterialProperty waterColorGradientKeywordState = null;

        #endregion

        #region Variables

        MaterialEditor materialEditor;
        bool firstTimeApply = true;
        bool undoPerformed = false;

        /*Array of noise texture previews
		 * index 0 : Refraction Noise Preview Texture
		 * index 1 : Reflection Noise Preview Texture
		 * index 2 : Surface Noise Preview Texture
		 * index 3 : Water Noise Preview Texture
		*/
        Texture2D[] noiseTexturePreviews = new Texture2D[4];
        string[] noiseTextureSizes = {
            "32"
            , "64"
            , "128"
            , "256"
            , "512"
			//, "1024"
			//, "2048"
			//, "4096"
			//, "8192"
		};

        AnimBool showWaterNoiseArea;
        AnimBool showBodyArea;
        AnimBool showSurfaceArea;
        AnimBool showSurfaceNoiseArea;
        AnimBool showRefractionArea;
        AnimBool showReflectionArea;
        AnimBool showNoiseTextureSettingsArea;

        #endregion

        #region Methods

        void FindProperties(MaterialProperty[] properties)
        {
            //finding water properties
            waterColor = FindProperty("_WaterColor", properties);
            waterColorGradientStart = FindProperty("_WaterColorGradientStart", properties);
            waterColorGradientEnd = FindProperty("_WaterColorGradientEnd", properties);
            waterTexture = FindProperty("_WaterTexture", properties);
            waterTextureOpacity = FindProperty("_WaterTextureOpacity", properties);
            waterNoiseSpeed = FindProperty("_WaterNoiseSpeed", properties);
            waterNoiseScaleOffset = FindProperty("_WaterNoiseScaleOffset", properties);
            waterNoiseStrength = FindProperty("_WaterNoiseStrength", properties);

            //finding surface properties
            surfaceLevel = FindProperty("_SurfaceLevel", properties);
            surfaceColor = FindProperty("_SurfaceColor", properties);
            surfaceTexture = FindProperty("_SurfaceTexture", properties);
            surfaceTextureOpacity = FindProperty("_SurfaceTextureOpacity", properties);
            surfaceNoiseSpeed = FindProperty("_SurfaceNoiseSpeed", properties);
            surfaceNoiseScaleOffset = FindProperty("_SurfaceNoiseScaleOffset", properties);
            surfaceNoiseStrength = FindProperty("_SurfaceNoiseStrength", properties);

            //finding refraction properties
            refractionAmountOfBending = FindProperty("_RefractionAmountOfBending", properties);
            refractionNoiseSpeed = FindProperty("_RefractionNoiseSpeed", properties);
            refractionNoiseScaleOffset = FindProperty("_RefractionNoiseScaleOffset", properties);
            refractionNoiseStrength = FindProperty("_RefractionNoiseStrength", properties);

            //finding reflection properties
            reflectionVisibility = FindProperty("_ReflectionVisibility", properties);
            reflectionNoiseSpeed = FindProperty("_ReflectionNoiseSpeed", properties);
            reflectionNoiseScaleOffset = FindProperty("_ReflectionNoiseScaleOffset", properties);
            reflectionNoiseStrength = FindProperty("_ReflectionNoiseStrength", properties);

            //finding other properties
            noiseTexture = FindProperty("_NoiseTexture", properties);
            blendMode = FindProperty("_Mode", properties);

            refractionKeywordState = FindProperty("_Water2D_IsRefractionEnabled", properties);
            reflectionKeywordState = FindProperty("_Water2D_IsReflectionEnabled", properties);
            waterNoiseKeywordState = FindProperty("_Water2D_IsWaterNoiseEnabled", properties);
            surfaceKeywordState = FindProperty("_Water2D_IsSurfaceEnabled", properties);
            surfaceNoiseKeywordState = FindProperty("_Water2D_IsSurfaceNoiseEnabled", properties);
            waterColorGradientKeywordState = FindProperty("_Water2D_IsColorGradientEnabled", properties);
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            /*
			 * MaterialProperties can be animated so we do not cache them
			 * We fetch them every event to ensure animated values are updated correctly.
			*/
            FindProperties(properties);
            this.materialEditor = materialEditor;
            Material material = materialEditor.target as Material;

            if (firstTimeApply)
            {
                MaterialChanged(material);

                GenerateNoiseTexture();

                Undo.undoRedoPerformed -= UndoRedoPerformed;
                Undo.undoRedoPerformed += UndoRedoPerformed;

                firstTimeApply = false;
            }

            ShaderPropertiesGUI(material);
        }

        void ShaderPropertiesGUI(Material material)
        {
            EditorGUIUtility.labelWidth = 0f;
            EditorGUIUtility.fieldWidth = 65f;

            EditorGUI.BeginChangeCheck();
            {
                DoBlendModePopup();
                DoBodyArea(material);
                DoSurfaceArea();
                if (blendMode.hasMixedValue)
                {
                    EditorGUILayout.HelpBox(Styles.DifferentRenderModeRefractionReflectionMessage, MessageType.Info, true);
                }
                else
                {
                    if (blendMode.floatValue == (float)BlendMode.Transparent)
                    {
                        EditorGUILayout.HelpBox(Styles.TransparentRenderModeRefractionReflectionMessage, MessageType.Info, true);
                    }
                    else
                    {
                        DoRefractionArea();
                        DoReflectionArea();
                    }
                }
                if ((!surfaceNoiseKeywordState.hasMixedValue && material.IsKeywordEnabled(Styles.SurfaceNoiseKeyword))
                     || (!waterNoiseKeywordState.hasMixedValue && material.IsKeywordEnabled(Styles.WaterNoiseKeyword))
                     || (!refractionKeywordState.hasMixedValue && material.IsKeywordEnabled(Styles.RefractiveKeyword))
                     || (!reflectionKeywordState.hasMixedValue && material.IsKeywordEnabled(Styles.ReflectiveKeyword)))
                {
                    DoNoiseTextureSettingsArea(material);
                }
            }

            if (undoPerformed)
            {
                undoPerformed = false;
                GenerateNoiseTexture();
            }

            if (EditorGUI.EndChangeCheck())
            {
                foreach (Material obj in materialEditor.targets)
                {
                    MaterialChanged(obj);
                }
            }

            EditorGUIUtility.fieldWidth = 0f;
        }

        void UndoRedoPerformed()
        {
            undoPerformed = true;
        }

        void DoBodyArea(Material material)
        {
            bool showBodyAreaState = EditorPrefs.GetBool("Water2D_ShaderGUI_BodyPropertiesExpanded", false);
            if (showBodyArea == null)
            {
                showBodyArea = new AnimBool(showBodyAreaState);
                showBodyArea.valueChanged.AddListener(materialEditor.Repaint);
            }

            EditorGUI.BeginChangeCheck();
            showBodyAreaState = EditorGUILayout.Foldout(showBodyAreaState, Styles.BodyFoldoutLabel, true);
            if (EditorGUI.EndChangeCheck())
            {
                showBodyArea.target = showBodyAreaState;
                EditorPrefs.SetBool("Water2D_ShaderGUI_BodyPropertiesExpanded", showBodyAreaState);
            }

            if (EditorGUILayout.BeginFadeGroup(showBodyArea.faded))
            {
                DoWaterColorArea(material);
            }
            EditorGUILayout.EndFadeGroup();
        }

        void DoBlendModePopup()
        {
            EditorGUI.showMixedValue = blendMode.hasMixedValue;
            BlendMode mode = (BlendMode)blendMode.floatValue;
            EditorGUI.BeginChangeCheck();
            mode = (BlendMode)EditorGUILayout.EnumPopup(Styles.RenderingModeLabel, mode);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(Styles.RenderingModeLabel);
                blendMode.floatValue = (float)mode;
            }
            EditorGUI.showMixedValue = false;
        }

        void DoWaterColorArea(Material material)
        {
            WaterColorMode colorMode = (WaterColorMode)waterColorGradientKeywordState.floatValue;

            EditorGUI.showMixedValue = waterColorGradientKeywordState.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            colorMode = (WaterColorMode)EditorGUILayout.EnumPopup(Styles.ColorModeLabel, colorMode);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(Styles.ColorModeLabel);
                waterColorGradientKeywordState.floatValue = (float)colorMode;
            }
            EditorGUI.showMixedValue = false;

            if (!waterColorGradientKeywordState.hasMixedValue)
            {
                if (colorMode == WaterColorMode.GradientColor)
                {
                    materialEditor.ColorProperty(waterColorGradientStart, Styles.ColorGradientStartLabel);
                    materialEditor.ColorProperty(waterColorGradientEnd, Styles.ColorGradientEndLabel);
                }
                else
                {
                    materialEditor.ColorProperty(waterColor, Styles.ColorLabel);
                }
            }

            DrawTexturePropertyWithScaleOffsetOpacity(waterTexture, waterTextureOpacity);

            if (waterTexture.textureValue != null)
            {
                if (showWaterNoiseArea == null)
                {
                    showWaterNoiseArea = new AnimBool(false);
                    showWaterNoiseArea.valueChanged.AddListener(materialEditor.Repaint);
                }

                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = waterNoiseKeywordState.hasMixedValue;
                showWaterNoiseArea.target = EditorGUILayout.ToggleLeft(Styles.NoiseLabel, waterNoiseKeywordState.floatValue == 1.0f);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo(Styles.NoiseLabel);
                    waterNoiseKeywordState.floatValue = showWaterNoiseArea.target ? 1.0f : 0.0f;
                }
                if (EditorGUILayout.BeginFadeGroup(showWaterNoiseArea.faded))
                {
                    EditorGUIUtility.labelWidth = 80f;
                    materialEditor.ShaderProperty(waterNoiseSpeed, Styles.NoiseSpeedLabel);
                    EditorGUIUtility.labelWidth = 0f;
                    DrawNoisePreview(NoiseTextureImageChannel.Water, waterNoiseScaleOffset, waterNoiseStrength);
                }
                EditorGUILayout.EndFadeGroup();
                EditorGUI.indentLevel--;
            }
        }

        void DoSurfaceArea()
        {
            bool showSurfaceAreaState = EditorPrefs.GetBool("Water2D_ShaderGUI_SurfacePropertiesExpanded", false);
            if (showSurfaceArea == null)
            {
                showSurfaceArea = new AnimBool(showSurfaceAreaState);
                showSurfaceArea.valueChanged.AddListener(materialEditor.Repaint);
            }

            EditorGUI.BeginChangeCheck();
            showSurfaceAreaState = EditorGUILayout.Foldout(showSurfaceAreaState, Styles.SurfaceFoldoutLabel, true);
            if (EditorGUI.EndChangeCheck())
            {
                showSurfaceArea.target = showSurfaceAreaState;
                EditorPrefs.SetBool("Water2D_ShaderGUI_SurfacePropertiesExpanded", showSurfaceAreaState);
            }

            if (EditorGUILayout.BeginFadeGroup(showSurfaceArea.faded))
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = surfaceKeywordState.hasMixedValue;
                bool surfaceState = EditorGUILayout.ToggleLeft(Styles.SurfaceToggleLabel, surfaceKeywordState.floatValue == 1.0f);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo(Styles.SurfaceToggleLabel);
                    surfaceKeywordState.floatValue = surfaceState ? 1.0f : 0.0f;
                }

                EditorGUI.BeginDisabledGroup(!surfaceState);

                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = surfaceLevel.hasMixedValue;
                float surfaceThickness = 1f - surfaceLevel.floatValue;
                surfaceThickness = EditorGUILayout.Slider(Styles.SurfaceThicknessLabel, surfaceThickness, 0f, 1f);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo(Styles.SurfaceThicknessLabel.text);
                    surfaceLevel.floatValue = 1f - surfaceThickness;
                }

                materialEditor.ColorProperty(surfaceColor, Styles.ColorLabel);
                DrawTexturePropertyWithScaleOffsetOpacity(surfaceTexture, surfaceTextureOpacity);
                if (surfaceTexture.textureValue != null)
                {
                    if (showSurfaceNoiseArea == null)
                    {
                        showSurfaceNoiseArea = new AnimBool(false);
                        showSurfaceNoiseArea.valueChanged.AddListener(materialEditor.Repaint);
                    }

                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = surfaceNoiseKeywordState.hasMixedValue;
                    showSurfaceNoiseArea.target = EditorGUILayout.ToggleLeft(Styles.NoiseLabel, surfaceNoiseKeywordState.floatValue == 1.0f);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    {
                        materialEditor.RegisterPropertyChangeUndo(Styles.NoiseLabel);
                        surfaceNoiseKeywordState.floatValue = showSurfaceNoiseArea.target ? 1.0f : 0.0f;
                    }
                    if (EditorGUILayout.BeginFadeGroup(showSurfaceNoiseArea.faded))
                    {
                        EditorGUI.indentLevel++;
                        EditorGUIUtility.labelWidth = 95f;
                        materialEditor.ShaderProperty(surfaceNoiseSpeed, Styles.NoiseSpeedLabel);
                        EditorGUIUtility.labelWidth = 0f;
                        DrawNoisePreview(NoiseTextureImageChannel.Surface, surfaceNoiseScaleOffset, surfaceNoiseStrength);
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndFadeGroup();
                }

                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndFadeGroup();
        }

        void DoReflectionArea()
        {
            bool showReflectionAreaState = EditorPrefs.GetBool("Water2D_ShaderGUI_ReflectionPropertiesExpanded", false);
            if (showReflectionArea == null)
            {
                showReflectionArea = new AnimBool(showReflectionAreaState);
                showReflectionArea.valueChanged.AddListener(materialEditor.Repaint);
            }

            EditorGUI.BeginChangeCheck();
            showReflectionAreaState = EditorGUILayout.Foldout(showReflectionAreaState, Styles.ReflectionFoldoutLabel, true);
            if (EditorGUI.EndChangeCheck())
            {
                showReflectionArea.target = showReflectionAreaState;
                EditorPrefs.SetBool("Water2D_ShaderGUI_ReflectionPropertiesExpanded", showReflectionAreaState);
            }

            if (EditorGUILayout.BeginFadeGroup(showReflectionArea.faded))
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = reflectionKeywordState.hasMixedValue;
                bool reflectionState = EditorGUILayout.ToggleLeft(Styles.ReflectionToggleLabel, reflectionKeywordState.floatValue == 1.0f);
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo(Styles.ReflectionToggleLabel);
                    reflectionKeywordState.floatValue = reflectionState ? 1.0f : 0.0f;
                }
                EditorGUI.showMixedValue = false;

                EditorGUI.BeginDisabledGroup(!reflectionState);

                EditorGUIUtility.labelWidth = 80f;
                materialEditor.ShaderProperty(reflectionNoiseSpeed, Styles.NoiseSpeedLabel);
                EditorGUIUtility.labelWidth = 0f;
                if (refractionKeywordState.floatValue == 1.0f)
                {
                    materialEditor.ShaderProperty(reflectionVisibility, Styles.VisibilityLabel);
                }
                DrawNoisePreview(NoiseTextureImageChannel.Reflective, reflectionNoiseScaleOffset, reflectionNoiseStrength);

                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndFadeGroup();
        }

        void DoRefractionArea()
        {
            bool showRefractionAreaState = EditorPrefs.GetBool("Water2D_ShaderGUI_RefractionPropertiesExpanded", false);
            if (showRefractionArea == null)
            {
                showRefractionArea = new AnimBool(showRefractionAreaState);
                showRefractionArea.valueChanged.AddListener(materialEditor.Repaint);
            }

            EditorGUI.BeginChangeCheck();
            showRefractionAreaState = EditorGUILayout.Foldout(showRefractionAreaState, Styles.RefractionFoldoutLabel, true);
            if (EditorGUI.EndChangeCheck())
            {
                showRefractionArea.target = showRefractionAreaState;
                EditorPrefs.SetBool("Water2D_ShaderGUI_RefractionPropertiesExpanded", showRefractionAreaState);
            }

            if (EditorGUILayout.BeginFadeGroup(showRefractionArea.faded))
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = refractionKeywordState.hasMixedValue;
                bool refractionState = EditorGUILayout.ToggleLeft(Styles.RefractionToggleLabel, refractionKeywordState.floatValue == 1.0f);
                if (EditorGUI.EndChangeCheck())
                {
                    materialEditor.RegisterPropertyChangeUndo(Styles.RefractionToggleLabel);
                    refractionKeywordState.floatValue = refractionState ? 1.0f : 0.0f;
                }
                EditorGUI.showMixedValue = false;

                EditorGUI.BeginDisabledGroup(!refractionState);

                EditorGUIUtility.labelWidth = 80f;
                materialEditor.ShaderProperty(refractionNoiseSpeed, Styles.NoiseSpeedLabel);
                EditorGUI.BeginChangeCheck();
                materialEditor.ShaderProperty(refractionAmountOfBending, Styles.RefractionAmountOfBending);
                if (EditorGUI.EndChangeCheck())
                {
                    GenerateNoise(NoiseTextureImageChannel.Refractive, refractionNoiseScaleOffset, refractionNoiseStrength, refractionAmountOfBending);
                }
                EditorGUIUtility.labelWidth = 0f;
                DrawNoisePreview(NoiseTextureImageChannel.Refractive, refractionNoiseScaleOffset, refractionNoiseStrength, refractionAmountOfBending);

                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndFadeGroup();
        }

        void DrawNoisePreview(NoiseTextureImageChannel channel, MaterialProperty scaleOffsetProperty, MaterialProperty strengthProperty, MaterialProperty amountOfBending = null)
        {
            Vector4 scaleOffset = scaleOffsetProperty.vectorValue;
            Vector2 scale = new Vector2(scaleOffset.x, scaleOffset.y);
            Vector2 offset = new Vector2(scaleOffset.z, scaleOffset.w);

            Rect totalRect = EditorGUILayout.GetControlRect(true, MaterialEditor.GetDefaultPropertyHeight(noiseTexture), EditorStyles.layerMaskField);
            Rect previewRect = totalRect;
            previewRect.xMin = previewRect.xMax - EditorGUIUtility.fieldWidth;
            Rect fieldsRect = totalRect;
            fieldsRect.yMin += 16f;
            fieldsRect.xMax -= EditorGUIUtility.fieldWidth + 2f;
            Rect scaleFieldRect = new Rect(fieldsRect.x, fieldsRect.y, fieldsRect.width, 16f);
            Rect offsetFieldRect = new Rect(fieldsRect.x, scaleFieldRect.y + 16f, fieldsRect.width, 16f);
            Rect strengthFieldRect = new Rect(fieldsRect.x, offsetFieldRect.y + 16f, fieldsRect.width, 16f);

            GUI.DrawTexture(previewRect, noiseTexturePreviews[(int)channel], ScaleMode.ScaleToFit);
            EditorGUI.BeginChangeCheck();
            EditorGUIUtility.labelWidth = 65f;
            scaleFieldRect = EditorGUI.PrefixLabel(scaleFieldRect, Styles.NoiseScaleLabel);
            Vector2 newScale = EditorGUI.Vector2Field(scaleFieldRect, GUIContent.none, scale);
            offsetFieldRect = EditorGUI.PrefixLabel(offsetFieldRect, Styles.NoiseOffsetLabel);
            Vector2 newOffset = EditorGUI.Vector2Field(offsetFieldRect, GUIContent.none, offset);
            strengthFieldRect = EditorGUI.PrefixLabel(strengthFieldRect, Styles.NoiseStrengthLabel);
            EditorGUI.showMixedValue = strengthProperty.hasMixedValue;
            float strength = EditorGUI.Slider(strengthFieldRect, strengthProperty.floatValue, strengthProperty.rangeLimits.x, strengthProperty.rangeLimits.y);
            EditorGUI.showMixedValue = false;
            EditorGUIUtility.labelWidth = 0f;
            if (EditorGUI.EndChangeCheck())
            {
                scaleOffsetProperty.vectorValue = new Vector4(newScale.x, newScale.y, newOffset.x, newOffset.y);
                strengthProperty.floatValue = strength;
                GenerateNoise(channel, scaleOffsetProperty, strengthProperty, amountOfBending);
            }
        }

        void GenerateNoiseTexture()
        {
            GenerateNoise(NoiseTextureImageChannel.Water, waterNoiseScaleOffset, waterNoiseStrength);
            GenerateNoise(NoiseTextureImageChannel.Surface, surfaceNoiseScaleOffset, surfaceNoiseStrength);
            GenerateNoise(NoiseTextureImageChannel.Reflective, reflectionNoiseScaleOffset, reflectionNoiseStrength);
            GenerateNoise(NoiseTextureImageChannel.Refractive, refractionNoiseScaleOffset, refractionNoiseStrength, refractionAmountOfBending);
        }

        void GenerateNoise(NoiseTextureImageChannel channel, MaterialProperty scaleOffsetProperty, MaterialProperty strengthProperty, MaterialProperty amountOfBending = null)
        {
            Texture2D noiseTex = (Texture2D)noiseTexture.textureValue;

            if (noiseTex == null)
            {
                const int defaultNoiseTextureSize = 128;
                noiseTex = new Texture2D(defaultNoiseTextureSize, defaultNoiseTextureSize, TextureFormat.RGBAHalf, false, true);
                //Mirror Texture Wrap Mode is only supported in Unity 2017 and newer
#if UNITY_2017_1_OR_NEWER
                noiseTex.wrapMode = TextureWrapMode.Mirror;
#else
				noiseTex.wrapMode = TextureWrapMode.Repeat;
#endif
                noiseTex.filterMode = FilterMode.Bilinear;
                noiseTexture.textureValue = noiseTex;
            }

            int noiseSize = noiseTex.width;
            int channelIndex = (int)channel;

            Texture2D noisePreviewTex = noiseTexturePreviews[channelIndex];
            if (noisePreviewTex == null)
            {
                noisePreviewTex = new Texture2D(noiseSize, noiseSize, noiseTex.format, false, true);
                noiseTexturePreviews[channelIndex] = noisePreviewTex;
            }

            Vector4 scaleOffset = scaleOffsetProperty.vectorValue;
            Vector2 scale = new Vector2(scaleOffset.x, scaleOffset.y);
            Vector2 offset = new Vector2(scaleOffset.z, scaleOffset.w);
            float strength = strengthProperty.floatValue;
            float bending = amountOfBending != null ? amountOfBending.floatValue : 0f;

            Color[] noiseTexpixels = noiseTex.GetPixels();
            Color[] previewTexPixels = noisePreviewTex.GetPixels();

            for (int i = 0; i < noiseSize; i++)
            {
                for (int j = 0; j < noiseSize; j++)
                {
                    float x = scale.x * (j / (float)(noiseSize - 1) + offset.x);
                    float y = scale.y * (i / (float)(noiseSize - 1) + offset.y);
                    int pixelIndex = i * noiseSize + j;
                    float noise = Mathf.PerlinNoise(x, y);
                    noiseTexpixels[pixelIndex][channelIndex] = (noise - 0.5f) * strength + bending;
                    previewTexPixels[pixelIndex] = new Color(noise, noise, noise);
                }
            }
            noiseTex.SetPixels(noiseTexpixels);
            noiseTex.Apply();
            noisePreviewTex.SetPixels(previewTexPixels);
            noisePreviewTex.Apply();
            noiseTexture.textureValue = noiseTex;
        }

        void DrawTexturePropertyWithScaleOffsetOpacity(MaterialProperty textureProperty, MaterialProperty opacityProperty)
        {
            EditorGUILayout.LabelField(Styles.TextureLabel);
            Rect rect = EditorGUILayout.GetControlRect(true, MaterialEditor.GetDefaultPropertyHeight(textureProperty), EditorStyles.layerMaskField);
            materialEditor.TextureProperty(rect, textureProperty, string.Empty, false);

            Texture texture = textureProperty.textureValue;

            EditorGUI.BeginDisabledGroup(texture == null);

            rect = materialEditor.GetTexturePropertyCustomArea(rect);
            rect.y -= 32f;
            float rectWidth = rect.width;
            float rectX = rect.x;
            materialEditor.TextureScaleOffsetProperty(rect, textureProperty);

            EditorGUI.showMixedValue = opacityProperty.hasMixedValue;

            EditorGUIUtility.labelWidth = 65f;
            rect.Set(rectX, rect.y + 3 * 16f, rectWidth, 16f);
            rect = EditorGUI.PrefixLabel(rect, Styles.OpacityLabel);

            EditorGUI.BeginChangeCheck();

            float opacity = EditorGUI.Slider(rect, GUIContent.none, opacityProperty.floatValue, 0f, 1f);

            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(Styles.OpacityLabel.text);
                opacityProperty.floatValue = opacity;
            }

            rect.Set(rectX, rect.y + 16f, rectWidth, 16f);
            rect = EditorGUI.PrefixLabel(rect, Styles.wrapModeLabel);

            EditorGUI.BeginChangeCheck();

            TextureWrapMode wrapMode = texture != null ? texture.wrapMode : TextureWrapMode.Repeat;
            wrapMode = (TextureWrapMode)EditorGUI.EnumPopup(rect, wrapMode);

            if (EditorGUI.EndChangeCheck())
            {
                TextureImporter textureImportSettings = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture)) as TextureImporter;
                textureImportSettings.wrapMode = wrapMode;
                textureImportSettings.SaveAndReimport();
            }
            EditorGUIUtility.labelWidth = 0f;

            EditorGUI.showMixedValue = false;

            EditorGUI.EndDisabledGroup();
        }

        void DoNoiseTextureSettingsArea(Material material)
        {
            bool showNoiseTextureSettingsAreaState = EditorPrefs.GetBool("Water2D_ShaderGUI_NoiseTextureSettingsExpanded", false);
            if (showNoiseTextureSettingsArea == null)
            {
                showNoiseTextureSettingsArea = new AnimBool(showNoiseTextureSettingsAreaState);
                showNoiseTextureSettingsArea.valueChanged.AddListener(materialEditor.Repaint);
            }

            EditorGUI.BeginChangeCheck();
            showNoiseTextureSettingsAreaState = EditorGUILayout.Foldout(showNoiseTextureSettingsAreaState, Styles.NoiseTextureSettings, true);
            if (EditorGUI.EndChangeCheck())
            {
                showNoiseTextureSettingsArea.target = showNoiseTextureSettingsAreaState;
                EditorPrefs.SetBool("Water2D_ShaderGUI_NoiseTextureSettingsExpanded", showNoiseTextureSettingsAreaState);
            }

            if (EditorGUILayout.BeginFadeGroup(showNoiseTextureSettingsArea.faded))
            {
                Texture2D noiseTex = noiseTexture.textureValue as Texture2D;
                int noiseSize = noiseTex.width;
                NoiseTextureDataTypePrecision noisePrecision;

                if (noiseTex.format == TextureFormat.RGBA32)
                    noisePrecision = NoiseTextureDataTypePrecision.Low;
                else if (noiseTex.format == TextureFormat.RGBAHalf)
                    noisePrecision = NoiseTextureDataTypePrecision.Medium;
                else
                    noisePrecision = NoiseTextureDataTypePrecision.High;

                EditorGUI.BeginChangeCheck();
                int selectedIndex = (int)Mathf.Log(noiseSize, 2) - 5;
                int newNoiseSize = (int)Mathf.Pow(2, (EditorGUILayout.Popup(Styles.NoiseTextureSizeLabel.text, selectedIndex, noiseTextureSizes) + 5));
                NoiseTextureDataTypePrecision newNoiseTextureDataTypePrecision = (NoiseTextureDataTypePrecision)EditorGUILayout.EnumPopup(Styles.NoiseTexturePrecision, noisePrecision);
                //Mirror Texture Wrap Mode is only supported in Unity 2017 and newer
#if UNITY_2017_1_OR_NEWER
                NoiseTextureWrapMode newNoiseTextureWrapMode = (NoiseTextureWrapMode)EditorGUILayout.EnumPopup(Styles.NoiseTextureWrapMode, noiseTex.wrapMode == TextureWrapMode.Repeat ? NoiseTextureWrapMode.Repeat : NoiseTextureWrapMode.Mirror);
#endif
                FilterMode newNoiseTextureFilterMode = (FilterMode)EditorGUILayout.EnumPopup(Styles.NoiseTextureFilterMode, noiseTex.filterMode);
                if (EditorGUI.EndChangeCheck())
                {
                    TextureFormat format;
                    if (newNoiseTextureDataTypePrecision == NoiseTextureDataTypePrecision.Low)
                        format = TextureFormat.RGBA32;
                    else if (newNoiseTextureDataTypePrecision == NoiseTextureDataTypePrecision.Medium)
                        format = TextureFormat.RGBAHalf;
                    else
                        format = TextureFormat.RGBAFloat;

                    noiseTex.Resize(newNoiseSize, newNoiseSize, format, false);

#if UNITY_2017_1_OR_NEWER
                    noiseTex.wrapMode = newNoiseTextureWrapMode == NoiseTextureWrapMode.Repeat ? TextureWrapMode.Repeat : TextureWrapMode.Mirror;
#else
					noiseTex.wrapMode = TextureWrapMode.Repeat;
#endif
                    noiseTex.filterMode = newNoiseTextureFilterMode;

                    noiseTex.Apply(false, false);

                    noiseTexturePreviews = new Texture2D[4];

                    GenerateNoise(NoiseTextureImageChannel.Water, waterNoiseScaleOffset, waterNoiseStrength);
                    GenerateNoise(NoiseTextureImageChannel.Surface, surfaceNoiseScaleOffset, surfaceNoiseStrength);
                    GenerateNoise(NoiseTextureImageChannel.Reflective, reflectionNoiseScaleOffset, reflectionNoiseStrength);
                    GenerateNoise(NoiseTextureImageChannel.Refractive, refractionNoiseScaleOffset, refractionNoiseStrength, refractionAmountOfBending);
                }

                EditorGUILayout.LabelField(string.Format("Format: {0} , Size: {1} KB", noiseTex.format, Profiler.GetRuntimeMemorySizeLong(noiseTex) / 1024));

                bool textureAssetAlreadyExist = AssetDatabase.Contains(noiseTex);
                if (textureAssetAlreadyExist)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField(GUIContent.none, noiseTex, typeof(object), false);
                    EditorGUI.EndDisabledGroup();
                }

                //Fix Noise Texture missing asset
                bool materialAssetAlreadyExist = AssetDatabase.Contains(material);
                if (materialAssetAlreadyExist && !textureAssetAlreadyExist && GUILayout.Button("Fix Noise Texture"))
                {
                    string materialName = System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(material));
                    string prefabsPath = EditorPrefs.GetString("Water2D_Paths_PrefabUtility_Path");
                    AssetDatabase.CreateAsset(noiseTex, prefabsPath + materialName + "_noiseTexture.asset");
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        static void SetupMaterialWithBlendMode(Material material, bool isRenderModeSetToTransparent)
        {
            if (isRenderModeSetToTransparent)
            {
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
            else
            {
                material.SetOverrideTag("RenderType", "Opaque");
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
            }
        }

        static void SetMaterialKeywords(Material material, bool isRenderModeSetToTransparent)
        {
            bool isWaterTextureEnabled = material.GetTexture("_WaterTexture") != null;
            SetKeyword(material, Styles.WaterTextureKeyword, isWaterTextureEnabled);
            SetKeyword(material, Styles.ColorGradientKeyword, material.GetFloat("_Water2D_IsColorGradientEnabled") == 1.0f);
            SetKeyword(material, Styles.WaterNoiseKeyword, isWaterTextureEnabled && material.GetFloat("_Water2D_IsWaterNoiseEnabled") == 1.0f);

            bool isSurfaceEnabled = material.GetFloat("_Water2D_IsSurfaceEnabled") == 1.0f;
            bool isSurfaceTextureEnabled = isSurfaceEnabled && material.GetTexture("_SurfaceTexture") != null;
            SetKeyword(material, Styles.SurfaceKeyword, isSurfaceEnabled);
            SetKeyword(material, Styles.SurfaceTextureKeyword, isSurfaceEnabled && isSurfaceTextureEnabled);
            SetKeyword(material, Styles.SurfaceNoiseKeyword, isSurfaceEnabled && isSurfaceTextureEnabled && material.GetFloat("_Water2D_IsSurfaceNoiseEnabled") == 1.0f);

            bool isRefractionEnabled = !isRenderModeSetToTransparent && material.GetFloat("_Water2D_IsRefractionEnabled") == 1.0f;
            bool isReflectionEnabled = !isRenderModeSetToTransparent && material.GetFloat("_Water2D_IsReflectionEnabled") == 1.0f;
            SetKeyword(material, Styles.RefractiveKeyword, isRefractionEnabled);
            SetKeyword(material, Styles.ReflectiveKeyword, isReflectionEnabled);
        }

        void MaterialChanged(Material material)
        {
            bool isRenderModeSetToTransparent = blendMode.floatValue == 3000f;
            SetupMaterialWithBlendMode(material, isRenderModeSetToTransparent);
            SetMaterialKeywords(material, isRenderModeSetToTransparent);
        }

        static void SetKeyword(Material material, string keyword, bool state)
        {
            if (state)
                material.EnableKeyword(keyword);
            else
                material.DisableKeyword(keyword);
        }

        #endregion
    }
}
