using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils.PlatformAbstractions;
using System.Collections;
using UnityEngine;

namespace BattleCruisers.Buildables
{
    /// <summary>
    /// Non-buildable civilian building that can be placed on cruisers/fortresses.
    /// Takes damage and shows wreckage when destroyed.
    /// 
    /// IMPORTANT: To persist CivBuildings after cruiser death:
    /// 1. Add this script to the CivBuilding GameObject
    /// 2. Add the CivBuilding GameObject to the cruiser's "Persistent Objects" array in the inspector
    /// 3. When the cruiser is destroyed, the CivBuilding will remain in the scene
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class CivBuilding : MonoBehaviour, ITargetProxy
    {
        [Tooltip("Health points for this civilian building")]
        public float maxHealth = 100f;

        [Tooltip("The GameObject with the building sprite (will be deactivated on death)")]
        public GameObject buildingGameObject;

        [Tooltip("The GameObject with the wreckage/death animation (will be activated on death)")]
        public GameObject wreckageGameObject;

        private float _currentHealth;
        private bool _isDestroyed = false;
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            if (_collider != null)
            {
                _collider.isTrigger = true;
            }

            _currentHealth = maxHealth;

            // Ensure wreckage is disabled at start
            if (wreckageGameObject != null)
            {
                wreckageGameObject.SetActive(false);
            }

            // Ensure building is enabled at start
            if (buildingGameObject != null)
            {
                buildingGameObject.SetActive(true);
            }
        }

        // ITargetProxy implementation
        public ITarget Target => new CivBuildingTarget(this);

        /// <summary>
        /// Apply damage to this civilian building
        /// </summary>
        public void TakeDamage(float damageAmount)
        {
            if (_isDestroyed)
                return;

            _currentHealth -= damageAmount;

            if (_currentHealth <= 0)
            {
                StartCoroutine(DestroyBuilding());
            }
        }

        /// <summary>
        /// Coroutine to destroy the building with a small delay
        /// </summary>
        private IEnumerator DestroyBuilding()
        {
            _isDestroyed = true;

            // Random pause between 0 and 0.2 seconds
            float delay = Random.Range(0f, 0.2f);
            yield return new WaitForSeconds(delay);

            // Deactivate building sprite
            if (buildingGameObject != null)
            {
                buildingGameObject.SetActive(false);
            }

            // Activate wreckage
            if (wreckageGameObject != null)
            {
                wreckageGameObject.SetActive(true);
            }

            // Disable collider so it can't take more damage
            if (_collider != null)
            {
                _collider.enabled = false;
            }
        }

        /// <summary>
        /// Inner class that implements ITarget for damage handling
        /// </summary>
        private class CivBuildingTarget : ITarget
        {
            private readonly CivBuilding _civBuilding;

            public CivBuildingTarget(CivBuilding civBuilding)
            {
                _civBuilding = civBuilding;
            }

            public void TakeDamage(float damageAmount, ITarget damageSource, bool ignoreImmuneStatus = false)
            {
                _civBuilding?.TakeDamage(damageAmount);
            }

            // ITarget implementation - minimal properties needed for civilian buildings
            public bool IsDestroyed => _civBuilding?._isDestroyed ?? true;
            public Faction Faction => Faction.Reds; // Civilian buildings are neutral/enemy
            public GameObject GameObject => _civBuilding?.gameObject;
            public TargetType TargetType => TargetType.Buildings;
            public TargetValue TargetValue => TargetValue.Low;
            public Vector2 Velocity => Vector2.zero;
            public Vector2 Size => _civBuilding?._collider?.bounds.size ?? Vector2.one;
            public Vector2 DroneAreaSize => Size;
            public ITransform Transform => null;
            public Vector2 DroneAreaPosition => Position;
            public Vector2 Position
            {
                get => _civBuilding?.transform.position ?? Vector2.zero;
                set { }
            }
            public Quaternion Rotation
            {
                get => _civBuilding?.transform.rotation ?? Quaternion.identity;
                set { }
            }
            public Color Color { set { } }
            public bool IsInScene => _civBuilding?.gameObject.scene.isLoaded ?? false;
            public float Health => _civBuilding?._currentHealth ?? 0f;
            public float HealthGainPerDroneS => 0f;
            public RepairCommand RepairCommand => null;
            public float MaxHealth => _civBuilding?.maxHealth ?? 0f;
            public System.Collections.ObjectModel.ReadOnlyCollection<TargetType> AttackCapabilities => null;
            public ITarget LastDamagedSource => null;
            public System.Action clickedRepairButton { get; set; }
            public event System.EventHandler<DestroyedEventArgs> Destroyed { add { } remove { } }
            public event System.EventHandler<DamagedEventArgs> Damaged { add { } remove { } }
            public event System.EventHandler HealthChanged { add { } remove { } }
            public void Destroy() => _civBuilding?.TakeDamage(_civBuilding.maxHealth);
            public HighlightArgs CreateHighlightArgs(HighlightArgsFactory highlightArgsFactory) => null;
            public bool IsShield() => false;
            public void SetBuildingImmunity(bool boo) { }
            public bool IsBuildingImmune() => false;
        }
    }
}

