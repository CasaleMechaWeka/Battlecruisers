using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Represents an individual cruiser section that can be independently targeted and selected.
    /// Each section has its own collider, sprite renderer, and building slots.
    /// Works with any Cruiser (single-section or multi-section).
    /// </summary>
    public class CruiserSection : MonoBehaviour, ITarget
    {
        [Header("Cruiser Section Configuration")]
        [Tooltip("Reference to the parent Cruiser this section belongs to")]
        public Cruiser ParentCruiser;

        [Tooltip("Unique identifier for this cruiser section")]
        public string HullId;

        [Tooltip("If true, destroying this hull ends the battle (victory for player)")]
        public bool IsPrimary = false;

        [Tooltip("Sprite renderer for this cruiser section")]
        public SpriteRenderer SpriteRenderer;

        [Tooltip("Primary collider for this cruiser section (used for targeting)")]
        public PolygonCollider2D PrimaryCollider;

        [Header("Health Configuration")]
        [Tooltip("Maximum health for this cruiser section")]
        public float maxHealth = 1000f;

        [Tooltip("Health regeneration rate per drone per second")]
        public float healthGainPerDroneS = 0.667f;

        [Header("Death Configuration")]
        [Tooltip("Explosion prefab spawned when this hull is destroyed")]
        public CruiserDeathExplosion DeathPrefab;


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

        public float MaxHealth => maxHealth;
        public float Health => _healthTracker?.Health ?? maxHealth;
        public bool IsDestroyed => _isDestroyed;

        // Additional ITarget interface members
        public GameObject GameObject => gameObject;
        public ITarget LastDamagedSource => _lastDamagedSource;

        // IRepairable members
        public float HealthGainPerDroneS => healthGainPerDroneS;
        public RepairCommand RepairCommand => ParentCruiser?.RepairCommand;

        // Click handling
        private ClickHandler _clickHandler;
        private ClickHandlerWrapper _clickHandlerWrapper;

        // Health and death handling
        private HealthTracker _healthTracker;
        private bool _isDestroyed = false;
        private ITarget _lastDamagedSource;

        public void Initialize()
        {
            if (ParentCruiser == null)
            {
                Debug.LogError($"[CruiserSection] {name}: ParentCruiser is null! Cannot initialize.");
                return;
            }

            // Set up health tracking (own health tracker)
            _healthTracker = new HealthTracker(maxHealth);
            _healthTracker.HealthGone += OnHealthGone;
            _healthTracker.HealthChanged += OnHealthChangedInternal;

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

            Debug.Log($"[CruiserSection] {HullId} initialized successfully");
        }

        private void OnHealthChangedInternal(object sender, EventArgs e)
        {
            HealthChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnHealthGone(object sender, EventArgs e)
        {
            if (_isDestroyed) return;

            _isDestroyed = true;

            Debug.Log($"[CruiserSection] {HullId} destroyed!");

            // Spawn death explosion
            if (DeathPrefab != null)
            {
                Instantiate(DeathPrefab, transform.position, transform.rotation);
            }

            // Hide this section
            HideSection();

            // Raise destroyed event
            Destroyed?.Invoke(this, new DestroyedEventArgs(this));

            // Notify parent cruiser
            ParentCruiser?.OnHullDestroyed(this);
        }


        private void HideSection()
        {
            // Hide sprite
            if (SpriteRenderer != null)
            {
                SpriteRenderer.enabled = false;
            }

            // Disable collider so it can't be targeted
            if (PrimaryCollider != null)
            {
                PrimaryCollider.enabled = false;
            }
        }

        private void OnSingleClick(object sender, EventArgs e)
        {
            // Forward click to parent cruiser
            ParentCruiser.OnHullClicked(this);
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            // Forward double-click to parent cruiser
            ParentCruiser.OnHullDoubleClicked(this);
        }

        private void OnTripleClick(object sender, EventArgs e)
        {
            // Forward triple-click to parent cruiser for hull-specific targeting
            ParentCruiser.OnHullTripleClicked(this);
        }

        public void TakeDamage(float damageAmount, ITarget damageSource, bool ignoreImmuneStatus = false)
        {
            if (_isDestroyed) return;
            if (IsBuildingImmune() && !ignoreImmuneStatus) return;

            Logging.Log(Tags.TARGET, $"[CruiserSection] {HullId} taking {damageAmount} damage from {damageSource}");

            _lastDamagedSource = damageSource;

            if (_healthTracker.RemoveHealth(damageAmount))
            {
                Damaged?.Invoke(this, new DamagedEventArgs(damageSource));
            }
        }

        public void Destroy()
        {
            if (!_isDestroyed)
            {
                _healthTracker.RemoveHealth(_healthTracker.MaxHealth);
            }
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
            return $"CruiserSection '{HullId}' [{(_isDestroyed ? "DESTROYED" : $"HP: {Health}/{MaxHealth}")}]";
        }


        public void MakeInvincible()
        {
            _healthTracker.State = HealthTrackerState.Immutable;
        }

        public void MakeDamageable()
        {
            _healthTracker.State = HealthTrackerState.Mutable;
        }

        private void OnDestroy()
        {
            // Clean up event handlers
            if (_healthTracker != null)
            {
                _healthTracker.HealthGone -= OnHealthGone;
                _healthTracker.HealthChanged -= OnHealthChangedInternal;
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
