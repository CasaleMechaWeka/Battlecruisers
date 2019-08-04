using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game2DWaterKit
{
    [
        RequireComponent(typeof(MeshRenderer)),
        RequireComponent(typeof(MeshFilter)),
        RequireComponent(typeof(BoxCollider2D)),
        RequireComponent(typeof(BuoyancyEffector2D))
    ]

    [ExecuteInEditMode]

    public class Game2DWater : MonoBehaviour
    {
        #region variables

        [SerializeField] Vector2 waterSize = Vector2.one;
        [SerializeField] Vector2 lastWaterSize = Vector2.one;
        [SerializeField] int subdivisionsCountPerUnit = 3;

        [SerializeField] float minimumDisturbance = 0.1f;
        [SerializeField] float maximumDisturbance = 0.75f;
        [SerializeField] float velocityMultiplier = 0.12f;
        [SerializeField] LayerMask collisionMask = ~(1 << 4);
        [SerializeField] float collisionMinimumDepth = -10f;
        [SerializeField] float collisionMaximumDepth = 10f;
        [SerializeField] float collisionRaycastMaxDistance = 0.5f;

        [SerializeField] bool activateConstantRipples = false;
        [SerializeField] bool constantRipplesUpdateWhenOffscreen = false;
        [SerializeField] float constantRipplesDisturbance = 0.10f;
        [SerializeField] bool constantRipplesRandomizeDisturbance = false;
        [SerializeField] float constantRipplesMinimumDisturbance = 0.08f;
        [SerializeField] float constantRipplesMaximumDisturbance = 0.12f;
        [SerializeField] bool constantRipplesRandomizeInterval = false;
        [SerializeField] float constantRipplesInterval = 1f;
        [SerializeField] float constantRipplesMinimumInterval = 0.75f;
        [SerializeField] float constantRipplesMaximumInterval = 1.25f;
        [SerializeField] bool constantRipplesSmoothDisturbance = false;
        [SerializeField] float constantRipplesSmoothFactor = 0.5f;
        [SerializeField] bool constantRipplesRandomizeRipplesSourcesPositions = false;
        [SerializeField] int constantRipplesRandomizeRipplesSourcesCount = 3;
        [SerializeField] private bool constantRipplesAllowDuplicateRipplesSourcesPositions = false;
        [SerializeField] List<float> constantRipplesSourcePositions = new List<float>();

        [SerializeField] float damping = 0.05f;
        [SerializeField] float stiffness = 60f;
        [SerializeField] float stiffnessSquareRoot = Mathf.Sqrt(60f);
        [SerializeField] float spread = 60f;
        [SerializeField] bool useCustomBoundaries = false;
        [SerializeField] float firstCustomBoundary = 0.5f;
        [SerializeField] float secondCustomBoundary = -0.5f;
        [SerializeField] float lastSecondCustomBoundary = -0.5f;
        [SerializeField] float lastFirstCustomBoundary = 0.5f;
        [SerializeField] float buoyancyEffectorSurfaceLevel = 0.02f;

        [SerializeField] float refractionRenderTextureResizeFactor = 1f;
        [SerializeField] LayerMask refractionCullingMask = ~(1 << 4);
        [SerializeField] float reflectionRenderTextureResizeFactor = 1f;
        [SerializeField] LayerMask reflectionCullingMask = ~(1 << 4);
        [SerializeField] float reflectionZOffset = 0f;

        [SerializeField] int sortingLayerID = 0;
        [SerializeField] int sortingOrder = 0;
        [SerializeField] float farClipPlane = 100f;
        [SerializeField] bool renderPixelLights = true;
        [SerializeField] bool allowMSAA = false;
        [SerializeField] bool allowHDR = false;

        [SerializeField] AudioClip splashAudioClip = null;
        [SerializeField] bool useConstantAudioPitch = false;
        [SerializeField] float audioPitch = 1f;
        [SerializeField] float minimumAudioPitch = 0.75f;
        [SerializeField] float maximumAudioPitch = 1.25f;

        [SerializeField] bool activateOnCollisionSplashParticleEffect = false;
        [SerializeField] bool activateConstantSplashParticleEffect = false;
        [SerializeField] ParticleSystem onCollisionSplashParticleEffect = null;
        [SerializeField] ParticleSystem constantSplashParticleEffect = null;
        [SerializeField] int onCollisionSplashParticleEffectPoolSize = 10;
        [SerializeField] int constantSplashParticleEffectPoolSize = 10;
        [SerializeField] Vector3 onCollisionSplashParticleEffectSpawnOffset = Vector3.zero;
        [SerializeField] Vector3 constantSplashParticleEffectSpawnOffset = Vector3.zero;

        [SerializeField] UnityEvent onWaterEnter = new UnityEvent();

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        Mesh mesh;
        Material waterMaterial;
        MaterialPropertyBlock materialPropertyBlock;
        BoxCollider2D boxCollider;
        BuoyancyEffector2D buoyancyEffector;
        AudioSource audioSource;
        EdgeCollider2D edgeCollider;
        Vector2[] edgeColliderPoints = new Vector2[4];

        Camera waterCamera;
        RenderTexture refractionRenderTexture;
        RenderTexture reflectionRenderTexture;
        bool renderRefraction;
        bool renderReflection;
        bool updateWaterRenderSettings;

        Vector2 lastWaterBoundsScreenSpaceMin = Vector2.zero;
        Vector2 lastWaterBoundsScreenSpaceMax = Vector2.zero;
        Vector3 waterCameraPositionForReflectionRendering = Vector3.zero;
        Vector3 waterCameraPositionForRefractionRendering = Vector3.zero;

        int surfaceVerticesCount;
        List<Vector3> vertices;
        List<Vector2> uvs;
        List<int> triangles;
        bool updateWaterSimulation;
        float waterPositionOfRest;
        float[] velocities;

        List<int> constantRipplesSourcesIndices = new List<int>();
        List<int> constantRipplesSourcesRandomIndices = new List<int>();
        float constantRipplesDeltaTime;
        float constantRipplesCurrentInterval;

        List<Vector2> waterWorldspaceBounds = new List<Vector2>(4);
        Vector2[] cameraWorldSpaceBounds = new Vector2[4];
        List<Vector2>[] clippingInputOutput = new List<Vector2>[] { new List<Vector2>(), new List<Vector2>() };

        bool useConstantSplashParticleEffect;
        ParticleSystem[] constantSplashParticleEffectPool;
        int constantSplashEffectPoolFirstActiveIndex;
        int constantSplashEffectPoolLastActiveIndex;
        int constantSplashEffectPoolActiveCount;

        bool useOnCollisionSplashParticleEffect;
        ParticleSystem[] onCollisionSplashParticleEffectPool;
        int onCollisionSplashEffectPoolFirstActiveIndex;
        int onCollisionSplashEffectPoolLastActiveIndex;
        int onCollisionSplashEffectPoolActiveCount;

        Transform splashParticleEffectPoolRoot;
        bool waterIsVisible = false;
        Vector2 raycastDirection = Vector2.up;

        private float _originalFixedDeltaTime;

#if UNITY_2017_1_OR_NEWER 
        static int refractionTextureID = Shader.PropertyToID("_RefractionTexture");
        static int reflectionTextureID = Shader.PropertyToID("_ReflectionTexture");
        static int waterMatrixID = Shader.PropertyToID("_WaterMVP");
        static int waterReflectionLowerLimitID = Shader.PropertyToID("_ReflectionLowerLimit");
#else
        int refractionTextureID;
        int reflectionTextureID;
        int waterMatrixID;
        int waterReflectionLowerLimitID;
#endif

        #endregion

        #region Properties

        /// <summary>
        /// Sets the water size. X represents the width and Y represents the height.
        /// </summary>
        public Vector2 WaterSize
        {
            get
            {
                return waterSize;
            }
            set
            {
                value.x = Mathf.Clamp(value.x, 0f, float.MaxValue);
                value.y = Mathf.Clamp(value.y, 0f, float.MaxValue);
                if (waterSize == value)
                    return;
                waterSize = value;
                RecomputeMesh();
                if (activateConstantRipples)
                    UpdateConstantRipplesSourceIndices();
            }
        }

        /// <summary>
        /// Sets the number of water’s surface vertices within one unit.
        /// </summary>
        public int SubdivisionsCountPerUnit
        {
            get
            {
                return subdivisionsCountPerUnit;
            }
            set
            {
                value = Mathf.Clamp(value, 0, int.MaxValue);
                if (subdivisionsCountPerUnit == value)
                    return;
                subdivisionsCountPerUnit = value;
                RecomputeMesh();
                if (activateConstantRipples)
                    UpdateConstantRipplesSourceIndices();
            }
        }

        /// <summary>
        /// Enable/Disable using custom wave boundaries. When waves reach a boundary, they bounce back.
        /// </summary>
        public bool UseCustomBoundaries
        {
            get
            {
                return useCustomBoundaries;
            }
            set
            {
                if (useCustomBoundaries == value)
                    return;
                useCustomBoundaries = value;
                RecomputeMesh();
                if (activateConstantRipples)
                    UpdateConstantRipplesSourceIndices();
            }
        }

        /// <summary>
        /// The location of the first boundary.
        /// </summary>
        public float FirstCustomBoundary
        {
            get
            {
                return firstCustomBoundary;
            }
            set
            {
                float halfWidth = waterSize.x / 2f;
                value = Mathf.Clamp(value, -halfWidth, halfWidth);
                if (Mathf.Approximately(firstCustomBoundary, value))
                    return;
                firstCustomBoundary = value;
                if (useCustomBoundaries)
                    RecomputeMesh();
                if (activateConstantRipples)
                    UpdateConstantRipplesSourceIndices();
            }
        }

        /// <summary>
        /// The location of the second boundary.
        /// </summary>
        public float SecondCustomBoundary
        {
            get
            {
                return secondCustomBoundary;
            }
            set
            {
                float halfWidth = waterSize.x / 2f;
                value = Mathf.Clamp(value, -halfWidth, halfWidth);
                if (Mathf.Approximately(secondCustomBoundary, value))
                    return;
                secondCustomBoundary = value;
                if (useCustomBoundaries)
                    RecomputeMesh();
                if (activateConstantRipples)
                    UpdateConstantRipplesSourceIndices();
            }
        }

        /// <summary>
        /// Sets the surface location of the buoyancy fluid. When a GameObject is above this line, no buoyancy forces are applied. When a GameObject is intersecting or completely below this line, buoyancy forces are applied.
        /// </summary>
        public float BuoyancyEffectorSurfaceLevel
        {
            get
            {
                return buoyancyEffectorSurfaceLevel;
            }
            set
            {
                value = Mathf.Clamp01(value);
                if (Mathf.Approximately(buoyancyEffectorSurfaceLevel, value))
                    return;
                buoyancyEffectorSurfaceLevel = value;
                if (buoyancyEffector)
                    buoyancyEffector.surfaceLevel = waterSize.y * (0.5f - buoyancyEffectorSurfaceLevel);
            }
        }

        /// <summary>
        /// The minimum displacement of water’s surface when an GameObject falls into water.
        /// </summary>
        public float MinimumDisturbance
        {
            get
            {
                return minimumDisturbance;
            }
            set
            {
                minimumDisturbance = Mathf.Clamp(value, 0f, float.MaxValue);
            }
        }

        /// <summary>
        /// The maximum displacement of water’s surface when an GameObject falls into water.
        /// </summary>
        public float MaximumDisturbance
        {
            get
            {
                return maximumDisturbance;
            }
            set
            {
                maximumDisturbance = Mathf.Clamp(value, 0f, float.MaxValue);
            }
        }

        /// <summary>
        /// When a rigidbody falls into water, the amount of water’s surface displacement is determined by multiplying the rigidbody velocity by this factor.
        /// </summary>
        public float VelocityMultiplier
        {
            get
            {
                return velocityMultiplier;
            }
            set
            {
                velocityMultiplier = Mathf.Clamp(value, 0f, float.MaxValue);
            }
        }

        /// <summary>
        /// Controls how fast the waves decay. A low value will make waves oscillate for a long time, while a high value will make waves oscillate for a short time.
        /// </summary>
        public float Damping
        {
            get
            {
                return damping;
            }
            set
            {
                damping = Mathf.Clamp01(value);
            }
        }

        /// <summary>
        /// Controls how fast the waves spread.
        /// </summary>
        public float Spread
        {
            get
            {
                return spread;
            }
            set
            {
                spread = Mathf.Clamp(value, 0f, float.MaxValue);
            }
        }

        /// <summary>
        /// Controls the frequency of wave vibration. A low value will make waves oscillate slowly, while a high value will make waves oscillate quickly.
        /// </summary>
        public float Stiffness
        {
            get
            {
                return stiffness;
            }
            set
            {
                stiffness = Mathf.Clamp(value, 0f, float.MaxValue);
                stiffnessSquareRoot = Mathf.Sqrt(stiffness);
            }
        }

        /// <summary>
        /// Only GameObjects on these layers will disturb the water’s surface (produce waves) when they fall into water.
        /// </summary>
        public LayerMask CollisionMask
        {
            get
            {
                return collisionMask;
            }
            set
            {
                collisionMask = value & ~(1 << 4);
            }
        }

        /// <summary>
        /// The maximum distance from the water's surface over which to check for collisions (Default: 0.5)
        /// </summary>
        public float CollisionRaycastMaxDistance
        {
            get
            {
                return collisionRaycastMaxDistance;
            }
            set
            {
                collisionRaycastMaxDistance = Mathf.Clamp(value, 0f, float.MaxValue);
            }
        }

        /// <summary>
        /// Only GameObjects with Z coordinate (depth) greater than or equal to this value will disturb the water’s surface when they fall into water.
        /// </summary>
        public float MinimumCollisionDepth
        {
            get
            {
                return collisionMinimumDepth;
            }
            set
            {
                collisionMinimumDepth = value;
            }
        }

        /// <summary>
        /// Only GameObjects with Z coordinate (depth) less than or equal to this value will disturb the water’s surface when they fall into water.
        /// </summary>
        public float MaximumCollisionDepth
        {
            get
            {
                return collisionMaximumDepth;
            }
            set
            {
                collisionMaximumDepth = value;
            }
        }

        /// <summary>
        /// Specifies how much the RenderTexture used to render refraction is resized. Decreasing this value lowers the RenderTexture resolution and thus improves performance at the expense of visual quality.
        /// </summary>
        public float RefractionRenderTextureResizeFactor
        {
            get
            {
                return refractionRenderTextureResizeFactor;
            }
            set
            {
                value = Mathf.Clamp01(value);
                if (Mathf.Approximately(refractionRenderTextureResizeFactor, value))
                    return;
                refractionRenderTextureResizeFactor = value;
                updateWaterRenderSettings = true;
            }
        }

        /// <summary>
        /// Only GameObjects on these layers will be rendered.
        /// </summary>
        public LayerMask RefractionCullingMask
        {
            get
            {
                return refractionCullingMask;
            }
            set
            {
                refractionCullingMask = value & ~(1 << 4);
            }
        }

        /// <summary>
        /// Specifies how much the RenderTexture used to render reflection is resized. Decreasing this value lowers the RenderTexture resolution and thus improves performance at the expense of visual quality.
        /// </summary>
        public float ReflectionRenderTextureResizeFactor
        {
            get
            {
                return reflectionRenderTextureResizeFactor;
            }
            set
            {
                value = Mathf.Clamp01(value);
                if (Mathf.Approximately(reflectionRenderTextureResizeFactor, value))
                    return;
                reflectionRenderTextureResizeFactor = value;
                updateWaterRenderSettings = true;
            }
        }

        /// <summary>
        /// Only GameObjects on these layers will be rendered.
        /// </summary>
        public LayerMask ReflectionCullingMask
        {
            get
            {
                return reflectionCullingMask;
            }
            set
            {
                reflectionCullingMask = value & ~(1 << 4);
            }
        }

        /// <summary>
        /// Controls where to start rendering reflection relative to the water GameObject position.
        /// </summary>
        public float ReflectionZOffset
        {
            get
            {
                return reflectionZOffset;
            }
            set
            {
                reflectionZOffset = value;
                waterCameraPositionForReflectionRendering.z = transform.position.z + reflectionZOffset;
            }
        }

        /// <summary>
        /// The name of the water mesh renderer sorting layer.
        /// </summary>
        public int SortingLayerID
        {
            get
            {
                return sortingLayerID;
            }
            set
            {

                if (sortingLayerID == value)
                    return;
                sortingLayerID = value;
                if (meshRenderer)
                    meshRenderer.sortingLayerID = sortingLayerID;
            }
        }

        /// <summary>
        /// The water mesh renderer order within a sorting layer.
        /// </summary>
        public int SortingOrder
        {
            get
            {
                return sortingOrder;
            }
            set
            {
                if (sortingOrder == value)
                    return;
                sortingOrder = value;
                if (meshRenderer)
                    meshRenderer.sortingOrder = sortingOrder;
            }
        }

        /// <summary>
        /// Controls whether the rendered objects will be affected by pixel lights. Disabling this could increase performance at the expense of visual fidelity.
        /// </summary>
        public bool RenderPixelLights
        {
            get
            {
                return renderPixelLights;
            }
            set
            {
                renderPixelLights = value;
            }
        }

        /// <summary>
        /// Sets the furthest point relative to the water to draw when rendering refraction and/or reflection.
        /// </summary>
        public float FarClipPlane
        {
            get
            {
                return farClipPlane;
            }
            set
            {
                if (Mathf.Approximately(farClipPlane, value))
                    return;
                farClipPlane = value;
                updateWaterRenderSettings = true;
                if (waterCamera)
                    waterCamera.farClipPlane = farClipPlane;
            }
        }

        /// <summary>
        /// Allow multisample antialiasing rendering.
        /// </summary>
        public bool AllowMSAA
        {
            get
            {
                return allowMSAA;
            }
            set
            {
                if (allowMSAA == value)
                    return;
                allowMSAA = value;
                if (waterCamera)
                    waterCamera.allowMSAA = allowMSAA;
            }
        }

        /// <summary>
        /// Allow high dynamic range rendering.
        /// </summary>
        public bool AllowHDR
        {
            get
            {
                return allowHDR;
            }
            set
            {
                allowHDR = value;
                if (waterCamera)
                    waterCamera.allowHDR = allowHDR;
            }
        }

        /// <summary>
        /// The AudioClip asset to play when a GameObject falls into water.
        /// </summary>
        public AudioClip SplashAudioClip
        {
            get
            {
                return splashAudioClip;
            }
            set
            {
                splashAudioClip = value;
            }
        }

        /// <summary>
        /// Sets the splash audio clip’s minimum playback speed.
        /// </summary>
        public float MinimumAudioPitch
        {
            get
            {
                return minimumAudioPitch;
            }
            set
            {
                minimumAudioPitch = Mathf.Clamp(value, -3f, 3f);
            }
        }

        /// <summary>
        /// Sets the splash audio clip’s maximum playback speed.
        /// </summary>
        public float MaximumAudioPitch
        {
            get
            {
                return maximumAudioPitch;
            }
            set
            {
                maximumAudioPitch = Mathf.Clamp(value, -3f, 3f);
            }
        }

        /// <summary>
        /// Apply constant splash audio clip playback speed.
        /// </summary>
        public bool UseConstantAudioPitch
        {
            get
            {
                return useConstantAudioPitch;
            }
            set
            {
                useConstantAudioPitch = value;
            }
        }

        /// <summary>
        /// Sets the splash audio clip’s playback speed.
        /// </summary>
        public float AudioPitch
        {
            get
            {
                return audioPitch;
            }
            set
            {
                audioPitch = Mathf.Clamp(value, -3f, 3f);
            }
        }

        /// <summary>
        /// Apply splash particle effect when generating constant ripples.
        /// </summary>
        public bool ActivateConstantSplashParticleEffect
        {
            get
            {
                return activateConstantSplashParticleEffect;
            }
        }

        /// <summary>
        /// Apply splash particle effect when a GameObject falls into water.
        /// </summary>
        public bool ActivateOnCollisionSplashParticleEffect
        {
            get
            {
                return activateOnCollisionSplashParticleEffect;
            }
        }

        /// <summary>
        /// Sets the splash particle effect prefab.
        /// </summary>
        public ParticleSystem OnCollisionSplashParticleEffect
        {
            get
            {
                return onCollisionSplashParticleEffect;
            }
            set
            {
                onCollisionSplashParticleEffect = value;
                InitSplashParticleEffect();
            }
        }

        /// <summary>
        /// Sets the splash particle effect prefab.
        /// </summary>
        public ParticleSystem ConstantSplashParticleEffect
        {
            get
            {
                return constantSplashParticleEffect;
            }
            set
            {
                constantSplashParticleEffect = value;
                InitSplashParticleEffect();
            }
        }

        /// <summary>
        /// Sets the number of splash particle effect objects that will be created and pooled when the game starts
        /// </summary>
        public int OnCollisionSplashParticleEffectPoolSize
        {
            get
            {
                return onCollisionSplashParticleEffectPoolSize;
            }
            set
            {
                onCollisionSplashParticleEffectPoolSize = Mathf.Clamp(value, 0, int.MaxValue);
                InitSplashParticleEffect();
            }
        }

        /// <summary>
        /// Sets the number of splash particle effect objects that will be created and pooled when the game starts
        /// </summary>
        public int ConstantSplashParticleEffectPoolSize
        {
            get
            {
                return constantSplashParticleEffectPoolSize;
            }
            set
            {
                constantSplashParticleEffectPoolSize = Mathf.Clamp(value, 0, int.MaxValue);
                InitSplashParticleEffect();
            }
        }

        /// <summary>
        /// Shift the splash particle effect spawn position.
        /// </summary>
        public Vector3 OnCollisionSplashParticleEffectSpawnOffset
        {
            get
            {
                return onCollisionSplashParticleEffectSpawnOffset;
            }
            set
            {
                onCollisionSplashParticleEffectSpawnOffset = value;
            }
        }

        /// <summary>
        /// Shift the splash particle effect spawn position.
        /// </summary>
        public Vector3 ConstantSplashParticleEffectSpawnOffset
        {
            get
            {
                return constantSplashParticleEffectSpawnOffset;
            }
            set
            {
                constantSplashParticleEffectSpawnOffset = value;
            }
        }

        /// <summary>
        /// Sets the constant ripples sources positions.
        /// </summary>
        public List<float> ConstantRipplesSourcePositions
        {
            get
            {
                return constantRipplesSourcePositions;
            }
            set
            {
                constantRipplesSourcePositions = value;
                UpdateConstantRipplesSourceIndices();
            }
        }

        /// <summary>
        /// Activate constant ripples.
        /// </summary>
        public bool ActivateConstantRipples
        {
            get
            {
                return activateConstantRipples;
            }
            set
            {
                activateConstantRipples = value;
                if (value)
                    UpdateConstantRipplesSourceIndices();
            }
        }

        /// <summary>
        /// Apply constant ripples even when the water is invisible to the camera.
        /// </summary>
        public bool ConstantRipplesUpdateWhenOffscreen
        {
            get
            {
                return constantRipplesUpdateWhenOffscreen;
            }
            set
            {
                constantRipplesUpdateWhenOffscreen = value;
            }
        }

        /// <summary>
        /// Sets the displacement of water’s surface.
        /// </summary>
        public float ConstantRipplesDisturbance
        {
            get
            {
                return constantRipplesDisturbance;
            }
            set
            {
                constantRipplesDisturbance = Mathf.Clamp(value, 0f, float.MaxValue);
            }
        }

        /// <summary>
        /// Randomize the disturbance (displacement) of the water's surface.
        /// </summary>
        public bool ConstantRipplesRandomizeDisturbance
        {
            get
            {
                return constantRipplesRandomizeDisturbance;
            }
            set
            {
                constantRipplesRandomizeDisturbance = value;
            }
        }

        /// <summary>
        /// Sets the minimum displacement of water’s surface.
        /// </summary>
        public float ConstantRipplesMinimumDisturbance
        {
            get
            {
                return constantRipplesMinimumDisturbance;
            }
            set
            {
                constantRipplesMinimumDisturbance = Mathf.Clamp(value, 0f, float.MaxValue);
            }
        }

        /// <summary>
        /// Sets the maximum displacement of water’s surface.
        /// </summary>
        public float ConstantRipplesMaximumDisturbance
        {
            get
            {
                return constantRipplesMaximumDisturbance;
            }
            set
            {
                constantRipplesMaximumDisturbance = Mathf.Clamp(value, 0f, float.MaxValue);
            }
        }

        /// <summary>
        /// Randomize the interval.
        /// </summary>
        public bool ConstantRipplesRandomizeInterval
        {
            get
            {
                return constantRipplesRandomizeInterval;
            }
            set
            {
                constantRipplesRandomizeInterval = value;
            }
        }

        /// <summary>
        /// Apply constant ripples at regular intervals (second).
        /// </summary>
        public float ConstantRipplesInterval
        {
            get
            {
                return constantRipplesInterval;
            }
            set
            {
                constantRipplesInterval = Mathf.Clamp(value, 0f, float.MaxValue);
            }
        }

        /// <summary>
        /// Minimum Interval.
        /// </summary>
        public float ConstantRipplesMinimumInterval
        {
            get
            {
                return constantRipplesMinimumInterval;
            }
            set
            {
                constantRipplesMinimumInterval = Mathf.Clamp(value, 0f, float.MaxValue);
            }
        }

        /// <summary>
        /// Maximum Interval.
        /// </summary>
        public float ConstantRipplesMaximumInterval
        {
            get
            {
                return constantRipplesMaximumInterval;
            }
            set
            {
                constantRipplesMaximumInterval = Mathf.Clamp(value, 0f, float.MaxValue);
            }
        }

        /// <summary>
        /// Disturb neighbor vertices to create a smoother ripple (wave).
        /// </summary>
        public bool ConstantRipplesSmoothDisturbance
        {
            get
            {
                return constantRipplesSmoothDisturbance;
            }
            set
            {
                constantRipplesSmoothDisturbance = value;
            }
        }

        /// <summary>
        /// The amount of disturbance to apply to neighbor vertices.
        /// </summary>
        public float ConstantRipplesSmoothFactor
        {
            get
            {
                return constantRipplesSmoothFactor;
            }
            set
            {
                constantRipplesSmoothFactor = Mathf.Clamp01(value);
            }
        }

        /// <summary>
        /// Randomize constant ripples sources positions.
        /// </summary>
        public bool ConstantRipplesRandomizeRipplesSourcesPositions
        {
            get
            {
                return constantRipplesRandomizeRipplesSourcesPositions;
            }
            set
            {
                constantRipplesRandomizeRipplesSourcesPositions = value;
            }
        }

        /// <summary>
        /// Sets the number of constant ripples sources.
        /// </summary>
        public int ConstantRipplesRandomizeRipplesSourcesCount
        {
            get
            {
                return constantRipplesRandomizeRipplesSourcesCount;
            }
            set
            {
                constantRipplesRandomizeRipplesSourcesCount = Mathf.Clamp(value, 0, int.MaxValue);
            }
        }

        /// <summary>
        /// Allow applying constant ripple at the same position more than once.
        /// </summary>
        public bool ConstantRipplesAllowDuplicateRipplesSourcesPositions
        {
            get
            {
                return constantRipplesAllowDuplicateRipplesSourcesPositions;
            }
            set
            {
                constantRipplesAllowDuplicateRipplesSourcesPositions = value;
                UpdateConstantRipplesSourceIndices();
            }
        }

        #endregion

        #region Methods

#if UNITY_EDITOR
        // Add menu item to create Game2D Water GameObject.
        // Priority 10 ensures it is grouped with the other menu items of the same kind and propagated to the hierarchy dropdown and hierarchy context menus.
        [MenuItem("GameObject/2D Object/Game2D Water", false, 10)]
        static private void CreateCustomGameObject(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Game2D Water");
            go.AddComponent<Game2DWater>();
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create" + go.name);
            Selection.activeObject = go;
        }

        private void Reset()
        {
            if (waterCamera)
            {
                DestroyImmediate(waterCamera.gameObject);
                waterCamera = null;
            }

            waterSize = Vector2.one;
            lastWaterSize = Vector2.one;
            subdivisionsCountPerUnit = 3;

            minimumDisturbance = 0.1f;
            maximumDisturbance = 0.75f;
            velocityMultiplier = 0.12f;
            collisionMask = ~(1 << 4);
            collisionMinimumDepth = -10f;
            collisionMaximumDepth = 10f;
            collisionRaycastMaxDistance = 0.5f;

            damping = 0.05f;
            stiffness = 60f;
            stiffnessSquareRoot = Mathf.Sqrt(60f);
            spread = 60f;
            useCustomBoundaries = false;
            firstCustomBoundary = 0.5f;
            secondCustomBoundary = -0.5f;
            lastSecondCustomBoundary = -0.5f;
            lastFirstCustomBoundary = 0.5f;
            buoyancyEffectorSurfaceLevel = 0.02f;

            refractionRenderTextureResizeFactor = 1f;
            refractionCullingMask = ~(1 << 4);
            reflectionRenderTextureResizeFactor = 1f;
            reflectionCullingMask = ~(1 << 4);
            reflectionZOffset = 0f;

            sortingLayerID = 0;
            sortingOrder = 0;
            farClipPlane = 100f;
            renderPixelLights = true;
            allowMSAA = false;
            allowHDR = false;

            splashAudioClip = null;
            useConstantAudioPitch = false;
            minimumAudioPitch = 0.75f;
            maximumAudioPitch = 1.25f;
            audioPitch = 1f;

            activateOnCollisionSplashParticleEffect = false;
            activateConstantSplashParticleEffect = false;
            onCollisionSplashParticleEffect = null;
            constantSplashParticleEffect = null;
            onCollisionSplashParticleEffectPoolSize = 10;
            constantSplashParticleEffectPoolSize = 10;
            onCollisionSplashParticleEffectSpawnOffset = Vector3.zero;
            constantSplashParticleEffectSpawnOffset = Vector3.zero;

            activateConstantRipples = false;
            constantRipplesRandomizeDisturbance = false;
            constantRipplesDisturbance = 0.10f;
            constantRipplesMinimumDisturbance = 0.08f;
            constantRipplesMaximumDisturbance = 0.12f;
            constantRipplesRandomizeInterval = false;
            constantRipplesInterval = 1f;
            constantRipplesMinimumInterval = 0.75f;
            constantRipplesMaximumInterval = 1.25f;
            constantRipplesSmoothDisturbance = false;
            constantRipplesSmoothFactor = 0.5f;
            constantRipplesRandomizeRipplesSourcesPositions = false;
            constantRipplesRandomizeRipplesSourcesCount = 3;
            constantRipplesAllowDuplicateRipplesSourcesPositions = false;
            constantRipplesSourcePositions.Clear();
            constantRipplesSourcesIndices.Clear();

            onWaterEnter = new UnityEvent(); ;

            RecomputeMesh();
        }

        //This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
        public void OnValidate()
        {
            refractionRenderTextureResizeFactor = Mathf.Clamp01(refractionRenderTextureResizeFactor);
            reflectionRenderTextureResizeFactor = Mathf.Clamp01(reflectionRenderTextureResizeFactor);
            reflectionCullingMask &= ~(1 << 4);
            refractionCullingMask &= ~(1 << 4);
            minimumDisturbance = Mathf.Clamp(minimumDisturbance, 0f, float.MaxValue);
            maximumDisturbance = Mathf.Clamp(maximumDisturbance, 0f, float.MaxValue);
            velocityMultiplier = Mathf.Clamp(velocityMultiplier, 0f, float.MaxValue);
            damping = Mathf.Clamp01(damping);
            stiffness = Mathf.Clamp(stiffness, 0f, float.MaxValue);
            stiffnessSquareRoot = Mathf.Sqrt(stiffness);
            spread = Mathf.Clamp(spread, 0f, float.MaxValue);
            buoyancyEffectorSurfaceLevel = Mathf.Clamp01(buoyancyEffectorSurfaceLevel);
            collisionMask &= ~(1 << 4);
            collisionRaycastMaxDistance = Mathf.Clamp(collisionRaycastMaxDistance, 0f, float.MaxValue);
            waterSize.x = Mathf.Clamp(waterSize.x, 0f, float.MaxValue);
            waterSize.y = Mathf.Clamp(waterSize.y, 0f, float.MaxValue);
            subdivisionsCountPerUnit = Mathf.Clamp(subdivisionsCountPerUnit, 0, int.MaxValue);
            float halfWidth = waterSize.x / 2f;
            firstCustomBoundary = Mathf.Clamp(firstCustomBoundary, -halfWidth, halfWidth);
            secondCustomBoundary = Mathf.Clamp(secondCustomBoundary, -halfWidth, halfWidth);

            constantRipplesDisturbance = Mathf.Clamp(constantRipplesDisturbance, 0f, float.MaxValue);
            constantRipplesMinimumDisturbance = Mathf.Clamp(constantRipplesMinimumDisturbance, 0f, float.MaxValue);
            constantRipplesMaximumDisturbance = Mathf.Clamp(constantRipplesMaximumDisturbance, 0f, float.MaxValue);
            constantRipplesInterval = Mathf.Clamp(constantRipplesInterval, 0f, float.MaxValue);
            constantRipplesMinimumInterval = Mathf.Clamp(constantRipplesMinimumInterval, 0f, float.MaxValue);
            constantRipplesMaximumInterval = Mathf.Clamp(constantRipplesMaximumInterval, 0f, float.MaxValue);
            constantRipplesRandomizeRipplesSourcesCount = Mathf.Clamp(constantRipplesRandomizeRipplesSourcesCount, 0, int.MaxValue);
            if (Application.isPlaying)
                UpdateConstantRipplesSourceIndices();

            onCollisionSplashParticleEffectPoolSize = Mathf.Clamp(onCollisionSplashParticleEffectPoolSize, 0, int.MaxValue);
            constantSplashParticleEffectPoolSize = Mathf.Clamp(constantSplashParticleEffectPoolSize, 0, int.MaxValue);

            edgeCollider = GetComponent<EdgeCollider2D>();

            if (meshFilter && meshFilter.sharedMesh)
            {
                Vector3 meshSize = meshFilter.sharedMesh.bounds.size;
                int meshVerticesCount = meshFilter.sharedMesh.vertexCount;
                int vertexCount;
                Vector2 size;
                if (useCustomBoundaries)
                    size = new Vector2(Mathf.Abs(secondCustomBoundary - firstCustomBoundary), waterSize.y);
                else
                    size = waterSize;
                vertexCount = (Mathf.RoundToInt(size.x * subdivisionsCountPerUnit) + 2) * 2;

                if (meshVerticesCount != vertexCount || !Mathf.Approximately(meshSize.x, size.x) || !Mathf.Approximately(meshSize.y, size.y))
                {
                    RecomputeMesh();
                    if (activateConstantRipples)
                        UpdateConstantRipplesSourceIndices();
                }
            }

            if (waterCamera)
            {
                waterCamera.allowMSAA = allowMSAA;
                waterCamera.allowHDR = allowHDR;
                waterCamera.farClipPlane = farClipPlane;
            }

            if (meshRenderer)
            {
                meshRenderer.sortingLayerID = sortingLayerID;
                meshRenderer.sortingOrder = sortingOrder;
            }

            UpdateComponents();
        }
#endif

        private void OnEnable()
        {
            if (!mesh || !meshFilter.sharedMesh)
                RecomputeMesh();

#if !UNITY_2017_1_OR_NEWER
            refractionTextureID = Shader.PropertyToID("_RefractionTexture");
            reflectionTextureID = Shader.PropertyToID("_ReflectionTexture");
            waterMatrixID = Shader.PropertyToID("_WaterMVP");
            waterReflectionLowerLimitID = Shader.PropertyToID("_ReflectionLowerLimit");
#endif
        }

        private void OnDisable()
        {
            if (waterCamera)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(waterCamera.gameObject);
                else
#endif
                    Destroy(waterCamera.gameObject);
            }
        }

        private void OnDestroy()
        {
            if (splashParticleEffectPoolRoot != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(splashParticleEffectPoolRoot.gameObject);
                else
#endif
                    Destroy(splashParticleEffectPoolRoot.gameObject);
            }
        }

        private void Awake()
        {
            _originalFixedDeltaTime = Time.fixedDeltaTime;

            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();

            waterMaterial = meshRenderer.sharedMaterial;
            if (!waterMaterial)
            {
                waterMaterial = new Material(Shader.Find("Game2DWaterKit/Unlit (Supports Lightmap)"));
                waterMaterial.name = gameObject.name + " Material";
                meshRenderer.sharedMaterial = waterMaterial;
            }
            meshRenderer.sortingLayerID = sortingLayerID;
            meshRenderer.sortingOrder = sortingOrder;

            boxCollider = GetComponent<BoxCollider2D>();
            //BuoyancyEffector only works when an attached collider is marked as a trigger and used by effector 
            boxCollider.isTrigger = true;
            boxCollider.usedByEffector = true;

            edgeCollider = GetComponent<EdgeCollider2D>();
            buoyancyEffector = GetComponent<BuoyancyEffector2D>();
            audioSource = GetComponent<AudioSource>();

            gameObject.layer = LayerMask.NameToLayer("Water");

            renderRefraction = waterMaterial.IsKeywordEnabled("Water2D_Refraction");
            renderReflection = waterMaterial.IsKeywordEnabled("Water2D_Reflection");

            RecomputeMesh();

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
#endif
                if (activateConstantRipples)
                    UpdateConstantRipplesSourceIndices();

                InitSplashParticleEffect();
#if UNITY_EDITOR
            }
#endif
        }

        private void OnBecameVisible()
        {
            waterIsVisible = true;
        }

        private void OnBecameInvisible()
        {
            waterIsVisible = false;
        }

        //Sent when another object enters a water's box trigger collider attached to this object (2D physics only).
        private void OnTriggerEnter2D(Collider2D other)
        {
            Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;

            float totalVelocities = 0f;
            int hitsCount = 0;

            int vertexIndex;
            int endIndex;
            int startIndex;
            if (useCustomBoundaries)
            {
                startIndex = 1;
                endIndex = surfaceVerticesCount - 1;
                vertexIndex = 2;
            }
            else
            {
                startIndex = 0;
                endIndex = surfaceVerticesCount;
                vertexIndex = 0;
            }

            Vector2 meanPosition = new Vector2();

            for (int i = startIndex; i < endIndex; i++, vertexIndex += 2)
            {
                Vector2 surfaceVertexLocalSpacePosition = vertices[vertexIndex];
                Vector2 surfaceVertexWorldSpacePosition;
                surfaceVertexWorldSpacePosition.x = localToWorldMatrix.m00 * surfaceVertexLocalSpacePosition.x + localToWorldMatrix.m01 * surfaceVertexLocalSpacePosition.y + localToWorldMatrix.m03;
                surfaceVertexWorldSpacePosition.y = localToWorldMatrix.m10 * surfaceVertexLocalSpacePosition.x + localToWorldMatrix.m11 * surfaceVertexLocalSpacePosition.y + localToWorldMatrix.m13;
                RaycastHit2D hit = Physics2D.Raycast(surfaceVertexWorldSpacePosition, raycastDirection, collisionRaycastMaxDistance, collisionMask, collisionMinimumDepth, collisionMaximumDepth);
                if (hit.rigidbody && hit.collider == other)
                {
                    meanPosition += hit.point;
                    float velocity = -hit.rigidbody.GetPointVelocity(surfaceVertexWorldSpacePosition).y * velocityMultiplier;
                    velocity = Mathf.Clamp(velocity, minimumDisturbance, maximumDisturbance);
                    velocities[i] -= velocity * stiffnessSquareRoot;
                    totalVelocities += velocity;
                    hitsCount++;
                }
            }

            if (hitsCount > 0)
            {
                updateWaterSimulation = true;
                float meanVelocity = totalVelocities / hitsCount;
                PlaySplashSound(meanVelocity);
                if (useOnCollisionSplashParticleEffect)
                {
                    meanPosition /= hitsCount;
                    Vector3 spawnPosition = new Vector3(meanPosition.x, meanPosition.y, transform.position.z);
                    spawnPosition += onCollisionSplashParticleEffectSpawnOffset;
                    SpawnSplashParticleSystem(true, spawnPosition);
                }

                if (onWaterEnter != null)
                    onWaterEnter.Invoke();
            }
        }

        private void FixedUpdate()
        {
            if (updateWaterSimulation)
            {
                updateWaterSimulation = false;

                float deltaTime = _originalFixedDeltaTime;
                float dampingFactor = damping * 2f * stiffnessSquareRoot;
                float spreadFactor = spread * subdivisionsCountPerUnit;

                int vertexIndex;
                int startIndex;
                int endIndex;

                if (useCustomBoundaries)
                {
                    startIndex = 1;
                    endIndex = surfaceVerticesCount - 1;
                    vertexIndex = 2;
                }
                else
                {
                    startIndex = 0;
                    endIndex = surfaceVerticesCount;
                    vertexIndex = 0;
                }

                Vector3 currentVertexPosition = vertices[vertexIndex];
                Vector3 prevVertexPosition = currentVertexPosition;
                Vector3 nextVertexPosition;

                for (int i = startIndex; i < endIndex; i++, vertexIndex += 2)
                {
                    nextVertexPosition = i < endIndex - 1 ? vertices[vertexIndex + 2] : currentVertexPosition;

                    float velocity = velocities[i];
                    float restoringForce = stiffness * (waterPositionOfRest - currentVertexPosition.y);
                    float dampingForce = -dampingFactor * velocity;
                    float spreadForce = spreadFactor * (prevVertexPosition.y - currentVertexPosition.y + nextVertexPosition.y - currentVertexPosition.y);

                    prevVertexPosition = currentVertexPosition;

                    velocity += (restoringForce + dampingForce + spreadForce) * deltaTime;
                    currentVertexPosition.y += velocity * deltaTime;

                    vertices[vertexIndex] = currentVertexPosition;
                    velocities[i] = velocity;

                    currentVertexPosition = nextVertexPosition;

                    if (!updateWaterSimulation)
                    {
                        //if all the velocities are in the [-0.0001,0.0001] range then, we don't need to continue updating the 
                        //water simulation and thus improving performance
                        const float UpdateWaterSimulationMinimumThreshold = 0.0001f;
                        updateWaterSimulation |= velocity > UpdateWaterSimulationMinimumThreshold || velocity < -UpdateWaterSimulationMinimumThreshold;
                    }
                }

                //Ensure that we apply the changes to the mesh
                mesh.SetVertices(vertices);
                mesh.UploadMeshData(false);
                mesh.RecalculateBounds();
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
#endif
                if (activateConstantRipples)
                    UpdateConstantRipples();
                if (useOnCollisionSplashParticleEffect || useConstantSplashParticleEffect)
                    UpdateSplashParticleEffectPool();
#if UNITY_EDITOR
            }
#endif

            AnimateWaterSize();
        }

        //OnWillRenderObject is called for each camera if the object is visible.
        private void OnWillRenderObject()
        {
#if UNITY_EDITOR
            waterMaterial = meshRenderer.sharedMaterial;
#endif

            if (!waterMaterial || waterSize == Vector2.zero)
                return;

#if UNITY_EDITOR
            renderRefraction = waterMaterial.IsKeywordEnabled("Water2D_Refraction");
            renderReflection = waterMaterial.IsKeywordEnabled("Water2D_Reflection");
#endif

            if (!(renderReflection || renderRefraction))
                return;

            Camera currentCamera = Camera.current;

            if (!currentCamera)
                return;

            if (!waterCamera)
            {
                GameObject waterCameraGameObject = new GameObject("Water Camera For" + GetInstanceID());
                //we will take care of creating and destroying this camera
                waterCameraGameObject.hideFlags = HideFlags.HideAndDontSave;
                waterCamera = waterCameraGameObject.AddComponent<Camera>();
                //we will render this camera manually
                waterCameraGameObject.SetActive(false);
                waterCamera.enabled = false;
                waterCamera.clearFlags = CameraClearFlags.SolidColor;
                waterCamera.orthographic = true;
                waterCamera.nearClipPlane = 0.03f;
                waterCamera.farClipPlane = farClipPlane;
                waterCamera.allowHDR = allowHDR;
                waterCamera.allowMSAA = allowMSAA;
            }

            //we get water bounds in world space
            Bounds bounds = meshRenderer.bounds;
            Vector2 waterBoundsMin, waterBoundsMax;
            waterBoundsMin = bounds.min;
            waterBoundsMax = bounds.max;

            //when the size or the position of the water changes, or the current camera moves , we update our render settings
            Vector2 waterBoundsScreenSpaceMin = currentCamera.WorldToScreenPoint(waterBoundsMin);
            Vector2 waterBoundsScreenSpaceMax = currentCamera.WorldToScreenPoint(waterBoundsMax);
            updateWaterRenderSettings = waterBoundsMin != lastWaterBoundsScreenSpaceMin || waterBoundsMax != lastWaterBoundsScreenSpaceMax;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                updateWaterRenderSettings = true;
            }
