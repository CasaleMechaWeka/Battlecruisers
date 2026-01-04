using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class ShieldController : Target
    {
        private float _timeSinceDamageInS;
        private IDebouncer _takeDamageSoundDebouncer;

        public GameObject visuals;
        public CircleCollider2D circleCollider;
        public HealthBarController healthBar;
        private List<Collider2D> protectedColliders;

        private const int NUM_OF_POINTS_IN_RING = 100;
        private const float HEALTH_BAR_Y_POSITION_MULTIPLIER = 1.2f;
        private const float SHIELD_RADIUS_TO_HEALTH_BAR_WIDTH_MULTIPLIER = 1.6f;
        private const float HEALTH_BAR_WIDTH_TO_HEIGHT_MULTIPLIER = 0.025f;

        public IShieldStats Stats { get; private set; }
        public override TargetType TargetType => TargetType.Buildings;

        private Vector2 _size;
        public override Vector2 Size => _size;
        private int shieldUpdateCnt = 0;

        // Serialized fields for manual offset override
        [SerializeField]
        private bool manualOffsetOverride = false; // Checkbox to enable manual override

        [SerializeField]
        private Vector2 manualOffset = Vector2.zero; // Manual offset values

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            Helper.AssertIsNotNull(visuals, circleCollider, healthBar);

            Stats = GetComponent<IShieldStats>();
            Assert.IsNotNull(Stats);

            float diameter = 2 * Stats.ShieldRadiusInM;
            _size = new Vector2(diameter, diameter);

            _takeDamageSoundDebouncer = new Debouncer(TimeBC.Instance.TimeSinceGameStartProvider, debounceTimeInS: 0.5f);
        }

        public void Initialise(Faction faction)
        {
            Faction = faction;

            _timeSinceDamageInS = 1000;
            circleCollider.radius = Stats.ShieldRadiusInM;

            SetupHealthBar();
        }

        private void SetupHealthBar()
        {
            healthBar.Initialise(this);

            float yPos = HEALTH_BAR_Y_POSITION_MULTIPLIER * Stats.ShieldRadiusInM;
            healthBar.Offset = new Vector2(0, yPos);

            /*

            float width = SHIELD_RADIUS_TO_HEALTH_BAR_WIDTH_MULTIPLIER * Stats.ShieldRadiusInM;
            float height = HEALTH_BAR_WIDTH_TO_HEIGHT_MULTIPLIER * width;
            healthBar.UpdateSize(width, height);

            */
        }

        // PERF:  Don't need to do this every frame
        void FixedUpdate()
        {
            // Eat into recharge delay
            if (Health < maxHealth)
            {
                _timeSinceDamageInS += _time.DeltaTime;

                // Heal
                if (_timeSinceDamageInS >= Stats.ShieldRechargeDelayInS)
                {
                    if (IsDestroyed)
                    {
                        EnableShield();
                    }

                    RepairCommandExecute(Stats.ShieldRechargeRatePerS * _time.DeltaTime);

                    if (Health == maxHealth)
                    {
                        _ = SoundPlayer.PlaySoundAsync(SoundKeys.Shields.FullyCharged, Position);
                    }
                }
            }
            shieldUpdateCnt++;
            shieldUpdateCnt %= 100;
            if (shieldUpdateCnt == 0)
            {
                UpdateBuildingImmunity(circleCollider.enabled);
            }
        }

        protected override void OnHealthGone()
        {
            DisableShield();
            InvokeDestroyedEvent();
            _timeSinceDamageInS = 0;
        }

        protected override void OnTakeDamage()
        {
            _takeDamageSoundDebouncer.Debounce(PlayDamagedSound);
        }

        private void UpdateBuildingImmunity(bool boo)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5.0f);

            foreach (Collider2D c2d in colliders)
            {
                if (c2d.gameObject.layer == 15)
                {
                    // Check if the center of the collider is within the circle
                    Vector2 circleCenter = transform.position;
                    Vector2 colliderCenter = c2d.bounds.center;
                    //float colliderRadius = c2d.bounds.extents.magnitude;
                    if ((colliderCenter - circleCenter).sqrMagnitude <= 25.0f) // 5.0f * 5.0f
                    {
                        ITarget target = c2d.gameObject.GetComponent<ITargetProxy>()?.Target;
                        if (target != null)
                        {
                            target.SetBuildingImmunity(boo);
                        }
                    }
                }
            }
        }

        private void PlayDamagedSound()
        {
            _ = SoundPlayer.PlaySoundAsync(SoundKeys.Shields.HitWhileActive, Position);
        }

        private void EnableShield()
        {
            visuals.SetActive(true);
            circleCollider.enabled = true;
            UpdateBuildingImmunity(true);
        }

        private void DisableShield()
        {
            visuals.SetActive(false);
            circleCollider.enabled = false;
            _ = SoundPlayer.PlaySoundAsync(SoundKeys.Shields.FullyDepleted, Position);
            UpdateBuildingImmunity(false);
        }

        public override bool IsShield()
        {
            return true;
        }

        public virtual void ApplyVariantStats(IBuilding building)
        {
            int variantIndex = building.variantIndex;
            Debug.Log(variantIndex);
            if (variantIndex != -1)
            {
                VariantPrefab variant = PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(variantIndex));
                StatVariant statVariant = variant.statVariant;
                maxHealth += statVariant.shield_health;
                Stats.shieldRechargeDelayModifier += statVariant.shield_recharge_delay;
                Stats.shieldRechargeRateModifier += statVariant.shield_recharge_rate;
            }
        }

        // UpdatePosition method to adjust position based on offset
        private void UpdatePosition()
        {
            Vector3 parentPosition = transform.position;
            Vector2 offsetToUse = manualOffsetOverride ? manualOffset : healthBar.Offset;

            Vector3 newPosition = new Vector3(
                parentPosition.x + offsetToUse.x,
                parentPosition.y + offsetToUse.y,
                transform.position.z);

            Debug.Log($"UpdatePosition: {gameObject.name} ParentPosition: {parentPosition}, Offset: {offsetToUse}, NewPosition: {newPosition}");

            healthBar.transform.position = newPosition;
        }
    }
}
