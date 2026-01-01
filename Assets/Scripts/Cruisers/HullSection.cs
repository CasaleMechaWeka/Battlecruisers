using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Represents an individual hull section that can be independently targeted and selected.
    /// Each hull section has its own collider, sprite renderer, and building slots.
    /// </summary>
    public class HullSection : MonoBehaviour, ITarget
    {
        [Header("Hull Section Configuration")]
        [Tooltip("Reference to the parent ChainCruiser this hull belongs to")]
        public ChainCruiser ParentCruiser;

        [Tooltip("Unique identifier for this hull section")]
        public string HullId;

        [Tooltip("Sprite renderer for this hull section")]
        public SpriteRenderer SpriteRenderer;

        [Tooltip("Primary collider for this hull section (used for targeting)")]
        public PolygonCollider2D PrimaryCollider;

        [Tooltip("SlotWrapperController managing buildings for this hull section")]
        public SlotWrapperController SlotController;

        // ITarget implementation
        public Faction Faction => ParentCruiser?.Faction ?? Faction.Reds;
        public TargetType TargetType => TargetType.Cruiser;
        public Vector2 Velocity => ParentCruiser?.Velocity ?? Vector2.zero;
        public ReadOnlyCollection<TargetType> AttackCapabilities => ParentCruiser?.AttackCapabilities ?? new ReadOnlyCollection<TargetType>(new TargetType[0]);
        public TargetValue TargetValue => ParentCruiser?.TargetValue ?? TargetValue.Low;
        public Color Color { get => SpriteRenderer?.color ?? Color.white; set { if (SpriteRenderer != null) SpriteRenderer.color = value; } }
        public bool IsInScene => gameObject.scene.IsValid();
        public Vector2 Size => PrimaryCollider?.bounds.size ?? Vector2.zero;
        public ITransform Transform => new TransformBC(transform);

        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public Vector2 DroneAreaPosition => Position;
        public Vector2 DroneAreaSize => Size;

        // Events
        public event EventHandler<DestroyedEventArgs> Destroyed;
        public event EventHandler<DamagedEventArgs> Damaged;
        public event EventHandler HealthChanged;

        // Health tracking - hull sections share the parent cruiser's health
        private IHealthTracker _healthTracker;
        public float MaxHealth => ParentCruiser?.MaxHealth ?? 0;
        public bool IsDestroyed => ParentCruiser?.IsDestroyed ?? true;
        public float Health => ParentCruiser?.Health ?? 0;

        // Additional ITarget interface members
        public GameObject GameObject => gameObject;
        public ITarget LastDamagedSource => ParentCruiser?.LastDamagedSource;

        // IRepairable members
        public float HealthGainPerDroneS => ParentCruiser?.HealthGainPerDroneS ?? 0;
        public RepairCommand RepairCommand => ParentCruiser?.RepairCommand;

        // Click handling
        private ClickHandler _clickHandler;
        private ClickHandlerWrapper _clickHandlerWrapper;

        public void Initialize()
        {
            if (ParentCruiser == null)
            {
                Debug.LogError($"[HullSection] {name}: ParentCruiser is null! Cannot initialize.");
                return;
            }

            // Set up health tracking (shared with parent)
            _healthTracker = ParentCruiser.GetHealthTracker();
            if (_healthTracker != null)
            {
                _healthTracker.HealthChanged += OnHealthChanged;
            }

            // Set up click handling
            _clickHandlerWrapper = GetComponent<ClickHandlerWrapper>();
            if (_clickHandlerWrapper != null)
            {
                _clickHandler = _clickHandlerWrapper.GetClickHandler();
                _clickHandler.SingleClick += OnSingleClick;
                _clickHandler.DoubleClick += OnDoubleClick;
                _clickHandler.TripleClick += OnTripleClick;
            }

            // Validate components
            if (SpriteRenderer == null)
            {
                SpriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (PrimaryCollider == null)
            {
                PrimaryCollider = GetComponent<PolygonCollider2D>();
            }

            if (SlotController == null)
            {
                SlotController = GetComponentInChildren<SlotWrapperController>(includeInactive: true);
            }

            Debug.Log($"[HullSection] {HullId} initialized successfully");
        }

        private void OnHealthChanged(object sender, EventArgs e)
        {
            HealthChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnSingleClick(object sender, EventArgs e)
        {
            // Forward click to parent cruiser
            ParentCruiser.OnHullSectionClicked(this);
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            // Forward double-click to parent cruiser
            ParentCruiser.OnHullSectionDoubleClicked(this);
        }

        private void OnTripleClick(object sender, EventArgs e)
        {
            // Forward triple-click to parent cruiser for hull-specific targeting
            ParentCruiser.OnHullSectionTripleClicked(this);
        }

        public void TakeDamage(float damageAmount, ITarget damageSource, bool ignoreImmuneStatus = false)
        {
            // Forward damage to parent cruiser
            ParentCruiser.TakeDamage(damageAmount, damageSource, ignoreImmuneStatus);
        }

        public void Destroy()
        {
            // Hull sections don't destroy independently - forward to parent
            Debug.LogWarning($"[HullSection] {HullId}: Destroy() called on hull section. Forwarding to parent cruiser.");
            ParentCruiser?.Destroy();
        }

        public bool IsShield()
        {
            return ParentCruiser?.IsShield() ?? false;
        }

        public void SetBuildingImmunity(bool immune)
        {
            ParentCruiser?.SetBuildingImmunity(immune);
        }

        public bool IsBuildingImmune()
        {
            return ParentCruiser?.IsBuildingImmune() ?? false;
        }

        public Action clickedRepairButton { get; set; }

        public HighlightArgs CreateHighlightArgs(HighlightArgsFactory highlightArgsFactory)
        {
            return highlightArgsFactory.CreateForInGameObject(Position, Size);
        }

        public override string ToString()
        {
            return $"HullSection '{HullId}' of {ParentCruiser?.name ?? "Unknown Cruiser"}";
        }

        private void OnDestroy()
        {
            // Clean up event handlers
            if (_healthTracker != null)
            {
                _healthTracker.HealthChanged -= OnHealthChanged;
            }

            if (_clickHandler != null)
            {
                _clickHandler.SingleClick -= OnSingleClick;
                _clickHandler.DoubleClick -= OnDoubleClick;
                _clickHandler.TripleClick -= OnTripleClick;
            }
        }
    }
}