#endif

            if (updateWaterRenderSettings)
            {
                lastWaterBoundsScreenSpaceMin = waterBoundsScreenSpaceMin;
                lastWaterBoundsScreenSpaceMax = waterBoundsScreenSpaceMax;
                updateWaterRenderSettings = false;

                //Find water/camera bounds intersection polygon
                waterWorldspaceBounds.Clear();
                waterWorldspaceBounds.Add(waterBoundsMax);
                waterWorldspaceBounds.Add(new Vector2(waterBoundsMax.x, waterBoundsMin.y));
                waterWorldspaceBounds.Add(waterBoundsMin);
                waterWorldspaceBounds.Add(new Vector2(waterBoundsMin.x, waterBoundsMax.y));

                cameraWorldSpaceBounds[0] = currentCamera.ViewportToWorldPoint(new Vector3(1f, 1f));
                cameraWorldSpaceBounds[1] = currentCamera.ViewportToWorldPoint(new Vector3(1f, 0f));
                cameraWorldSpaceBounds[2] = currentCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
                cameraWorldSpaceBounds[3] = currentCamera.ViewportToWorldPoint(new Vector3(0f, 1f));

                List<Vector2> waterCameraIntersectionPolygon = FindIntersectedPolygon();

                //compute the water visible area (water/camera intersection polygon) bounds
                Vector2 visibleAreaBoundsMin = waterBoundsMax;
                Vector2 visibleAreaBoundsMax = waterBoundsMin;

                for (int i = 0, length = waterCameraIntersectionPolygon.Count; i < length; i++)
                {
                    Vector2 vertexPosition = waterCameraIntersectionPolygon[i];
                    if (vertexPosition.x < visibleAreaBoundsMin.x)
                        visibleAreaBoundsMin.x = vertexPosition.x;

                    if (vertexPosition.x > visibleAreaBoundsMax.x)
                        visibleAreaBoundsMax.x = vertexPosition.x;

                    if (vertexPosition.y < visibleAreaBoundsMin.y)
                        visibleAreaBoundsMin.y = vertexPosition.y;

                    if (vertexPosition.y > visibleAreaBoundsMax.y)
                        visibleAreaBoundsMax.y = vertexPosition.y;
                }

                //compute the water camera projection matrix
                Matrix4x4 waterMatrix = Matrix4x4.Ortho(visibleAreaBoundsMin.x, visibleAreaBoundsMax.x, visibleAreaBoundsMin.y, visibleAreaBoundsMax.y, 0.03f, farClipPlane);
                waterCamera.projectionMatrix = waterMatrix;

                Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
                if (materialPropertyBlock == null)
                    materialPropertyBlock = new MaterialPropertyBlock();
                meshRenderer.GetPropertyBlock(materialPropertyBlock);
                waterMatrix = waterMatrix * localToWorldMatrix;
                materialPropertyBlock.SetMatrix(waterMatrixID, waterMatrix);

                //compute the render textures size
                float screenPixelsPerUnit = currentCamera.pixelHeight / (currentCamera.orthographicSize * 2f);
                int textureWidth = Mathf.RoundToInt((visibleAreaBoundsMax.x - visibleAreaBoundsMin.x) * screenPixelsPerUnit);
                int textureHeight = Mathf.RoundToInt((visibleAreaBoundsMax.y - visibleAreaBoundsMin.y) * screenPixelsPerUnit);

                float zPosition = transform.position.z;
                if (renderRefraction)
                {
                    waterCameraPositionForRefractionRendering.z = zPosition;

                    int refractionTextureWidth = Mathf.RoundToInt(textureWidth * refractionRenderTextureResizeFactor);
                    int refractionTextureHeight = Mathf.RoundToInt(textureHeight * refractionRenderTextureResizeFactor);

                    if (refractionTextureWidth < 1 || refractionTextureHeight < 1)
                        return;

                    if (refractionRenderTexture)
                    {
                        RenderTexture.ReleaseTemporary(refractionRenderTexture);
                    }
                    refractionRenderTexture = RenderTexture.GetTemporary(refractionTextureWidth, refractionTextureHeight, 16);

                    materialPropertyBlock.SetTexture(refractionTextureID, refractionRenderTexture);
                }

                if (renderReflection)
                {
                    //we position the water camera just above the water surface
                    waterCameraPositionForReflectionRendering.y = -(visibleAreaBoundsMax.y + visibleAreaBoundsMin.y) + localToWorldMatrix.m11 * waterSize.y + 2f * localToWorldMatrix.m13;
                    waterCameraPositionForReflectionRendering.z = zPosition + reflectionZOffset;

                    int reflectionTextureWidth = Mathf.RoundToInt(textureWidth * reflectionRenderTextureResizeFactor);
                    int reflectionTextureHeight = Mathf.RoundToInt(textureHeight * reflectionRenderTextureResizeFactor);

                    if (reflectionTextureWidth < 1 || reflectionTextureHeight < 1)
                        return;

                    if (reflectionRenderTexture)
                    {
                        RenderTexture.ReleaseTemporary(reflectionRenderTexture);
                    }
                    reflectionRenderTexture = RenderTexture.GetTemporary(reflectionTextureWidth, reflectionTextureHeight, 16);

                    materialPropertyBlock.SetTexture(reflectionTextureID, reflectionRenderTexture);
                    float waterReflectionLowerLimit = (waterMatrix.m11 * waterSize.y * 0.5f + waterMatrix.m13) * 0.5f + 0.5f;
                    materialPropertyBlock.SetFloat(waterReflectionLowerLimitID, waterReflectionLowerLimit);
                }

                meshRenderer.SetPropertyBlock(materialPropertyBlock);
            }

            int pixelLightsCount = QualitySettings.pixelLightCount;
            if (!renderPixelLights)
            {
                QualitySettings.pixelLightCount = 0;
            }

            Color waterCameraBackgroundColor = currentCamera.backgroundColor;
            waterCameraBackgroundColor.a = 0f;
            waterCamera.backgroundColor = waterCameraBackgroundColor;

            if (renderRefraction)
            {
                waterCamera.transform.position = waterCameraPositionForRefractionRendering;
                waterCamera.cullingMask = refractionCullingMask;
                waterCamera.targetTexture = refractionRenderTexture;
                waterCamera.Render();
            }

            if (renderReflection)
            {
                waterCamera.transform.position = waterCameraPositionForReflectionRendering;
                waterCamera.cullingMask = reflectionCullingMask;
                waterCamera.targetTexture = reflectionRenderTexture;
                waterCamera.Render();
            }

            QualitySettings.pixelLightCount = pixelLightsCount;
        }

        /// <summary>
        /// Create a splash at a particular position.
        /// </summary>
        /// <param name="xPosition">splash xPosition in world space</param>
        /// <param name="disturbanceFactor">Range: [0..1]: The disturbance is linearly interpolated between (onCollision ripples) minimum disturbance and (onCollision ripples) maximum disturbance by this factor.</param>
        /// <param name="playSound">Play splash audio clip.</param>
        /// <param name="playParticleEffect">Play (onCollision ripples) splash Particle Effect.</param>
        /// <param name="smooth">Disturb neighbor vertices to create a smoother ripple (wave).</param>
        /// <param name="smoothingFactor">Range: [0..1]: The amount of disturbance to apply to neighbor vertices.</param>
        public void CreateSplash(float xPosition, float disturbanceFactor, bool playSound, bool playParticleEffect, bool smooth, float smoothingFactor = 0.5f)
        {
            Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
            Vector2 halfWaterSize = waterSize / 2f;

            float leftMostWaterVertexWorldSpace, rightMostWaterVertexWorldSpace;
            int startIndex, endIndex;
            if (!useCustomBoundaries)
            {
                leftMostWaterVertexWorldSpace = localToWorldMatrix.m00 * (-halfWaterSize.x) + localToWorldMatrix.m01 * halfWaterSize.y + localToWorldMatrix.m03;
                rightMostWaterVertexWorldSpace = localToWorldMatrix.m00 * (halfWaterSize.x) + localToWorldMatrix.m01 * halfWaterSize.y + localToWorldMatrix.m03;
                startIndex = 0;
                endIndex = velocities.Length - 1;
            }
            else
            {
                float firstBoundaryX = localToWorldMatrix.m00 * firstCustomBoundary + localToWorldMatrix.m01 * halfWaterSize.y + localToWorldMatrix.m03;
                float secondBoundaryX = localToWorldMatrix.m00 * secondCustomBoundary + localToWorldMatrix.m01 * halfWaterSize.y + localToWorldMatrix.m03;
                if (firstBoundaryX < secondBoundaryX)
                {
                    leftMostWaterVertexWorldSpace = firstBoundaryX;
                    rightMostWaterVertexWorldSpace = secondBoundaryX;
                }
                else
                {
                    leftMostWaterVertexWorldSpace = secondBoundaryX;
                    rightMostWaterVertexWorldSpace = firstBoundaryX;
                }
                startIndex = 1;
                endIndex = velocities.Length - 2;
            }

            if (xPosition < leftMostWaterVertexWorldSpace || xPosition > rightMostWaterVertexWorldSpace)
                return;

            float velocity = stiffnessSquareRoot * Mathf.Lerp(minimumDisturbance, maximumDisturbance, disturbanceFactor);
            int indexOffset = useCustomBoundaries ? 1 : 0;
            float delta = (xPosition - leftMostWaterVertexWorldSpace) * subdivisionsCountPerUnit;

            int index = indexOffset + Mathf.RoundToInt(delta);
            velocities[index] -= velocity;
            if (smooth)
            {
                float smoothedVelocity = velocity * smoothingFactor;

                int previousNearestIndex = index - 1;
                if (previousNearestIndex >= startIndex)
                    velocities[previousNearestIndex] -= smoothedVelocity;

                int nextNearestIndex = index + 1;
                if (nextNearestIndex <= endIndex)
                    velocities[nextNearestIndex] -= smoothedVelocity;
            }

            if (playParticleEffect && useConstantSplashParticleEffect)
            {
                float surfaceY = localToWorldMatrix.m10 * halfWaterSize.x + localToWorldMatrix.m11 * halfWaterSize.y + localToWorldMatrix.m13;
                Vector3 spawnPosition = new Vector3(xPosition, surfaceY, transform.position.z);
                spawnPosition += onCollisionSplashParticleEffectSpawnOffset;
                SpawnSplashParticleSystem(true, spawnPosition);
            }
            if (playSound)
                PlaySplashSound(velocity);

            updateWaterSimulation = true;
        }

        public void ActivateSplashParticleEffect(bool activateOnCollisionParticleEffect, bool activateConstantParticleEffect)
        {
            activateOnCollisionSplashParticleEffect = activateOnCollisionParticleEffect;
            activateConstantSplashParticleEffect = activateConstantParticleEffect;
            InitSplashParticleEffect();
        }

        public void UpdateConstantRipplesSourceIndices()
        {
            constantRipplesSourcesIndices.Clear();

            float halfWidth = waterSize.x / 2f;

            float xStep, leftmostBoundary, rightmostBoundary;
            int indexOffset;

            if (useCustomBoundaries)
            {
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
            }
            else
            {
                xStep = 2f * halfWidth / (surfaceVerticesCount - 1);
                leftmostBoundary = -halfWidth;
                rightmostBoundary = halfWidth;
                indexOffset = 0;
            }

            Matrix4x4 worldToLocalMatrix = transform.worldToLocalMatrix;
            for (int i = 0, maxi = constantRipplesSourcePositions.Count; i < maxi; i++)
            {
                float xPosition = worldToLocalMatrix.m00 * constantRipplesSourcePositions[i] + worldToLocalMatrix.m03;
                if (xPosition < leftmostBoundary || xPosition > rightmostBoundary)
                    continue;
                int nearestIndex = Mathf.RoundToInt((xPosition - leftmostBoundary) / xStep) + indexOffset;
                if (!constantRipplesAllowDuplicateRipplesSourcesPositions)
                {
                    bool isDuplicate = false;
                    for (int j = 0, maxj = constantRipplesSourcesIndices.Count; j < maxj; j++)
                    {
                        if (constantRipplesSourcesIndices[j] == nearestIndex)
                        {
                            isDuplicate = true;
                            break;
                        }
                    }
                    if (isDuplicate)
                        continue;
                }
                constantRipplesSourcesIndices.Add(nearestIndex);
            }
        }

        public void RecomputeMesh()
        {
            if (!mesh || !meshFilter.sharedMesh)
            {
                mesh = new Mesh();
                mesh.MarkDynamic();
                mesh.hideFlags = HideFlags.HideAndDontSave;
                mesh.name = "Water2D Mesh";
                meshFilter.sharedMesh = mesh;
            }

            float halfWidth = waterSize.x / 2f;
            float halfHeight = waterSize.y / 2f;

            float activeWaterSurfaceWidth;
            float xStep;
            float leftmostBoundary;
            float uStep;
            float leftmostBoundaryU;

            if (useCustomBoundaries)
            {
                activeWaterSurfaceWidth = Mathf.Abs(secondCustomBoundary - firstCustomBoundary);
                surfaceVerticesCount = Mathf.RoundToInt(activeWaterSurfaceWidth * subdivisionsCountPerUnit) + 4;
                xStep = activeWaterSurfaceWidth / (surfaceVerticesCount - 3);
                leftmostBoundary = Mathf.Min(secondCustomBoundary, firstCustomBoundary);
                uStep = (activeWaterSurfaceWidth / waterSize.x) / (surfaceVerticesCount - 3);
                leftmostBoundaryU = (leftmostBoundary + halfWidth) / waterSize.x;
            }
            else
            {
                activeWaterSurfaceWidth = 2f * halfWidth;
                surfaceVerticesCount = Mathf.RoundToInt(waterSize.x * subdivisionsCountPerUnit) + 2;
                xStep = activeWaterSurfaceWidth / (surfaceVerticesCount - 1);
                leftmostBoundary = -halfWidth + xStep;
                uStep = 1f / (surfaceVerticesCount - 1);
                leftmostBoundaryU = uStep;
            }

            vertices = new List<Vector3>(surfaceVerticesCount * 2);
            uvs = new List<Vector2>(surfaceVerticesCount * 2);
            triangles = new List<int>((surfaceVerticesCount - 1) * 6);
            velocities = new float[surfaceVerticesCount];

            vertices.Add(new Vector3(-halfWidth, halfHeight));
            vertices.Add(new Vector3(-halfWidth, -halfHeight));

            uvs.Add(new Vector2(0f, 1f));
            uvs.Add(new Vector2(0f, 0f));

            triangles.Add(0);
            triangles.Add(2);
            triangles.Add(3);
            triangles.Add(0);
            triangles.Add(3);
            triangles.Add(1);

            float xPosition = 0f;
            float uPosition = 0f;
            for (int i = 1, index = 2, max = surfaceVerticesCount - 1; i < max; i++, index += 2)
            {
                float x = xPosition + leftmostBoundary;
                xPosition += xStep;
                vertices.Add(new Vector3(x, halfHeight));
                vertices.Add(new Vector3(x, -halfHeight));

                float u = uPosition + leftmostBoundaryU;
                uPosition += uStep;
                uvs.Add(new Vector2(u, 1f));
                uvs.Add(new Vector2(u, 0f));

                triangles.Add(index);
                triangles.Add(index + 2);
                triangles.Add(index + 3);
                triangles.Add(index);
                triangles.Add(index + 3);
                triangles.Add(index + 1);
            }

            vertices.Add(new Vector3(halfWidth, halfHeight));
            vertices.Add(new Vector3(halfWidth, -halfHeight));

            uvs.Add(new Vector2(1f, 1f));
            uvs.Add(new Vector2(1f, 0f));

            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();

            UpdateComponents();
            waterPositionOfRest = halfHeight;
            lastWaterSize = waterSize;
            if (useCustomBoundaries)
            {
                lastFirstCustomBoundary = firstCustomBoundary;
                lastSecondCustomBoundary = secondCustomBoundary;
            }
        }

        private void UpdateComponents()
        {
            float halfWidth = waterSize.x / 2f;
            float halfHeight = waterSize.y / 2f;
            if (boxCollider != null)
                boxCollider.size = waterSize;
            if (edgeCollider != null)
            {
                edgeColliderPoints[0].x = edgeColliderPoints[1].x = -halfWidth;
                edgeColliderPoints[2].x = edgeColliderPoints[3].x = halfWidth;

                edgeColliderPoints[0].y = edgeColliderPoints[3].y = halfHeight;
                edgeColliderPoints[1].y = edgeColliderPoints[2].y = -halfHeight;

                edgeCollider.points = edgeColliderPoints;
            }
            if (buoyancyEffector != null)
                buoyancyEffector.surfaceLevel = waterSize.y * (0.5f - buoyancyEffectorSurfaceLevel);
        }

        private void PlaySplashSound(float velocity)
        {
            if (!audioSource)
                return;

            if (useConstantAudioPitch)
            {
                audioSource.pitch = audioPitch;
            }
            else
            {
                float interpolationValue = 1f - Mathf.InverseLerp(minimumDisturbance, maximumDisturbance, velocity);
                audioSource.pitch = Mathf.Lerp(minimumAudioPitch, maximumAudioPitch, interpolationValue);
            }
            audioSource.PlayOneShot(splashAudioClip);
        }

        private void InitSplashParticleEffect()
        {
            useOnCollisionSplashParticleEffect = onCollisionSplashParticleEffect != null && activateOnCollisionSplashParticleEffect;
            useConstantSplashParticleEffect = constantSplashParticleEffect != null && activateConstantSplashParticleEffect;

            constantSplashEffectPoolFirstActiveIndex = 0;
            constantSplashEffectPoolLastActiveIndex = 0;
            constantSplashEffectPoolActiveCount = 0;
            onCollisionSplashEffectPoolFirstActiveIndex = 0;
            onCollisionSplashEffectPoolLastActiveIndex = 0;
            onCollisionSplashEffectPoolActiveCount = 0;

            if (splashParticleEffectPoolRoot != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(splashParticleEffectPoolRoot.gameObject);
                else
#endif
                    Destroy(splashParticleEffectPoolRoot.gameObject);
            }

            if (!useOnCollisionSplashParticleEffect && !useConstantSplashParticleEffect)
                return;

            splashParticleEffectPoolRoot = new GameObject("SplashParticleEffect Pool For " + GetInstanceID()).transform;
            splashParticleEffectPoolRoot.gameObject.hideFlags = HideFlags.HideInHierarchy;

            if (useOnCollisionSplashParticleEffect)
            {
                Transform onCollisionSplashParticleEffectPoolRoot = new GameObject("OnCollisionSplash").transform;
                onCollisionSplashParticleEffectPoolRoot.parent = splashParticleEffectPoolRoot;

                GameObject splashParticleEffectGameObject = onCollisionSplashParticleEffect.gameObject;
                Vector3 splashParticleEffectPosition = Vector3.zero;
                Quaternion splashParticleEffectRotation = splashParticleEffectGameObject.transform.rotation;

                onCollisionSplashParticleEffectPool = new ParticleSystem[onCollisionSplashParticleEffectPoolSize];

                for (int i = 0; i < onCollisionSplashParticleEffectPoolSize; i++)
                {
                    GameObject psGO = Instantiate(splashParticleEffectGameObject, splashParticleEffectPosition, splashParticleEffectRotation, onCollisionSplashParticleEffectPoolRoot);
                    onCollisionSplashParticleEffectPool[i] = psGO.GetComponent<ParticleSystem>();
                    psGO.SetActive(false);
                }
            }

            if (useConstantSplashParticleEffect)
            {
                Transform constantSplashParticleEffectPoolRoot = new GameObject("ConstantSplash").transform;
                constantSplashParticleEffectPoolRoot.parent = splashParticleEffectPoolRoot;

                GameObject splashParticleEffectGameObject = constantSplashParticleEffect.gameObject;
                Vector3 splashParticleEffectPosition = Vector3.zero;
                Quaternion splashParticleEffectRotation = splashParticleEffectGameObject.transform.rotation;

                constantSplashParticleEffectPool = new ParticleSystem[constantSplashParticleEffectPoolSize];

                for (int i = 0; i < constantSplashParticleEffectPoolSize; i++)
                {
                    GameObject psGO = Instantiate(splashParticleEffectGameObject, splashParticleEffectPosition, splashParticleEffectRotation, constantSplashParticleEffectPoolRoot);
                    constantSplashParticleEffectPool[i] = psGO.GetComponent<ParticleSystem>();
                    psGO.SetActive(false);
                }
            }
        }

        private void SpawnSplashParticleSystem(bool spawnOnCollisionParticleEffect, Vector3 spawnPosition)
        {
            ParticleSystem particleSystem;

            if (spawnOnCollisionParticleEffect)
            {
                if (onCollisionSplashEffectPoolActiveCount == onCollisionSplashParticleEffectPoolSize)
                    return;

                particleSystem = onCollisionSplashParticleEffectPool[onCollisionSplashEffectPoolLastActiveIndex];

                onCollisionSplashEffectPoolActiveCount++;

                onCollisionSplashEffectPoolLastActiveIndex++;
                if (onCollisionSplashEffectPoolLastActiveIndex == onCollisionSplashParticleEffectPoolSize)
                    onCollisionSplashEffectPoolLastActiveIndex = 0;
            }
            else
            {
                if (constantSplashEffectPoolActiveCount == constantSplashParticleEffectPoolSize)
                    return;

                particleSystem = constantSplashParticleEffectPool[constantSplashEffectPoolLastActiveIndex];

                constantSplashEffectPoolActiveCount++;

                constantSplashEffectPoolLastActiveIndex++;
                if (constantSplashEffectPoolLastActiveIndex == constantSplashParticleEffectPoolSize)
                    constantSplashEffectPoolLastActiveIndex = 0;
            }

            particleSystem.transform.position = spawnPosition;
            particleSystem.gameObject.SetActive(true);
            particleSystem.Play(true);
        }

        private void UpdateSplashParticleEffectPool()
        {
            if (useOnCollisionSplashParticleEffect && onCollisionSplashEffectPoolActiveCount > 0)
            {
                ParticleSystem particleSystem = onCollisionSplashParticleEffectPool[onCollisionSplashEffectPoolFirstActiveIndex];
                if (!particleSystem.IsAlive(true))
                {
                    particleSystem.gameObject.SetActive(false);

                    onCollisionSplashEffectPoolActiveCount--;
                    onCollisionSplashEffectPoolFirstActiveIndex++;
                    if (onCollisionSplashEffectPoolFirstActiveIndex == onCollisionSplashParticleEffectPoolSize)
                        onCollisionSplashEffectPoolFirstActiveIndex = 0;
                }
            }

            if (useConstantSplashParticleEffect && constantSplashEffectPoolActiveCount > 0)
            {
                ParticleSystem particleSystem = constantSplashParticleEffectPool[constantSplashEffectPoolFirstActiveIndex];
                if (!particleSystem.IsAlive(true))
                {
                    particleSystem.gameObject.SetActive(false);

                    constantSplashEffectPoolActiveCount--;
                    constantSplashEffectPoolFirstActiveIndex++;
                    if (constantSplashEffectPoolFirstActiveIndex == constantSplashParticleEffectPoolSize)
                        constantSplashEffectPoolFirstActiveIndex = 0;
                }
            }
        }

        private void UpdateConstantRipples()
        {
            if (!constantRipplesUpdateWhenOffscreen && !waterIsVisible)
                return;

            constantRipplesDeltaTime += Time.deltaTime;

            if (constantRipplesDeltaTime >= constantRipplesCurrentInterval)
            {
                int velocitiesCount = velocities.Length;
                int startIndex, endIndex;
                if (useCustomBoundaries)
                {
                    startIndex = 1;
                    endIndex = velocitiesCount - 1;
                }
                else
                {
                    startIndex = 0;
                    endIndex = velocitiesCount;
                }

                List<int> ripplesSourcesIndices;
                if (constantRipplesRandomizeRipplesSourcesPositions)
                {
                    ripplesSourcesIndices = constantRipplesSourcesRandomIndices;
                    ripplesSourcesIndices.Clear();
                    for (int i = 0; i < constantRipplesRandomizeRipplesSourcesCount; i++)
                    {
                        ripplesSourcesIndices.Add(Random.Range(startIndex, endIndex));
                    }
                }
                else
                {
                    ripplesSourcesIndices = constantRipplesSourcesIndices;
                }

                Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;

                for (int i = 0, max = ripplesSourcesIndices.Count; i < max; i++)
                {
                    int index = ripplesSourcesIndices[i];

                    float disturbance = constantRipplesRandomizeDisturbance ?
                Random.Range(constantRipplesMinimumDisturbance, constantRipplesMaximumDisturbance) :
                constantRipplesDisturbance;
                    disturbance *= stiffnessSquareRoot;

                    velocities[index] -= disturbance;

                    if (constantRipplesSmoothDisturbance)
                    {
                        float smoothedDisturbance = disturbance * constantRipplesSmoothFactor;
                        int previousIndex, nextIndex;
                        previousIndex = index - 1;
                        nextIndex = index + 1;

                        if (previousIndex >= startIndex)
                            velocities[previousIndex] -= smoothedDisturbance;
                        if (nextIndex < endIndex)
                            velocities[nextIndex] -= smoothedDisturbance;
                    }

                    if (useConstantSplashParticleEffect)
                    {
                        Vector3 spawnPosition = localToWorldMatrix.MultiplyPoint3x4(vertices[index * 2]) + constantSplashParticleEffectSpawnOffset;
                        SpawnSplashParticleSystem(false, spawnPosition);
                    }
                }

                constantRipplesCurrentInterval = constantRipplesRandomizeInterval ?
                 Random.Range(constantRipplesMinimumInterval, constantRipplesMaximumInterval) :
                 constantRipplesInterval;

                constantRipplesDeltaTime = 0f;
                updateWaterSimulation = true;
            }
        }

        private void AnimateWaterSize()
        {
            if (waterSize != lastWaterSize
             || (useCustomBoundaries
             && (!Mathf.Approximately(firstCustomBoundary, lastFirstCustomBoundary)
             || !Mathf.Approximately(secondCustomBoundary, lastSecondCustomBoundary))))
            {
                float halfWidth = waterSize.x / 2f;
                float halfDeltaHeight = (waterSize.y - lastWaterSize.y) / 2f;
                float xStep;
                float leftmostBoundary;

                if (useCustomBoundaries)
                {
                    xStep = Mathf.Abs(firstCustomBoundary - secondCustomBoundary) / (surfaceVerticesCount - 3);
                    leftmostBoundary = Mathf.Min(firstCustomBoundary, secondCustomBoundary);

                    lastFirstCustomBoundary = firstCustomBoundary;
                    lastSecondCustomBoundary = secondCustomBoundary;
                }
                else
                {
                    xStep = waterSize.x / (surfaceVerticesCount - 1);
                    leftmostBoundary = -waterSize.x / 2f + xStep;
                }

                Vector3 vertexTop = vertices[0];
                Vector3 vertexBottom = vertices[1];

                vertexTop.x = vertexBottom.x = -halfWidth;
                vertexTop.y += halfDeltaHeight;
                vertexBottom.y -= halfDeltaHeight;
                vertices[0] = vertexTop;
                vertices[1] = vertexBottom;

                float xPosition = 0f;
                for (int i = 1, vertexIndex = 2; i < surfaceVerticesCount - 1; i++, vertexIndex += 2)
                {
                    vertexTop = vertices[vertexIndex];
                    vertexBottom = vertices[vertexIndex + 1];

                    float x = xPosition + leftmostBoundary;
                    xPosition += xStep;

                    vertexTop.x = vertexBottom.x = x;
                    vertexTop.y += halfDeltaHeight;
                    vertexBottom.y -= halfDeltaHeight;
                    vertices[vertexIndex] = vertexTop;
                    vertices[vertexIndex + 1] = vertexBottom;
                }

                int lastVertexIndex = (surfaceVerticesCount - 1) * 2;
                vertexTop = vertices[lastVertexIndex];
                vertexBottom = vertices[lastVertexIndex + 1];

                vertexTop.x = vertexBottom.x = halfWidth;
                vertexTop.y += halfDeltaHeight;
                vertexBottom.y -= halfDeltaHeight;
                vertices[lastVertexIndex] = vertexTop;
                vertices[lastVertexIndex + 1] = vertexBottom;

                mesh.SetVertices(vertices);
                mesh.UploadMeshData(false);
                mesh.RecalculateBounds();

                UpdateComponents();
                lastWaterSize = waterSize;
                waterPositionOfRest = halfWidth;
            }
        }

        private List<Vector2> FindIntersectedPolygon()
        {
            //Uses Sutherland–Hodgman algorithm to find the intersected polygon

            clippingInputOutput[0] = waterWorldspaceBounds;
            List<Vector2> inputList = clippingInputOutput[0];
            List<Vector2> outputList = clippingInputOutput[1];

            Vector2 previousClippingPolygonVertex = cameraWorldSpaceBounds[3];
            for (int i = 0; i < 4; i++)
            {
                outputList.Clear();
                Vector2 currentClippingPolygonVertex = cameraWorldSpaceBounds[i];
                int inputListCount = inputList.Count;
                if (inputListCount == 0)
                    break;
                Vector2 previousSubjectPolygonVertex = inputList[inputListCount - 1];
                bool isPreviousSubjectPolygonVertexInsideClipEdge = IsVertexInsideClipEdge(previousSubjectPolygonVertex, previousClippingPolygonVertex, currentClippingPolygonVertex);
                for (int j = 0; j < inputListCount; j++)
                {
                    Vector2 currentSubjectPoygonVertex = inputList[j];
                    bool isCurrentSubjectPolygonVertexInsideClipEdge = IsVertexInsideClipEdge(currentSubjectPoygonVertex, previousClippingPolygonVertex, currentClippingPolygonVertex);
                    if (isCurrentSubjectPolygonVertexInsideClipEdge)
                    {
                        if (!isPreviousSubjectPolygonVertexInsideClipEdge)
                        {
                            Vector2? intersectionPoint = FindIntersection(previousSubjectPolygonVertex, currentSubjectPoygonVertex, previousClippingPolygonVertex, currentClippingPolygonVertex);
                            if (intersectionPoint != null)
                            {
                                outputList.Add(intersectionPoint.Value);
                            }
                        }
                        outputList.Add(currentSubjectPoygonVertex);
                    }
                    else
                    {
                        if (isPreviousSubjectPolygonVertexInsideClipEdge)
                        {
                            Vector2? intersectionPoint = FindIntersection(previousSubjectPolygonVertex, currentSubjectPoygonVertex, previousClippingPolygonVertex, currentClippingPolygonVertex);
                            if (intersectionPoint != null)
                            {
                                outputList.Add(intersectionPoint.Value);
                            }
                        }
                    }
                    previousSubjectPolygonVertex = currentSubjectPoygonVertex;
                    isPreviousSubjectPolygonVertexInsideClipEdge = isCurrentSubjectPolygonVertexInsideClipEdge;
                }
                previousClippingPolygonVertex = currentClippingPolygonVertex;
                //Swap input output lists
                List<Vector2> temp = inputList;
                inputList = outputList;
                outputList = temp;
            }

            return clippingInputOutput[0];
        }

        private static Vector2? FindIntersection(Vector2 subjectEdgeStart, Vector2 subjectEdgeEnd, Vector2 clipEdgeStart, Vector2 clipEdgeEnd)
        {
            Vector2 subjectEdgeEndStart = subjectEdgeEnd - subjectEdgeStart;
            Vector2 clipEdgeEndStart = clipEdgeEnd - clipEdgeStart;
            double dotProduct = (subjectEdgeEndStart.x * clipEdgeEndStart.y) - (subjectEdgeEndStart.y * clipEdgeEndStart.x);
            if (System.Math.Abs(dotProduct) <= .000000001d)
            {
                //lines are parallel
                return null;
            }
            Vector2 subjectEdgeStartClipEdgeStart = clipEdgeStart - subjectEdgeStart;
            double delta = (subjectEdgeStartClipEdgeStart.x * clipEdgeEndStart.y - subjectEdgeStartClipEdgeStart.y * clipEdgeEndStart.x) / dotProduct;
            Vector2 intersectionPoint = subjectEdgeStart + (float)delta * subjectEdgeEndStart;
            return intersectionPoint;
        }

        private static bool IsVertexInsideClipEdge(Vector2 vertex, Vector2 clipEdgeStart, Vector2 clipEdgeEnd)
        {
            return (clipEdgeEnd.x - clipEdgeStart.x) * (vertex.y - clipEdgeStart.y) - (vertex.x - clipEdgeStart.x) * (clipEdgeEnd.y - clipEdgeStart.y) <= 0f;
        }

#endregion
    }
}
