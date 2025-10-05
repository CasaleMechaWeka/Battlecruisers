using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Buildables
{
    /// <summary>
    /// Weak point component for cruisers.
    /// Acts as a damage proxy - when hit, applies modified damage to parent cruiser.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Core : MonoBehaviour, ITargetProxy
    {
        [Tooltip("Additional damage to apply when this core is hit")]
        public float additionalDamage = 0f;

        [Tooltip("Damage multiplier when this core is hit (1.0 = normal damage, 2.0 = double damage)")]
        public float damageMultiplier = 1.0f;

        private ICruiser _parentCruiser;
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            if (_collider != null)
            {
                _collider.isTrigger = true;
            }
        }

        private void Start()
        {
            _parentCruiser = GetComponentInParent<ICruiser>();
        }

        // ITargetProxy implementation - this makes the Core act as a proxy for the parent cruiser
        public ITarget Target => new CoreTarget(_parentCruiser, additionalDamage, damageMultiplier);

        /// <summary>
        /// Inner class that wraps the parent cruiser and modifies damage
        /// </summary>
        private class CoreTarget : ITarget
        {
            private readonly ICruiser _cruiser;
            private readonly float _additionalDamage;
            private readonly float _damageMultiplier;

            public CoreTarget(ICruiser cruiser, float additionalDamage, float damageMultiplier)
            {
                _cruiser = cruiser;
                _additionalDamage = additionalDamage;
                _damageMultiplier = damageMultiplier;
            }

            public void TakeDamage(float damageAmount, ITarget damageSource, bool ignoreImmuneStatus = false)
            {
                if (_cruiser == null || _cruiser.IsDestroyed)
                    return;

                float totalDamage = damageAmount;

                if (_additionalDamage > 0f)
                    totalDamage += _additionalDamage;

                if (_damageMultiplier != 1.0f)
                    totalDamage *= _damageMultiplier;

                _cruiser.TakeDamage(totalDamage, damageSource, ignoreImmuneStatus);
            }

            // Forward all other ITarget properties to the parent cruiser
            public bool IsDestroyed => _cruiser?.IsDestroyed ?? true;
            public Faction Faction => _cruiser?.Faction ?? Faction.Blues;
            public GameObject GameObject => _cruiser?.GameObject;
            public TargetType TargetType => _cruiser?.TargetType ?? TargetType.Cruiser;
            public TargetValue TargetValue => _cruiser?.TargetValue ?? TargetValue.Low;
            public Vector2 Velocity => _cruiser?.Velocity ?? Vector2.zero;
            public Vector2 Size => _cruiser?.Size ?? Vector2.zero;
            public Vector2 DroneAreaSize => _cruiser?.DroneAreaSize ?? Vector2.zero;
            public ITransform Transform => _cruiser?.Transform;
            public Vector2 DroneAreaPosition => _cruiser?.DroneAreaPosition ?? Vector2.zero;
            public Vector2 Position
            {
                get => _cruiser?.Position ?? Vector2.zero;
                set { if (_cruiser != null) _cruiser.Position = value; }
            }
            public Quaternion Rotation
            {
                get => _cruiser?.Rotation ?? Quaternion.identity;
                set { if (_cruiser != null) _cruiser.Rotation = value; }
            }
            public Color Color
            {
                set { if (_cruiser != null) _cruiser.Color = value; }
            }
            public bool IsInScene => _cruiser?.IsInScene ?? false;
            public float Health => _cruiser?.Health ?? 0f;
            public float HealthGainPerDroneS => _cruiser?.HealthGainPerDroneS ?? 0f;
            public RepairCommand RepairCommand => _cruiser?.RepairCommand;
            public float MaxHealth => _cruiser?.MaxHealth ?? 0f;
            public System.Collections.ObjectModel.ReadOnlyCollection<TargetType> AttackCapabilities => _cruiser?.AttackCapabilities;
            public ITarget LastDamagedSource => _cruiser?.LastDamagedSource;
            public System.Action clickedRepairButton { get; set; }
            public event System.EventHandler<DestroyedEventArgs> Destroyed { add { } remove { } }
            public event System.EventHandler<DamagedEventArgs> Damaged { add { } remove { } }
            public event System.EventHandler HealthChanged { add { } remove { } }
            public void Destroy() => _cruiser?.Destroy();
            public HighlightArgs CreateHighlightArgs(HighlightArgsFactory highlightArgsFactory) => null;
            public bool IsShield() => false;
            public void SetBuildingImmunity(bool boo) { }
            public bool IsBuildingImmune() => false;
        }
    }
}
