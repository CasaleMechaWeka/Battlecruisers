using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings.Tactical.Shields
{
    public class SectorShieldController : Target
    {
        private ISoundPlayer _soundPlayer;
        private float _timeSinceDamageInS;
        private IDebouncer _takeDamageSoundDebouncer;

        public GameObject visuals;
        public PolygonCollider2D polygonCollider;
        public HealthBarController healthBar;
        private List<Collider2D> protectedColliders;

        public IShieldStats Stats { get; private set; }
        public override TargetType TargetType => TargetType.Buildings;

        private Vector2 _size;
        public override Vector2 Size => _size;

        private int shieldUpdateCnt = 0;

        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);

            Helper.AssertIsNotNull(visuals, polygonCollider, healthBar);

            Stats = GetComponent<IShieldStats>();
            Assert.IsNotNull(Stats);

            // Since we're not using a radius, we will assume the size is set directly
            _size = new Vector2(polygonCollider.bounds.size.x, polygonCollider.bounds.size.y);

            _takeDamageSoundDebouncer = new Debouncer(TimeBC.Instance.TimeSinceGameStartProvider, debounceTimeInS: 0.5f);
        }

        public void Initialise(Faction faction, ISoundPlayer soundPlayer)
        {
            Faction = faction;

            _soundPlayer = soundPlayer;
            _timeSinceDamageInS = 1000;

            SetupHealthBar();
        }

        private void SetupHealthBar()
        {
            healthBar.Initialise(this);

            // Adjust the health bar position and size manually in the Unity Editor as needed
            // Since the resizing logic related to the shield's radius is removed
        }

        void Update()
        {
            if (Health < maxHealth)
            {
                _timeSinceDamageInS += _time.DeltaTime;

                if (_timeSinceDamageInS >= Stats.ShieldRechargeDelayInS)
                {
                    if (IsDestroyed)
                    {
                        EnableShield();
                    }

                    RepairCommandExecute(Stats.ShieldRechargeRatePerS * _time.DeltaTime);

                    if (Health == maxHealth)
                    {
                        _soundPlayer.PlaySoundAsync(SoundKeys.Shields.FullyCharged, Position);
                    }
                }
            }
            shieldUpdateCnt++;
            shieldUpdateCnt %= 100;
            if (shieldUpdateCnt == 0)
            {
                UpdateBuildingImmunity(polygonCollider.enabled);
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
            // The logic here remains the same, but ensure it aligns with how your PolygonCollider2D is set up
            Collider2D[] colliders = Physics2D.OverlapAreaAll(polygonCollider.bounds.min, polygonCollider.bounds.max);

            foreach (Collider2D c2d in colliders)
            {
                if (c2d.gameObject.layer == 15)
                {
                    ITarget target = c2d.gameObject.GetComponent<ITargetProxy>()?.Target;
                    target.SetBuildingImmunity(boo);
                }
            }
        }

        private void PlayDamagedSound()
        {
            _soundPlayer.PlaySoundAsync(SoundKeys.Shields.HitWhileActive, Position);
        }

        private void EnableShield()
        {
            visuals.SetActive(true);
            polygonCollider.enabled = true;
            UpdateBuildingImmunity(true);
        }

        private void DisableShield()
        {
            visuals.SetActive(false);
            polygonCollider.enabled = false;
            _soundPlayer.PlaySoundAsync(SoundKeys.Shields.FullyDepleted, Position);
            UpdateBuildingImmunity(false);
        }

        public override bool IsShield()
        {
            return true;
        }

        public virtual async void ApplyVariantStats(IBuilding building)
        {
            // This method remains unchanged
            int variantIndex = building.variantIndex;
            Debug.Log(variantIndex);
            if (variantIndex != -1)
            {
                VariantPrefab variant = await BattleSceneGod.Instance.factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(variantIndex));
                StatVariant statVariant = variant.statVariant;
                maxHealth += statVariant.shield_health;
                Stats.shieldRechargeDelayModifier += statVariant.shield_recharge_delay;
                Stats.shieldRechargeRateModifier += statVariant.shield_recharge_rate;
            }
        }
    }
}
