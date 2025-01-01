using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
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

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields
{
    public class PvPSectorShieldController : PvPTarget
    {
        private IPvPSoundPlayer _soundPlayer;
        private float _timeSinceDamageInS;
        private IDebouncer _takeDamageSoundDebouncer;

        public GameObject visuals;
        public PolygonCollider2D polygonCollider;
        public PvPHealthBarController healthBar;
        private List<Collider2D> protectedColliders;

        public IPvPShieldStats Stats { get; private set; }

        private TargetType _targetType;

        public override TargetType TargetType => _targetType;

        private Vector2 _size;
        public override Vector2 Size => _size;
        private int shieldUpdateCnt = 0;

        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);

            PvPHelper.AssertIsNotNull(visuals, polygonCollider, healthBar);

            Stats = GetComponent<IPvPShieldStats>();
            Assert.IsNotNull(Stats);

            float diameter = 2 * Stats.ShieldRadiusInM;
            _size = new Vector2(diameter, diameter);

            _takeDamageSoundDebouncer = new Debouncer(PvPTimeBC.Instance.TimeSinceGameStartProvider, debounceTimeInS: 0.5f);
        }

        public void Initialise(Faction faction, IPvPSoundPlayer soundPlayer, TargetType targetType = TargetType.Buildings)
        {
            _targetType = targetType;

            //otherwise the shield won't recharge / reactivate when used on units
            if (_targetType != TargetType.Buildings)
                Stats.BoostMultiplier = 1f;

            Faction = faction;

            _soundPlayer = soundPlayer;
            _timeSinceDamageInS = 1000;

            SetHealthToMax();
            SetupHealthBar();

            EnableShield();
            OnHealthRecoveredClientRpc();
        }

        private void SetupHealthBar()
        {
            healthBar.Initialise(this);
        }

        // PERF:  Don't need to do this every frame
        void FixedUpdate()
        {
            Debug.Log(IsDestroyed);

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
                        //    _soundPlayer.PlaySoundAsync(SoundKeys.Shields.FullyCharged, Position);
                        //   OnPlayFullchargedSoundClientRpc(Position);
                        OnPlaySoundClientRpc(SoundKeys.Shields.FullyCharged.Type, SoundKeys.Shields.FullyCharged.Name, Position);
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
            //  _soundPlayer.PlaySoundAsync(SoundKeys.Shields.HitWhileActive, Position);
            //  OnPlayTakeDamageSoundClientRpc(Position);
            OnPlaySoundClientRpc(SoundKeys.Shields.HitWhileActive.Type, SoundKeys.Shields.HitWhileActive.Name, Position);
        }

        private void EnableShield()
        {
            if (visuals != null)
                visuals.SetActive(true);
            polygonCollider.enabled = true;
            UpdateBuildingImmunity(true);
        }

        private void DisableShield()
        {
            if (visuals != null)
                visuals.SetActive(false);
            polygonCollider.enabled = false;
            // _soundPlayer.PlaySoundAsync(SoundKeys.Shields.FullyDepleted, Position);
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
        public void ActivateShield()//For the turtles ships
        {
            EnableShield();
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
                if (visuals != null)
                    visuals.SetActive(false);
        }

        [ClientRpc]
        private void OnHealthRecoveredClientRpc()
        {
            if (IsClient)
                if (visuals != null)
                    visuals.SetActive(true);
        }

    }
}
