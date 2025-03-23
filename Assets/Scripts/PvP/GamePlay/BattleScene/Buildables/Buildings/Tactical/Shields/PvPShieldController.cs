using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Timers;
using System.Collections.Generic;
using Unity.Netcode;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields
{
    public class PvPShieldController : PvPTarget
    {
        private ISoundPlayer _soundPlayer;
        private float _timeSinceDamageInS;
        private IDebouncer _takeDamageSoundDebouncer;

        public GameObject visuals;
        public CircleCollider2D circleCollider;
        public PvPHealthBarController healthBar;
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

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            PvPHelper.AssertIsNotNull(visuals, circleCollider, healthBar);

            Stats = GetComponent<IShieldStats>();
            Assert.IsNotNull(Stats);

            float diameter = 2 * Stats.ShieldRadiusInM;
            _size = new Vector2(diameter, diameter);

            _takeDamageSoundDebouncer = new Debouncer(TimeBC.Instance.TimeSinceGameStartProvider, debounceTimeInS: 0.5f);
        }

        public void Initialise(Faction faction, ISoundPlayer soundPlayer)
        {
            Faction = faction;

            _soundPlayer = soundPlayer;
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
                        OnHealthRecoveredClientRpc();
                    }

                    RepairCommandExecute(Stats.ShieldRechargeRatePerS * _time.DeltaTime);

                    if (Health == maxHealth)
                    {
                        //    _soundPlayer.PlaySoundAsync(PvPSoundKeys.Shields.FullyCharged, Position);
                        //   OnPlayFullchargedSoundClientRpc(Position);
                        OnPlaySoundClientRpc(SoundKeys.Shields.FullyCharged.Type, SoundKeys.Shields.FullyCharged.Name, Position);
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
            OnHealthGoneClientRpc();
        }

        protected override void OnTakeDamage()
        {

            _takeDamageSoundDebouncer.Debounce(PlayDamagedSound);
        }

        private void UpdateBuildingImmunity(bool boo)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5);

            //// Added by Anuj
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
                        target.SetBuildingImmunity(boo);
                    }
                }
            }
        }

        private void PlayDamagedSound()
        {
            //  _soundPlayer.PlaySoundAsync(PvPSoundKeys.Shields.HitWhileActive, Position);
            //  OnPlayTakeDamageSoundClientRpc(Position);
            OnPlaySoundClientRpc(SoundKeys.Shields.HitWhileActive.Type, SoundKeys.Shields.HitWhileActive.Name, Position);
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
            // _soundPlayer.PlaySoundAsync(PvPSoundKeys.Shields.FullyDepleted, Position);
            OnPlaySoundClientRpc(SoundKeys.Shields.FullyDepleted.Type, SoundKeys.Shields.FullyDepleted.Name, Position);
            UpdateBuildingImmunity(false);
        }

        public override bool IsShield()
        {
            return true;
        }

        public virtual async void ApplyVariantStats(IPvPBuilding building)
        {
            int variantIndex = building.variantIndex;
            Debug.Log(variantIndex);
            if (variantIndex != -1)
            {
                VariantPrefab variant = await PvPBattleSceneGodClient.Instance.factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(variantIndex));
                StatVariant statVariant = variant.statVariant;
                maxHealth += statVariant.shield_health;
                Stats.shieldRechargeDelayModifier += statVariant.shield_recharge_delay;
                Stats.shieldRechargeRateModifier += statVariant.shield_recharge_rate;
            }
        }

        [ClientRpc]
        private void OnPlaySoundClientRpc(SoundType soundType, string soundName, Vector3 position)
        {
            if (IsClient)
                PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayer.PlaySoundAsync(new SoundKey(soundType, soundName), position);
        }

        [ClientRpc]
        private void OnHealthGoneClientRpc()
        {
            if (IsClient)
                visuals.SetActive(false);
        }

        [ClientRpc]
        private void OnHealthRecoveredClientRpc()
        {
            if (IsClient)
                visuals.SetActive(true);
        }

    }
}
