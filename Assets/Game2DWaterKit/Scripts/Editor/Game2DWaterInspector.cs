using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace Game2DWaterKit
{
    [CanEditMultipleObjects, CustomEditor(typeof(Game2DWater))]
    class Game2DWaterInspector : Editor
    {
        #region variables

        private SerializedProperty subdivisionsCountPerUnit;
        private SerializedProperty waterSize;

        private SerializedProperty damping;
        private SerializedProperty stiffness;
        private SerializedProperty spread;
        private SerializedProperty useCustomBoundaries;
        private SerializedProperty firstCustomBoundary;
        private SerializedProperty secondCustomBoundary;
        private SerializedProperty buoyancyEffectorSurfaceLevel;

        private SerializedProperty collisionMask;
        private SerializedProperty collisionMinimumDepth;
        private SerializedProperty collisionMaximumDepth;
        private SerializedProperty collisionRaycastMaxDistance;
        private SerializedProperty minimumDisturbance;
        private SerializedProperty maximumDisturbance;
        private SerializedProperty velocityMultiplier;

        private SerializedProperty activateConstantRipples;
        private SerializedProperty constantRipplesUpdateWhenOffscreen;
        private SerializedProperty constantRipplesDisturbance;
        private SerializedProperty constantRipplesRandomizeDisturbance;
        private SerializedProperty constantRipplesMinimumDisturbance;
        private SerializedProperty constantRipplesMaximumDisturbance;
        private SerializedProperty constantRipplesRandomizeInterval;
        private SerializedProperty constantRipplesInterval;
        private SerializedProperty constantRipplesMinimumInterval;
        private SerializedProperty constantRipplesMaximumInterval;
        private SerializedProperty constantRipplesSmoothDisturbance;
        private SerializedProperty constantRipplesSmoothFactor;
        private SerializedProperty constantRipplesRandomizeRipplesSourcesPositions;
        private SerializedProperty constantRipplesRandomizeRipplesSourcesCount;
        private SerializedProperty constantRipplesAllowDuplicateRipplesSourcesPositions;
        private SerializedProperty constantRipplesSourcePositions;

        private SerializedProperty refractionRenderTextureResizeFactor;
        private SerializedProperty refractionCullingMask;
        private SerializedProperty reflectionRenderTextureResizeFactor;
        private SerializedProperty reflectionCullingMask;
        private SerializedProperty reflectionZOffset;

        private SerializedProperty renderPixelLights;
        private SerializedProperty sortingLayerID;
        private SerializedProperty sortingOrder;
        private SerializedProperty allowMSAA;
        private SerializedProperty allowHDR;
        private SerializedProperty farClipPlane;

        private SerializedProperty splashAudioClip;
        private SerializedProperty minimumAudioPitch;
        private SerializedProperty maximumAudioPitch;
        private SerializedProperty useConstanAudioPitch;
        private SerializedProperty audioPitch;

        private SerializedProperty activateOnCollisionSplashParticleEffect;
        private SerializedProperty activateConstantSplashParticleEffect;
        private SerializedProperty onCollisionSplashParticleEffect;
        private SerializedProperty constantSplashParticleEffect;
        private SerializedProperty onCollisionSplashParticleEffectPoolSize;
        private SerializedProperty constantSplashParticleEffectPoolSize;
        private SerializedProperty onCollisionSplashParticleEffectSpawnOffset;
        private SerializedProperty constantSplashParticleEffectSpawnOffset;

        private SerializedProperty onWaterEnter;
        
        private static readonly GUIContent fixScalingButtonLabel = new GUIContent("Fix Scaling");
        private static readonly GUIContent waterPropertiesFoldoutLabel = new GUIContent("Water Properties");
        private static readonly GUIContent onCollisionRipplesPropertiesFoldoutLabel = new GUIContent("On Collision Ripples Properties");
        private static readonly GUIContent constantRipplesPropertiesFoldoutLabel = new GUIContent("Constant Ripples Properties");
        private static readonly GUIContent refractionPropertiesFoldoutLabel = new GUIContent("Refraction Properties");
        private static readonly GUIContent reflectionPropertiesFoldoutLabel = new GUIContent("Reflection Properties");
        private static readonly GUIContent renderingSettingsFoldoutLabel = new GUIContent("Rendering Settings");
        private static readonly GUIContent audioSettingsFoldoutLabel = new GUIContent("Splash Sound Effect Properties");
        private static readonly GUIContent spalshParticleSystemSettingsFoldoutLabel = new GUIContent("Spalsh Particle Effect Properties");
        private static readonly GUIContent prefabUtilityFoldoutLabel = new GUIContent("Prefab Utility");

        private static readonly GUIContent waterSizeLabel = new GUIContent("Water Size", "Sets the water size. X represents the width and Y represents the height.");
        private static readonly GUIContent subdivisionsCountPerUnitLabel = new GUIContent("Subdivisions Per Unit", "Sets the number of water’s surface vertices within one unit.");
        private static readonly GUIContent useCustomBoundariesLabel = new GUIContent("Use Custom Boundaries", "Enable/Disable using custom wave boundaries. When waves reach a boundary, they bounce back.");
        private static readonly GUIContent firstCustomBoundaryLabel = new GUIContent("First Boundary", "The location of the first boundary.");
        private static readonly GUIContent secondCustomBoundaryLabel = new GUIContent("Second Boundary", "The location of the second boundary.");

        private static readonly GUIContent dampingLabel = new GUIContent("Damping", "Controls how fast the waves decay. A low value will make waves oscillate for a long time, while a high value will make waves oscillate for a short time.");
        private static readonly GUIContent spreadLabel = new GUIContent("Spread", "Controls how fast the waves spread.");
        private static readonly GUIContent stiffnessLabel = new GUIContent("Stiffness", "Controls the frequency of wave vibration. A low value will make waves oscillate slowly, while a high value will make waves oscillate quickly.");

        private static readonly GUIContent minimumDisturbanceLabel = new GUIContent("Minimum", "The minimum displacement of water’s surface when a GameObject falls into water.");
        private static readonly GUIContent maximumDisturbanceLabel = new GUIContent("Maximum", "The maximum displacement of water’s surface when a GameObject falls into water.");
        private static readonly GUIContent velocityMultiplierLabel = new GUIContent("Velocity Multiplier", "When a rigidbody falls into water, the amount of water’s surface displacement is determined by multiplying the rigidbody velocity by this factor.");
        private static readonly GUIContent buoyancyEffectorSurfaceLevelLabel = new GUIContent("Surface Level", "Sets the surface location of the buoyancy fluid. When a GameObject is above this line, no buoyancy forces are applied. When a GameObject is intersecting or completely below this line, buoyancy forces are applied.");
        private static readonly GUIContent collisionMinimumDepthLabel = new GUIContent("Minimum Depth", "Only GameObjects with Z coordinate (depth) greater than or equal to this value will disturb the water’s surface when they fall into water.");
        private static readonly GUIContent collisionMaximumDepthLabel = new GUIContent("Maximum Depth", "Only GameObjects with Z coordinate (depth) less than or equal to this value will disturb the water’s surface when they fall into water.");
        private static readonly GUIContent collisionRaycastMaxDistanceLabel = new GUIContent("Maximum Distance", "The maximum distance from the water's surface over which to check for collisions (Default: 0.5)");
        private static readonly GUIContent collisionMaskLabel = new GUIContent("Collision Mask", "Only GameObjects on these layers will disturb the water’s surface (produce waves) when they fall into water.");

        private static readonly GUIContent activateConstantRipplesLabel = new GUIContent("Activate", "Activate constant ripples.");
        private static readonly GUIContent constantRipplesUpdateWhenOffscreenLabel = new GUIContent("Update When Off-screen", "Apply constant ripples even when the water is invisible to the camera.");
        private static readonly GUIContent constantRipplesDisturbanceLabel = new GUIContent("Disturbance", "Sets the displacement of water’s surface.");
        private static readonly GUIContent constantRipplesRandomizeDisturbanceLabel = new GUIContent("Randomize", "Randomize the disturbance (displacement) of the water's surface.");
        private static readonly GUIContent constantRipplesMinimumDisturbanceLabel = new GUIContent("Minimum", "Sets the minimum displacement of water’s surface.");
        private static readonly GUIContent constantRipplesMaximumDisturbanceLabel = new GUIContent("Maximum", "Sets the maximum displacement of water’s surface.");
        private static readonly GUIContent randomizePersistnetWaveIntervalLabel = new GUIContent("Randomize", "Randomize the interval.");
        private static readonly GUIContent constantRipplesIntervalLabel = new GUIContent("Interval", "Apply constant ripples at regular intervals (second).");
        private static readonly GUIContent constantRipplesMinimumIntervalLabel = new GUIContent("Minimum", "Minimum Interval.");
        private static readonly GUIContent constantRipplesMaximumIntervalLabel = new GUIContent("Maximum", "Maximum Interval.");
        private static readonly GUIContent constantRipplesRandomizeRipplesSourcesCountLabel = new GUIContent("Sources Count", "Sets the number of constant ripples sources.");
        private static readonly GUIContent constantRipplesSmoothDisturbanceLabel = new GUIContent("Smooth Ripples", "Disturb neighbor vertices to create a smoother ripple (wave).");
        private static readonly GUIContent constantRipplesSmoothFactorLabel = new GUIContent("Smoothing Factor", "The amount of disturbance to apply to neighbor vertices.");
        private static readonly GUIContent constantRipplesRandomizeRipplesSourcesPositionsLabel = new GUIContent("Randomize", "Randomize constant ripples sources positions.");
        private static readonly GUIContent constantRipplesSourcePositionsLabel = new GUIContent("Ripples Sources Positions (X-axis)", "Sets the constant ripples sources positions.");
        private static readonly GUIContent constantRipplesAllowDuplicateRipplesSourcesPositionsLabel = new GUIContent("Allow Duplicate Positions","Allow applying constant ripple at the same position more than once.");
        private static readonly GUIContent constantRipplesEditSourcesPositionsLabel = new GUIContent("Edit Positions", "Edit constant ripples sources positions.");

        private static readonly GUIContent cameraCullingMaskLabel = new GUIContent("Culling Mask", "Only GameObjects on these layers will be rendered.");
        private static readonly GUIContent refractionRenderTextureResizeFactorLabel = new GUIContent("Resize Factor", "Specifies how much the RenderTexture used to render refraction is resized. Decreasing this value lowers the RenderTexture resolution and thus improves performance at the expense of visual quality.");
        private static readonly GUIContent reflectionRenderTextureResizeFactorLabel = new GUIContent("Resize Factor", "Specifies how much the RenderTexture used to render reflection is resized. Decreasing this value lowers the RenderTexture resolution and thus improves performance at the expense of visual quality.");
        private static readonly GUIContent reflectionZOffsetLabel = new GUIContent("Z Offset", "Controls where to start rendering reflection relative to the water GameObject position.");

        private static readonly GUIContent farClipPlaneLabel = new GUIContent("Far Clip Plane", "Sets the furthest point relative to the water that will be drawn when rendering refraction and/or reflection.");
        private static readonly GUIContent renderPixelLightsLabel = new GUIContent("Render Pixel Lights", "Controls whether the rendered objects will be affected by pixel lights. Disabling this could increase performance at the expense of visual fidelity.");
        private static readonly GUIContent sortingLayerLabel = new GUIContent("Sorting Layer", "The name of the water mesh renderer sorting layer.");
        private static readonly GUIContent orderInLayerLabel = new GUIContent("Order In Layer", "The water mesh renderer order within a sorting layer.");
        private static readonly GUIContent allowMSAALabel = new GUIContent("Allow MSAA", "Allow multisample antialiasing rendering.");
        private static readonly GUIContent allowHDRLabel = new GUIContent("Allow HDR", "Allow high dynamic range rendering.");
        private static readonly GUIContent useEdgeCollider2DLabel = new GUIContent("Use Edge Collider 2D", "Adds/Removes an EdgeCollider2D component. The points of the edge collider are automatically updated whenever the water size changes.");

        private static readonly GUIContent activateSplashSoundLabel = new GUIContent("Activate Splash Sound", "Activate/Deactivate playing splash sound effect when a GameObject falls into water. ");
        private static readonly GUIContent splashAudioClipLabel = new GUIContent("Splash Clip", "The AudioClip asset to play when a GameObject falls into water.");
        private static readonly GUIContent minimumAudioPitchLabel = new GUIContent("Minimum Pitch", "Sets the splash audio clip’s minimum playback speed.");
        private static readonly GUIContent maximumAudioPicthLabel = new GUIContent("Maximum Pitch", "Sets the splash audio clip’s maximum playback speed.");
        private static readonly GUIContent useConstanAudioPitchLabel = new GUIContent("Constant Pitch", "Apply constant splash audio clip playback speed.");
        private static readonly GUIContent audioPitchLabel = new GUIContent("Pitch", "Sets the splash audio clip’s playback speed.");
        
        private static readonly GUIContent activateOnCollisionSplashParticleEffectLabel = new GUIContent("Activate On Collision Ripples Splash","Apply splash particle effect when a GameObject falls into water.");
        private static readonly GUIContent activateConstantSplashParticleEffectLabel = new GUIContent("Activate Constant Ripples Splash", "Apply splash particle effect when generating constant ripples.");
        private static readonly GUIContent onCollisionSplashParticleEffectLabel = new GUIContent("Splash Prefab", "Sets the splash particle effect prefab");
        private static readonly GUIContent constantSplashParticleEffectLabel = new GUIContent("Splash Prefab", "Sets the splash particle effect prefab");
        private static readonly GUIContent onCollisionSplashParticleEffectPoolSizeLabel = new GUIContent("Pool Size", "Sets the number of splash particle effect objects that will be created and pooled when the game starts");
        private static readonly GUIContent constantSplashParticleEffectPoolSizeLabel = new GUIContent("Pool Size", "Sets the number of splash particle effect objects that will be created and pooled when the game starts");
        private static readonly GUIContent onCollisionSplashParticleEffectSpawnOffsetLabel = new GUIContent("Spawn Offset", "Shift the splash particle effect spawn position.");
        private static readonly GUIContent constantSplashParticleEffectSpawnOffsetLabel = new GUIContent("Spawn Offset", "Shift the splash particle effect spawn position.");
        
        private static readonly GUIContent onWaterEnterLabel = new GUIContent("OnWaterEnter", "UnityEvent that is triggered when a GameObject falls into water.");

        private static readonly string meshPropertiesLabel = "Mesh Properties";
        private static readonly string wavePropertiesLabel = "Wave Properties";
        private static readonly string disturbancePropertiesLabel = "Disturbance Properties";
        private static readonly string intervalPropertiesLabel = "Interval Properties";
        private static readonly string constantRipplesSourcesPropertiesLabel = "Ripple Source Positions Properties";
        private static readonly string collisionPropertiesLabel = "Collision Properties";
        private static readonly string miscLabel = "Misc";

        private static readonly string audioPitchMessage = "The AudioSource pitch (playback speed) is linearly interpolated between the minimum pitch and the maximum pitch. When a GameObject falls into water, the higher its velocity, the lower the pitch value is.";
        private static readonly string nonUniformScaleWarning = "Unexpected water simulation results may occur when using non-uniform scaling.";
        private static readonly string refractionMessage = "Refraction properties are disabled. \"Refraction\" can be activated in the material editor.";
        private static readonly string reflectionMessage = "Reflection properties are disabled. \"Reflection\" can be activated in the material editor.";

        private static readonly string noiseTextureShaderPropertyName = "_NoiseTexture";

        private static readonly Color wireframeColor = new Color(0.89f, 0.259f, 0.204f, 0.375f);
        private static readonly Color constantRipplesSourcesColorAdd = Color.green;
        private static readonly Color constantRipplesSourcesColorRemove = Color.red;
        private static readonly Color buoyancyEffectorSurfaceLevelGuidelineColor = Color.cyan;

        private AnimBool waterPropertiesExpanded = new AnimBool();
        private AnimBool onCollisionRipplesPropertiesExpanded = new AnimBool();
        private AnimBool constantRipplesPropertiesExpanded = new AnimBool();
        private AnimBool refractionPropertiesExpanded = new AnimBool();
        private AnimBool reflectionPropertiesExpanded = new AnimBool();
        private AnimBool renderingSettingsExpanded = new AnimBool();
        private AnimBool audioSettingsExpanded = new AnimBool();
        private AnimBool spalshParticleEffectSettingsExpanded = new AnimBool();
        private AnimBool prefabUtilityExpanded = new AnimBool();
        private AnimBool activateOnCollisionSplashParticleEffectExpanded = new AnimBool();
        private AnimBool activateConstantSplashParticleEffectExpanded = new AnimBool();

        private bool isMultiEditing = false;
        private bool constantRipplesEditSourcesPositions = false;
        private string prefabsPath;
        private bool updateSplashParticleEffectPool = false;

        #endregion

        #region Methods

        private void OnEnable()
        {
            waterSize = serializedObject.FindProperty("waterSize");
            subdivisionsCountPerUnit = serializedObject.FindProperty("subdivisionsCountPerUnit");

            damping = serializedObject.FindProperty("damping");
            stiffness = serializedObject.FindProperty("stiffness");
            spread = serializedObject.FindProperty("spread");
            useCustomBoundaries = serializedObject.FindProperty("useCustomBoundaries");
            firstCustomBoundary = serializedObject.FindProperty("firstCustomBoundary");
            secondCustomBoundary = serializedObject.FindProperty("secondCustomBoundary");
            buoyancyEffectorSurfaceLevel = serializedObject.FindProperty("buoyancyEffectorSurfaceLevel");
            onWaterEnter = serializedObject.FindProperty("onWaterEnter");

            collisionMask = serializedObject.FindProperty("collisionMask");
            collisionMinimumDepth = serializedObject.FindProperty("collisionMinimumDepth");
            collisionMaximumDepth = serializedObject.FindProperty("collisionMaximumDepth");
            collisionRaycastMaxDistance = serializedObject.FindProperty("collisionRaycastMaxDistance");
            minimumDisturbance = serializedObject.FindProperty("minimumDisturbance");
            maximumDisturbance = serializedObject.FindProperty("maximumDisturbance");
            velocityMultiplier = serializedObject.FindProperty("velocityMultiplier");

            constantRipplesDisturbance = serializedObject.FindProperty("constantRipplesDisturbance");
            constantRipplesUpdateWhenOffscreen = serializedObject.FindProperty("constantRipplesUpdateWhenOffscreen");
            constantRipplesRandomizeDisturbance = serializedObject.FindProperty("constantRipplesRandomizeDisturbance");
            constantRipplesMinimumDisturbance = serializedObject.FindProperty("constantRipplesMinimumDisturbance");
            constantRipplesMaximumDisturbance = serializedObject.FindProperty("constantRipplesMaximumDisturbance");
            constantRipplesRandomizeInterval = serializedObject.FindProperty("constantRipplesRandomizeInterval");
            constantRipplesInterval = serializedObject.FindProperty("constantRipplesInterval");
            constantRipplesMinimumInterval = serializedObject.FindProperty("constantRipplesMinimumInterval");
            constantRipplesMaximumInterval = serializedObject.FindProperty("constantRipplesMaximumInterval");
            constantRipplesSmoothDisturbance = serializedObject.FindProperty("constantRipplesSmoothDisturbance");
            constantRipplesSmoothFactor = serializedObject.FindProperty("constantRipplesSmoothFactor");
            activateConstantRipples = serializedObject.FindProperty("activateConstantRipples");
            constantRipplesRandomizeRipplesSourcesPositions = serializedObject.FindProperty("constantRipplesRandomizeRipplesSourcesPositions");
            constantRipplesRandomizeRipplesSourcesCount = serializedObject.FindProperty("constantRipplesRandomizeRipplesSourcesCount");
            constantRipplesSourcePositions = serializedObject.FindProperty("constantRipplesSourcePositions");
            constantRipplesAllowDuplicateRipplesSourcesPositions = serializedObject.FindProperty("constantRipplesAllowDuplicateRipplesSourcesPositions");

            refractionRenderTextureResizeFactor = serializedObject.FindProperty("refractionRenderTextureResizeFactor");
            refractionCullingMask = serializedObject.FindProperty("refractionCullingMask");
            reflectionRenderTextureResizeFactor = serializedObject.FindProperty("reflectionRenderTextureResizeFactor");
            reflectionCullingMask = serializedObject.FindProperty("reflectionCullingMask");
            reflectionZOffset = serializedObject.FindProperty("reflectionZOffset");

            renderPixelLights = serializedObject.FindProperty("renderPixelLights");
            sortingLayerID = serializedObject.FindProperty("sortingLayerID");
            sortingOrder = serializedObject.FindProperty("sortingOrder");
            allowMSAA = serializedObject.FindProperty("allowMSAA");
            allowHDR = serializedObject.FindProperty("allowHDR");
            farClipPlane = serializedObject.FindProperty("farClipPlane");

            splashAudioClip = serializedObject.FindProperty("splashAudioClip");
            minimumAudioPitch = serializedObject.FindProperty("minimumAudioPitch");
            maximumAudioPitch = serializedObject.FindProperty("maximumAudioPitch");
            useConstanAudioPitch = serializedObject.FindProperty("useConstantAudioPitch");
            audioPitch = serializedObject.FindProperty("audioPitch");

            activateOnCollisionSplashParticleEffect = serializedObject.FindProperty("activateOnCollisionSplashParticleEffect");
            activateConstantSplashParticleEffect = serializedObject.FindProperty("activateConstantSplashParticleEffect");
            onCollisionSplashParticleEffect = serializedObject.FindProperty("onCollisionSplashParticleEffect");
            constantSplashParticleEffect = serializedObject.FindProperty("constantSplashParticleEffect");
            onCollisionSplashParticleEffectPoolSize = serializedObject.FindProperty("onCollisionSplashParticleEffectPoolSize");
            constantSplashParticleEffectPoolSize = serializedObject.FindProperty("constantSplashParticleEffectPoolSize");
            onCollisionSplashParticleEffectSpawnOffset = serializedObject.FindProperty("onCollisionSplashParticleEffectSpawnOffset");
            constantSplashParticleEffectSpawnOffset = serializedObject.FindProperty("constantSplashParticleEffectSpawnOffset");
            
            waterPropertiesExpanded.valueChanged.AddListener(new UnityAction(Repaint));
            onCollisionRipplesPropertiesExpanded.valueChanged.AddListener(new UnityAction(Repaint));
            constantRipplesPropertiesExpanded.valueChanged.AddListener(new UnityAction(Repaint));
            refractionPropertiesExpanded.valueChanged.AddListener(new UnityAction(Repaint));
            reflectionPropertiesExpanded.valueChanged.AddListener(new UnityAction(Repaint));
            renderingSettingsExpanded.valueChanged.AddListener(new UnityAction(Repaint));
            audioSettingsExpanded.valueChanged.AddListener(new UnityAction(Repaint));
            spalshParticleEffectSettingsExpanded.valueChanged.AddListener(new UnityAction(Repaint));
            prefabUtilityExpanded.valueChanged.AddListener(new UnityAction(Repaint));
            activateOnCollisionSplashParticleEffectExpanded.valueChanged.AddListener(new UnityAction(Repaint));
            activateConstantSplashParticleEffectExpanded.valueChanged.AddListener(new UnityAction(Repaint));

            waterPropertiesExpanded.target = EditorPrefs.GetBool("Water2D_WaterPropertiesExpanded", false);
            onCollisionRipplesPropertiesExpanded.target = EditorPrefs.GetBool("Water2D_OnCollisionRipplesPropertiesExpanded", false);
            constantRipplesPropertiesExpanded.target = EditorPrefs.GetBool("Water2D_ConstantRipplesPropertiesExpanded", false);
            refractionPropertiesExpanded.target = EditorPrefs.GetBool("Water2D_RefractionPropertiesExpanded", false);
            reflectionPropertiesExpanded.target = EditorPrefs.GetBool("Water2D_ReflectionPropertiesExpanded", false);
            renderingSettingsExpanded.target = EditorPrefs.GetBool("Water2D_RenderingSettingsExpanded", false);
            audioSettingsExpanded.target = EditorPrefs.GetBool("Water2D_AudioSettingsExpanded", false);
            spalshParticleEffectSettingsExpanded.target = EditorPrefs.GetBool("Water2D_SpashParticleEffectSettingsExpanded", false);
            prefabUtilityExpanded.target = EditorPrefs.GetBool("Water2D_PrefabUtilityExpanded", false);
            activateOnCollisionSplashParticleEffectExpanded.target = EditorPrefs.GetBool("Water2D_ActivateOnCollisionSplashParticleEffectExpanded", false);
            activateConstantSplashParticleEffectExpanded.target = EditorPrefs.GetBool("Water2D_ActivateConstantSplashParticleEffectExpanded", false);

            prefabsPath = EditorPrefs.GetString("Water2D_Paths_PrefabUtility_Path", "Assets/");

            isMultiEditing = targets.Length > 1;
        }

        private void OnDisable()
        {
            waterPropertiesExpanded.valueChanged.RemoveListener(new UnityAction(Repaint));
            onCollisionRipplesPropertiesExpanded.valueChanged.RemoveListener(new UnityAction(Repaint));
            constantRipplesPropertiesExpanded.valueChanged.RemoveListener(new UnityAction(Repaint));
            refractionPropertiesExpanded.valueChanged.RemoveListener(new UnityAction(Repaint));
            reflectionPropertiesExpanded.valueChanged.RemoveListener(new UnityAction(Repaint));
            renderingSettingsExpanded.valueChanged.RemoveListener(new UnityAction(Repaint));
            audioSettingsExpanded.valueChanged.RemoveListener(new UnityAction(Repaint));
            spalshParticleEffectSettingsExpanded.valueChanged.RemoveListener(new UnityAction(Repaint));
            prefabUtilityExpanded.valueChanged.RemoveListener(new UnityAction(Repaint));

            EditorPrefs.SetBool("Water2D_WaterPropertiesExpanded", waterPropertiesExpanded.target);
            EditorPrefs.SetBool("Water2D_OnCollisionRipplesPropertiesExpanded", onCollisionRipplesPropertiesExpanded.target);
            EditorPrefs.SetBool("Water2D_ConstantRipplesPropertiesExpanded", constantRipplesPropertiesExpanded.target);
            EditorPrefs.SetBool("Water2D_RefractionPropertiesExpanded", refractionPropertiesExpanded.target);
            EditorPrefs.SetBool("Water2D_ReflectionPropertiesExpanded", reflectionPropertiesExpanded.target);
            EditorPrefs.SetBool("Water2D_RenderingSettingsExpanded", renderingSettingsExpanded.target);
            EditorPrefs.SetBool("Water2D_AudioSettingsExpanded", audioSettingsExpanded.target);
            EditorPrefs.SetBool("Water2D_SpashParticleEffectSettingsExpanded", spalshParticleEffectSettingsExpanded.target);
            EditorPrefs.SetBool("Water2D_PrefabUtilityExpanded", prefabUtilityExpanded.target);
            EditorPrefs.SetBool("Water2D_ActivateOnCollisionSplashParticleEffectExpanded", activateOnCollisionSplashParticleEffectExpanded.target);
            EditorPrefs.SetBool("Water2D_ActivateConstantSplashParticleEffectExpanded", activateConstantSplashParticleEffectExpanded.target);
            EditorPrefs.SetString("Water2D_Paths_PrefabUtility_Path", prefabsPath);
        }

        public override void OnInspectorGUI()
        {
            Game2DWater water2D = target as Game2DWater;
            Material water2DMaterial = water2D.GetComponent<MeshRenderer>().sharedMaterial;
            bool hasRefraction = false;
            bool hasReflection = false;
            if (water2DMaterial)
            {
                hasRefraction = water2DMaterial.IsKeywordEnabled("Water2D_Refraction");
                hasReflection = water2DMaterial.IsKeywordEnabled("Water2D_Reflection");
            }

            serializedObject.Update();

            if (!isMultiEditing)
                DrawFixScalingField(water2D);

            DrawWaterProperties(water2D);
            DrawOnCollisionRipplesProperties();
            DrawConstantRipplesProperties();
            DrawRefractionProperties(hasRefraction);
            DrawReflectionProperties(hasReflection);
            DrawRenderingSettingsProperties(hasRefraction, hasReflection);
            DrawSplashParticleEffectProperties();
            DrawSplashSoundProperties(water2D);

            if (!isMultiEditing && PrefabUtility.GetPrefabType(water2D) != PrefabType.Prefab)
                DrawPrefabUtility(water2D, water2DMaterial);

            EditorGUILayout.Space();

            //Draw OnWaterEnter event property field
            EditorGUILayout.PropertyField(onWaterEnter, onWaterEnterLabel);

            serializedObject.ApplyModifiedProperties();

            if (updateSplashParticleEffectPool && Application.isPlaying)
            {
                updateSplashParticleEffectPool = false;
                foreach (Game2DWater item in targets)
                {
                    item.ActivateSplashParticleEffect(activateOnCollisionSplashParticleEffect.boolValue, activateConstantSplashParticleEffect.boolValue);
                }
            }
        }

        private void OnSceneGUI()
        {
            Game2DWater game2DWater = target as Game2DWater;

            if (!isMultiEditing)
            {
                DrawWaterResizer(game2DWater);
                if (constantRipplesEditSourcesPositions)
                    DrawConstantRipplesSourcesPositions(game2DWater);
            }
            DrawWaterWireframe(game2DWater);
            DrawBuoyancyEffectorSurfaceLevelGuideline(game2DWater);

            if (GUI.changed)
                SceneView.RepaintAll();
        }

        private void DrawWaterWireframe(Game2DWater water2D)
        {
            List<Vector3> vertices = new List<Vector3>();
            water2D.GetComponent<MeshFilter>().sharedMesh.GetVertices(vertices);
            int start, end;
            if (water2D.UseCustomBoundaries)
            {
                start = 2;
                end = vertices.Count - 4;
            }
            else
            {
                start = 0;
                end = vertices.Count - 2;
            }
            Matrix4x4 localToWorldMatrix = water2D.transform.localToWorldMatrix;
            using (new Handles.DrawingScope(wireframeColor, localToWorldMatrix))
            {
                for (int i = start; i <= end; i += 2)
                {
                    Handles.DrawLine(vertices[i], vertices[i + 1]);
                }
            }
        }

        private void DrawBuoyancyEffectorSurfaceLevelGuideline(Game2DWater water2D)
        {
            Vector2 size = water2D.WaterSize / 2f;
            float y = size.y * (1f - 2f * water2D.BuoyancyEffectorSurfaceLevel);
            Vector3 lineStart = water2D.transform.TransformPoint(-size.x, y, 0f);
            Vector3 lineEnd = water2D.transform.TransformPoint(size.x, y, 0f);
            Handles.color = buoyancyEffectorSurfaceLevelGuidelineColor;
            Handles.DrawLine(lineStart, lineEnd);
            Handles.color = Color.white;
        }

        private void DrawWaterResizer(Game2DWater water2D)
        {
            Bounds bounds = water2D.GetComponent<MeshRenderer>().bounds;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            Vector3 center = bounds.center;

            Vector3 upHandle = new Vector3(center.x, max.y, center.z);
            Vector3 downHandle = new Vector3(center.x, min.y, center.z);
            Vector3 rightHandle = new Vector3(max.x, center.y, center.z);
            Vector3 leftHandle = new Vector3(min.x, center.y, center.z);

            float handlesSize = HandleUtility.GetHandleSize(center) * 0.5f;
            EditorGUI.BeginChangeCheck();
            Vector3 upPos = Handles.Slider(upHandle, Vector3.up, handlesSize, Handles.ArrowHandleCap, 1f);
            Vector3 downPos = Handles.Slider(downHandle, Vector3.down, handlesSize, Handles.ArrowHandleCap, 1f);
            Vector3 rightPos = Handles.Slider(rightHandle, Vector3.right, handlesSize, Handles.ArrowHandleCap, 1f);
            Vector3 leftPos = Handles.Slider(leftHandle, Vector3.left, handlesSize, Handles.ArrowHandleCap, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(water2D, "changing water size");
                Vector3 newCenter = new Vector3((rightPos.x + leftPos.x) / 2f, (upPos.y + downPos.y) / 2f, center.z);
                upPos = water2D.transform.worldToLocalMatrix.MultiplyPoint(upPos);
                downPos = water2D.transform.worldToLocalMatrix.MultiplyPoint(downPos);
                rightPos = water2D.transform.worldToLocalMatrix.MultiplyPoint(rightPos);
                leftPos = water2D.transform.worldToLocalMatrix.MultiplyPoint(leftPos);
                Vector2 newSize = new Vector2(Mathf.Clamp(rightPos.x - leftPos.x, 0f, float.MaxValue), Mathf.Clamp(upPos.y - downPos.y, 0f, float.MaxValue));
                if (newSize.x > 0f && newSize.y > 0f)
                {
                    if (water2D.UseCustomBoundaries)
                    {
                        float halfWidth = newSize.x / 2f;
                        water2D.FirstCustomBoundary = Mathf.Clamp(water2D.FirstCustomBoundary, -halfWidth, halfWidth);
                        water2D.SecondCustomBoundary = Mathf.Clamp(water2D.SecondCustomBoundary, -halfWidth, halfWidth);
                    }
                    water2D.WaterSize = newSize;
                    water2D.transform.position = newCenter;
                    EditorUtility.SetDirty(water2D);
                }
            }
        }

        private void DrawConstantRipplesSourcesPositions(Game2DWater water2D)
        {
            var ripplesSources = water2D.ConstantRipplesSourcePositions;
            List<Vector3> vertices = new List<Vector3>();
            water2D.GetComponent<MeshFilter>().sharedMesh.GetVertices(vertices);
            int surfaceVerticesCount = vertices.Count / 2;

            Vector2 halfWaterSize = water2D.WaterSize / 2f;

            float xStep, leftmostBoundary, rightmostBoundary;
            int indexOffset;
            int start, end;

            bool changeMade = false;
            bool addNewSource = false;
            int index = -1;
            
            Quaternion handlesRotation = Quaternion.identity;
            float handlesSize = HandleUtility.GetHandleSize(water2D.transform.position) * 0.05f;
            Color handlesColor = Handles.color;

            if (water2D.UseCustomBoundaries)
            {
                float firstCustomBoundary = water2D.FirstCustomBoundary;
                float secondCustomBoundary = water2D.SecondCustomBoundary;
                if (firstCustomBoundary < secondCustomBoundary)
                {
                    leftmostBoundary = firstCustomBoundary;
                    rightmostBoundary = secondCustomBoundary;
                }
                else
                {
                    leftmostBoundary = secondCustomBoundary;
                    rightmostBoundary = firstCustomBoundary;
                }
                xStep = (rightmostBoundary - leftmostBoundary) / (surfaceVerticesCount - 3);
                indexOffset = 1;
                start = 2;
                end = vertices.Count - 4;
            }
            else
            {
                xStep = halfWaterSize.x * 2f / (surfaceVerticesCount - 1);
                leftmostBoundary = -halfWaterSize.x;
                rightmostBoundary = halfWaterSize.x;
                indexOffset = 0;
                start = 0;
                end = vertices.Count - 2;
            }

            List<int> indices = new List<int>(ripplesSources.Count);
            Matrix4x4 worldToLocalMatrix = water2D.transform.worldToLocalMatrix;
            Matrix4x4 localToWorldMatrix = water2D.transform.localToWorldMatrix;
            float surfaceYPosition = localToWorldMatrix.m11 * halfWaterSize.y + localToWorldMatrix.m13;

            for (int i = 0, maxi = ripplesSources.Count; i < maxi; i++)
            {
                float xPosition = worldToLocalMatrix.m00 * ripplesSources[i] + worldToLocalMatrix.m03;
                if (xPosition < leftmostBoundary || xPosition > rightmostBoundary)
                {
                        Handles.color = constantRipplesSourcesColorRemove;
                        if (Handles.Button(new Vector3(ripplesSources[i], surfaceYPosition), handlesRotation, handlesSize, handlesSize, Handles.DotHandleCap))
                        {
                            changeMade = true;
                            index = i;
                            addNewSource = false;
                        }
                        indices.Add(-1);
                }
                else
                {
                    int nearestIndex = Mathf.RoundToInt((xPosition - leftmostBoundary) / xStep) + indexOffset;
                    indices.Add(nearestIndex * 2);
                }
            }

            for (int i = start; i <= end; i += 2)
            {
                Vector3 pos = localToWorldMatrix.MultiplyPoint3x4(vertices[i]);

                bool foundMatch = false;
                int foundMatchIndex = -1;
                for (int j = 0,maxj = indices.Count; j < maxj; j++)
                {
                    if (indices[j] == i)
                    {
                        foundMatch = true;
                        foundMatchIndex = j;
                        break;
                    }
                }

                if (foundMatch)
                {
                    Handles.color = constantRipplesSourcesColorRemove;
                    if (Handles.Button(pos, handlesRotation, handlesSize, handlesSize, Handles.DotHandleCap))
                    {
                        changeMade = true;
                        index = foundMatchIndex;
                        addNewSource = false;
                    }
                }
                else
                {
                    Handles.color = constantRipplesSourcesColorAdd;
                    if (Handles.Button(pos, handlesRotation, handlesSize, handlesSize, Handles.DotHandleCap))
                    {
                        changeMade = true;
                        index = i;
                        addNewSource = true;
                    }
                }
            }

            Handles.color = handlesColor;

            if (changeMade)
            {
                Undo.RecordObject(water2D, "editing water ripple source position");
                if (addNewSource)
                    ripplesSources.Add(water2D.transform.localToWorldMatrix.MultiplyPoint3x4(vertices[index]).x);
                else
                    ripplesSources.RemoveAt(index);
                EditorUtility.SetDirty(water2D);

                if (Application.isPlaying)
                    water2D.UpdateConstantRipplesSourceIndices();
            }
        }

        private void DrawWaterProperties(Game2DWater water2D)
        {
            waterPropertiesExpanded.target = EditorGUILayout.Foldout(waterPropertiesExpanded.target, waterPropertiesFoldoutLabel, true);
            using (var group = new EditorGUILayout.FadeGroupScope(waterPropertiesExpanded.faded))
            {
                if (group.visible)
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.LabelField(meshPropertiesLabel, EditorStyles.boldLabel);
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(waterSize, waterSizeLabel);
                    EditorGUILayout.PropertyField(subdivisionsCountPerUnit, subdivisionsCountPerUnitLabel);

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(wavePropertiesLabel, EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(stiffness, stiffnessLabel);
                    EditorGUILayout.PropertyField(spread, spreadLabel);
                    EditorGUILayout.Slider(damping, 0f, 1f, dampingLabel);
                    EditorGUILayout.PropertyField(useCustomBoundaries, useCustomBoundariesLabel);
                    if (useCustomBoundaries.boolValue)
                    {
                        EditorGUILayout.PropertyField(firstCustomBoundary, firstCustomBoundaryLabel);
                        EditorGUILayout.PropertyField(secondCustomBoundary, secondCustomBoundaryLabel);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(miscLabel, EditorStyles.boldLabel);
                    EditorGUILayout.Slider(buoyancyEffectorSurfaceLevel, 0f, 1f, buoyancyEffectorSurfaceLevelLabel);
                    if (!isMultiEditing)
                        DrawEdgeColliderPropertyField(water2D);

                    EditorGUI.indentLevel--;
                }
            }
        }

        private void DrawOnCollisionRipplesProperties()
        {
            onCollisionRipplesPropertiesExpanded.target = EditorGUILayout.Foldout(onCollisionRipplesPropertiesExpanded.target, onCollisionRipplesPropertiesFoldoutLabel, true);
            using (var group = new EditorGUILayout.FadeGroupScope(onCollisionRipplesPropertiesExpanded.faded))
            {
                if (group.visible)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.LabelField(disturbancePropertiesLabel, EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(minimumDisturbance, minimumDisturbanceLabel);
                    EditorGUILayout.PropertyField(maximumDisturbance, maximumDisturbanceLabel);
                    EditorGUILayout.PropertyField(velocityMultiplier, velocityMultiplierLabel);

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(collisionPropertiesLabel, EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(collisionMask, collisionMaskLabel);
                    EditorGUILayout.PropertyField(collisionMinimumDepth, collisionMinimumDepthLabel);
                    EditorGUILayout.PropertyField(collisionMaximumDepth, collisionMaximumDepthLabel);
                    EditorGUILayout.PropertyField(collisionRaycastMaxDistance, collisionRaycastMaxDistanceLabel);

                    EditorGUI.indentLevel--;
                }
            }
        }

        private void DrawConstantRipplesProperties()
        {
            constantRipplesPropertiesExpanded.target = EditorGUILayout.Foldout(constantRipplesPropertiesExpanded.target, constantRipplesPropertiesFoldoutLabel, true);
            using (var group = new EditorGUILayout.FadeGroupScope(constantRipplesPropertiesExpanded.faded))
            {
                if (group.visible)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.PropertyField(activateConstantRipples, activateConstantRipplesLabel);

                    EditorGUI.BeginDisabledGroup(!activateConstantRipples.boolValue);
                    EditorGUILayout.PropertyField(constantRipplesUpdateWhenOffscreen, constantRipplesUpdateWhenOffscreenLabel);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField(disturbancePropertiesLabel, EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(constantRipplesRandomizeDisturbance, constantRipplesRandomizeDisturbanceLabel);
                    bool randomizeDisturbance = constantRipplesRandomizeDisturbance.boolValue;
                    if (randomizeDisturbance)
                    {
                        EditorGUILayout.PropertyField(constantRipplesMinimumDisturbance, constantRipplesMinimumDisturbanceLabel);
                        EditorGUILayout.PropertyField(constantRipplesMaximumDisturbance, constantRipplesMaximumDisturbanceLabel);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(constantRipplesDisturbance, constantRipplesDisturbanceLabel);
                    }
                    EditorGUILayout.PropertyField(constantRipplesSmoothDisturbance, constantRipplesSmoothDisturbanceLabel);
                    bool smoothWave = constantRipplesSmoothDisturbance.boolValue;
                    if (smoothWave)
                        EditorGUILayout.Slider(constantRipplesSmoothFactor, 0f, 1f, constantRipplesSmoothFactorLabel);

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(intervalPropertiesLabel, EditorStyles.boldLabel);

                    EditorGUILayout.PropertyField(constantRipplesRandomizeInterval, randomizePersistnetWaveIntervalLabel);
                    bool randomizeInterval = constantRipplesRandomizeInterval.boolValue;
                    if (randomizeInterval)
                    {
                        EditorGUILayout.PropertyField(constantRipplesMinimumInterval, constantRipplesMinimumIntervalLabel);
                        EditorGUILayout.PropertyField(constantRipplesMaximumInterval, constantRipplesMaximumIntervalLabel);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(constantRipplesInterval, constantRipplesIntervalLabel);
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(constantRipplesSourcesPropertiesLabel, EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(constantRipplesRandomizeRipplesSourcesPositions, constantRipplesRandomizeRipplesSourcesPositionsLabel);
                    bool randomizeRipplesSources = constantRipplesRandomizeRipplesSourcesPositions.boolValue;
                    if (!randomizeRipplesSources)
                    {
                        EditorGUILayout.PropertyField(constantRipplesAllowDuplicateRipplesSourcesPositions, constantRipplesAllowDuplicateRipplesSourcesPositionsLabel);
                        EditorGUI.BeginDisabledGroup(isMultiEditing);
                        constantRipplesEditSourcesPositions = GUILayout.Toggle(constantRipplesEditSourcesPositions, constantRipplesEditSourcesPositionsLabel, "Button");
                        constantRipplesSourcePositions.isExpanded |= constantRipplesEditSourcesPositions;
                        EditorGUILayout.PropertyField(constantRipplesSourcePositions, constantRipplesSourcePositionsLabel, true);
                        EditorGUI.EndDisabledGroup();
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(constantRipplesRandomizeRipplesSourcesCount, constantRipplesRandomizeRipplesSourcesCountLabel);
                        constantRipplesEditSourcesPositions = false;
                    }

                    EditorGUI.indentLevel--;

                    EditorGUI.EndDisabledGroup();
                }
            }
        }

        private void DrawRefractionProperties(bool hasRefraction)
        {
            refractionPropertiesExpanded.target = EditorGUILayout.Foldout(refractionPropertiesExpanded.target, refractionPropertiesFoldoutLabel, true);
            using (var group = new EditorGUILayout.FadeGroupScope(refractionPropertiesExpanded.faded))
            {
                if (group.visible)
                {
                    EditorGUI.indentLevel++;

                    if (!hasRefraction)
                        EditorGUILayout.HelpBox(refractionMessage, MessageType.None, true);
                    EditorGUI.BeginDisabledGroup(!hasRefraction);
                    EditorGUILayout.PropertyField(refractionCullingMask, cameraCullingMaskLabel);
                    EditorGUILayout.Slider(refractionRenderTextureResizeFactor, 0f, 1f, refractionRenderTextureResizeFactorLabel);
                    EditorGUI.EndDisabledGroup();

                    EditorGUI.indentLevel--;
                }
            }
        }

        private void DrawReflectionProperties(bool hasReflection)
        {
            reflectionPropertiesExpanded.target = EditorGUILayout.Foldout(reflectionPropertiesExpanded.target, reflectionPropertiesFoldoutLabel, true);
            using (var group = new EditorGUILayout.FadeGroupScope(reflectionPropertiesExpanded.faded))
            {
                if (group.visible)
                {
                    EditorGUI.indentLevel++;

                    if (!hasReflection)
                        EditorGUILayout.HelpBox(reflectionMessage, MessageType.None, true);
                    EditorGUI.BeginDisabledGroup(!hasReflection);
                    EditorGUILayout.PropertyField(reflectionCullingMask, cameraCullingMaskLabel);
                    EditorGUILayout.Slider(reflectionRenderTextureResizeFactor, 0f, 1f, reflectionRenderTextureResizeFactorLabel);
                    EditorGUILayout.PropertyField(reflectionZOffset, reflectionZOffsetLabel);
                    EditorGUI.EndDisabledGroup();

                    EditorGUI.indentLevel--;
                }
            }
        }

        private void DrawRenderingSettingsProperties(bool hasRefraction, bool hasReflection)
        {
            renderingSettingsExpanded.target = EditorGUILayout.Foldout(renderingSettingsExpanded.target, renderingSettingsFoldoutLabel, true);
            using (var group = new EditorGUILayout.FadeGroupScope(renderingSettingsExpanded.faded))
            {
                if (group.visible)
                {
                    EditorGUI.indentLevel++;

                    EditorGUI.BeginDisabledGroup(!(hasReflection || hasRefraction));
                    EditorGUILayout.PropertyField(farClipPlane, farClipPlaneLabel);
                    EditorGUILayout.PropertyField(renderPixelLights, renderPixelLightsLabel);
                    EditorGUILayout.PropertyField(allowMSAA, allowMSAALabel);
                    EditorGUILayout.PropertyField(allowHDR, allowHDRLabel);
                    EditorGUI.EndDisabledGroup();
                    DrawSortingLayerField(sortingLayerID, sortingOrder);

                    EditorGUI.indentLevel--;
                }
            }
        }

        private void DrawSplashParticleEffectProperties()
        {
            spalshParticleEffectSettingsExpanded.target = EditorGUILayout.Foldout(spalshParticleEffectSettingsExpanded.target, spalshParticleSystemSettingsFoldoutLabel, true);
            using (var group = new EditorGUILayout.FadeGroupScope(spalshParticleEffectSettingsExpanded.faded))
            {
                if (group.visible)
                {
                    EditorGUI.indentLevel++;
                    
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = activateOnCollisionSplashParticleEffect.hasMultipleDifferentValues;
                    activateOnCollisionSplashParticleEffectExpanded.target = EditorGUILayout.ToggleLeft(activateOnCollisionSplashParticleEffectLabel, activateOnCollisionSplashParticleEffect.boolValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    {
                        activateOnCollisionSplashParticleEffect.boolValue = activateOnCollisionSplashParticleEffectExpanded.target;
                        updateSplashParticleEffectPool = true;
                    }
                    using (var subGroup = new EditorGUILayout.FadeGroupScope(activateOnCollisionSplashParticleEffectExpanded.faded))
                    {
                        if (subGroup.visible)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUI.BeginChangeCheck();
                            EditorGUILayout.PropertyField(onCollisionSplashParticleEffect, onCollisionSplashParticleEffectLabel);
                            EditorGUILayout.DelayedIntField(onCollisionSplashParticleEffectPoolSize, onCollisionSplashParticleEffectPoolSizeLabel);
                            if (EditorGUI.EndChangeCheck())
                                updateSplashParticleEffectPool = true;
                            EditorGUILayout.PropertyField(onCollisionSplashParticleEffectSpawnOffset, onCollisionSplashParticleEffectSpawnOffsetLabel);
                            EditorGUI.indentLevel--;
                        }
                    }

                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = activateConstantSplashParticleEffect.hasMultipleDifferentValues;
                    activateConstantSplashParticleEffectExpanded.target = EditorGUILayout.ToggleLeft(activateConstantSplashParticleEffectLabel, activateConstantSplashParticleEffect.boolValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    {
                        activateConstantSplashParticleEffect.boolValue = activateConstantSplashParticleEffectExpanded.target;
                        updateSplashParticleEffectPool = true;
                    }
                    using (var subGroup = new EditorGUILayout.FadeGroupScope(activateConstantSplashParticleEffectExpanded.faded))
                    {
                        if (subGroup.visible)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUI.BeginChangeCheck();
                            EditorGUILayout.PropertyField(constantSplashParticleEffect, constantSplashParticleEffectLabel);
                            EditorGUILayout.DelayedIntField(constantSplashParticleEffectPoolSize, constantSplashParticleEffectPoolSizeLabel);
                            if (EditorGUI.EndChangeCheck())
                                updateSplashParticleEffectPool = true;
                            EditorGUILayout.PropertyField(constantSplashParticleEffectSpawnOffset, constantSplashParticleEffectSpawnOffsetLabel);
                            EditorGUI.indentLevel--;
                        }
                    }
                    
                    EditorGUI.indentLevel--;
                }
            }
        }

        private void DrawSplashSoundProperties(Game2DWater water2D)
        {
            audioSettingsExpanded.target = EditorGUILayout.Foldout(audioSettingsExpanded.target, audioSettingsFoldoutLabel, true);
            using (var group = new EditorGUILayout.FadeGroupScope(audioSettingsExpanded.faded))
            {
                if (group.visible)
                {
                    EditorGUI.indentLevel++;

                    EditorGUI.BeginChangeCheck();
                    bool hasAudioSource = EditorGUILayout.Toggle(activateSplashSoundLabel, water2D.GetComponent<AudioSource>() != null);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (hasAudioSource)
                        {
                            water2D.gameObject.AddComponent<AudioSource>();
                        }
                        else
                        {
                            DestroyImmediate(water2D.GetComponent<AudioSource>());
                        }
                    }

                    EditorGUI.BeginDisabledGroup(!hasAudioSource);
                    EditorGUILayout.PropertyField(splashAudioClip, splashAudioClipLabel);
                    EditorGUILayout.PropertyField(useConstanAudioPitch, useConstanAudioPitchLabel);
                    if (useConstanAudioPitch.boolValue)
                    {
                        EditorGUILayout.Slider(audioPitch, -3f, 3f, audioPitchLabel);
                    }
                    else
                    {
                        EditorGUILayout.Slider(minimumAudioPitch, -3f, 3f, minimumAudioPitchLabel);
                        EditorGUILayout.Slider(maximumAudioPitch, -3f, 3f, maximumAudioPicthLabel);
                        EditorGUILayout.HelpBox(audioPitchMessage, MessageType.None, true);
                    }
                    EditorGUI.EndDisabledGroup();

                    EditorGUI.indentLevel--;
                }
            }
        }

        private void DrawEdgeColliderPropertyField(Game2DWater water2D)
        {
            EditorGUI.BeginChangeCheck();
            bool hasEdgeCollider = EditorGUILayout.Toggle(useEdgeCollider2DLabel, water2D.GetComponent<EdgeCollider2D>() != null);
            if (EditorGUI.EndChangeCheck())
            {
                if (hasEdgeCollider)
                {
                    EdgeCollider2D edgeCollider = water2D.gameObject.AddComponent<EdgeCollider2D>();
                    float xOffset, yOffset;
                    xOffset = -water2D.WaterSize.x / 2f;
                    yOffset = water2D.WaterSize.y / 2f;
                    edgeCollider.points = new[] {
                        new Vector2 ( xOffset, yOffset ),
                        new Vector2 ( xOffset, -yOffset ),
                        new Vector2 ( -xOffset, -yOffset ),
                        new Vector2 ( -xOffset, yOffset )
                    };
                    water2D.OnValidate();
                }
                else
                {
                    DestroyImmediate(water2D.GetComponent<EdgeCollider2D>());
                }
            }
        }

        private void DrawPrefabUtility(Game2DWater water2D, Material water2DMaterial)
        {
            prefabUtilityExpanded.target = EditorGUILayout.Foldout(prefabUtilityExpanded.target, prefabUtilityFoldoutLabel, true);
            using (var group = new EditorGUILayout.FadeGroupScope(prefabUtilityExpanded.faded))
            {
                if (group.visible)
                {
                    EditorGUI.indentLevel++;

                    GameObject water2DGameObject = water2D.gameObject;
                    Texture waterNoiseTexture = water2DMaterial != null && water2DMaterial.HasProperty(noiseTextureShaderPropertyName) ? water2DMaterial.GetTexture(noiseTextureShaderPropertyName) : null;

                    PrefabType prefabType = PrefabUtility.GetPrefabType(water2DGameObject);
                    bool materialAssetAlreadyExist = water2DMaterial != null && AssetDatabase.Contains(water2DMaterial);
                    bool textureAssetAlreadyExist = waterNoiseTexture != null && AssetDatabase.Contains(waterNoiseTexture);

                    EditorGUI.BeginDisabledGroup(true);
#if UNITY_2018_2_OR_NEWER
                    Object prefabObjct = prefabType == PrefabType.PrefabInstance ? PrefabUtility.GetCorrespondingObjectFromSource(water2DGameObject) : null;
#else
                    Object prefabObjct = prefabType == PrefabType.PrefabInstance ? PrefabUtility.GetPrefabParent(water2DGameObject) : null;
#endif
                    EditorGUILayout.ObjectField(prefabObjct, typeof(Object), false);
                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent(prefabsPath, string.Format("Prefab Path: {0}", prefabsPath)), EditorStyles.textField);
                    if (GUILayout.Button(".", EditorStyles.miniButton, GUILayout.MaxWidth(14f)))
                    {
                        string newPrefabsPath = EditorUtility.OpenFolderPanel("Select prefabs path", "Assets", "");
                        if (!string.IsNullOrEmpty(newPrefabsPath))
                        {
                            newPrefabsPath = newPrefabsPath.Substring(Application.dataPath.Length);
                            prefabsPath = "Assets" + newPrefabsPath + "/";
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (prefabType != PrefabType.PrefabInstance)
                    {
                        if (GUILayout.Button("Create Prefab"))
                        {
                            string fileName = GetValidAssetFileName(water2DGameObject.name, ".prefab", typeof(GameObject));

                            if (!textureAssetAlreadyExist && waterNoiseTexture != null)
                            {
                                string noiseTexturePath = prefabsPath + fileName + "_noiseTexture.asset";
                                AssetDatabase.CreateAsset(waterNoiseTexture, noiseTexturePath);
                            }

                            if (!materialAssetAlreadyExist && water2DMaterial != null)
                            {
                                string materialPath = prefabsPath + fileName + ".mat";
                                AssetDatabase.CreateAsset(water2DMaterial, materialPath);
                            }

                            string prefabPath = prefabsPath + fileName + ".prefab";
                            PrefabUtility.CreatePrefab(prefabPath, water2DGameObject, ReplacePrefabOptions.ConnectToPrefab);
                        }
                    }

                    if (prefabType == PrefabType.PrefabInstance)
                    {
                        if (GUILayout.Button("Unlink Prefab"))
                        {
                            PrefabUtility.DisconnectPrefabInstance(water2DGameObject);
#if UNITY_2018_2_OR_NEWER
                            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(water2DGameObject) as GameObject;
#else
                            GameObject prefab = PrefabUtility.GetPrefabParent(water2DGameObject) as GameObject;
#endif
                            Material prefabMaterial = prefab.GetComponent<MeshRenderer>().sharedMaterial;
                            if (water2DMaterial != null && water2DMaterial == prefabMaterial)
                            {
                                bool usePrefabMaterial = EditorUtility.DisplayDialog("Use same prefab's material?",
                            "Do you still want to use the prefab's material?",
                            "Yes",
                            "No, create water's own material");

                                if (!usePrefabMaterial)
                                {
                                    Material duplicateMaterial = new Material(water2DMaterial);
                                    if (waterNoiseTexture != null)
                                    {
                                        Texture duplicateWaterNoiseTexture = Instantiate<Texture>(waterNoiseTexture);
                                        duplicateMaterial.SetTexture("_NoiseTexture", duplicateWaterNoiseTexture);
                                    }
                                    water2DGameObject.GetComponent<MeshRenderer>().sharedMaterial = duplicateMaterial;
                                }
                            }
                        }
                    }

                    if (prefabType == PrefabType.DisconnectedPrefabInstance)
                    {
                        if (GUILayout.Button("Relink Prefab"))
                        {
                            PrefabUtility.ReconnectToLastPrefab(water2DGameObject);

#if UNITY_2018_2_OR_NEWER
                            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(water2DGameObject) as GameObject;
#else
                            GameObject prefab = PrefabUtility.GetPrefabParent(water2DGameObject) as GameObject;
#endif
                            Material prefabMaterial = prefab.GetComponent<MeshRenderer>().sharedMaterial;

                            if (prefabMaterial != null && water2DMaterial != prefabMaterial)
                            {
                                bool usePrefabMaterial = EditorUtility.DisplayDialog("Use prefab's material?",
                                "Do you want to use the prefab's material?",
                                "Yes",
                                "No, continue to use the current water material");

                                if (usePrefabMaterial)
                                {
                                    water2DGameObject.GetComponent<MeshRenderer>().sharedMaterial = prefabMaterial;
                                }
                                else
                                {
                                    if (!materialAssetAlreadyExist)
                                    {
                                        string fileName = GetValidAssetFileName(water2DGameObject.name, ".mat", typeof(Material));

                                        if (!textureAssetAlreadyExist)
                                        {
                                            string noiseTexturePath = prefabsPath + fileName + "_noiseTexture.asset";
                                            AssetDatabase.CreateAsset(waterNoiseTexture, noiseTexturePath);
                                        }

                                        string materialPath = prefabsPath + fileName + ".mat";
                                        AssetDatabase.CreateAsset(water2DMaterial, materialPath);
                                    }
                                }
                            }
                        }
                    }

                    EditorGUI.indentLevel--;
                }
            }
        }

        static void DrawSortingLayerField(SerializedProperty layerID, SerializedProperty orderInLayer)
        {
            MethodInfo methodInfo = typeof(EditorGUILayout).GetMethod("SortingLayerField", BindingFlags.Static | BindingFlags.NonPublic, null, new[] {
                typeof( GUIContent ),
                typeof( SerializedProperty ),
                typeof( GUIStyle ),
                typeof( GUIStyle )
            }, null);

            if (methodInfo != null)
            {
                object[] parameters = { sortingLayerLabel, layerID, EditorStyles.popup, EditorStyles.label };
                methodInfo.Invoke(null, parameters);
                EditorGUILayout.PropertyField(orderInLayer, orderInLayerLabel);
            }

        }

        private bool DrawFixScalingField(Game2DWater water2D)
        {
            Vector2 scale = water2D.transform.localScale;
            if (!Mathf.Approximately(scale.x, 1f) || !Mathf.Approximately(scale.y, 1f))
            {
                EditorGUILayout.HelpBox(nonUniformScaleWarning, MessageType.Warning, true);
                if (GUILayout.Button(fixScalingButtonLabel))
                {
                    waterSize.vector2Value = Vector2.Scale(waterSize.vector2Value, scale);
                    water2D.transform.localScale = Vector3.one;
                    return true;
                }
            }
            return false;
        }

        private string GetValidAssetFileName(string assetName, string assetExtension, System.Type assetType)
        {
            string fileName = assetName;

            string path = prefabsPath + fileName + assetExtension;
            bool prefabWithSameNameExist = AssetDatabase.LoadAssetAtPath(path, assetType) != null;
            if (prefabWithSameNameExist)
            {
                int i = 1;
                while (prefabWithSameNameExist)
                {
                    fileName = assetName + " " + i;
                    path = prefabsPath + fileName + assetExtension;
                    prefabWithSameNameExist = AssetDatabase.LoadAssetAtPath(path, assetType) != null;
                    i++;
                }
            }

            return fileName;
        }

#endregion
    }
}