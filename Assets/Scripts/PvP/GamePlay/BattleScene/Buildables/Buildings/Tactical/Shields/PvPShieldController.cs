using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical.Shields
{
    public class PvPShieldController : PvPTarget
    {
        private IPvPSoundPlayer _soundPlayer;
        private float _timeSinceDamageInS;
        private IPvPDebouncer _takeDamageSoundDebouncer;

        public GameObject visuals;
        public CircleCollider2D circleCollider;
        public PvPHealthBarController healthBar;
        private List<Collider2D> protectedColliders;

        private const int NUM_OF_POINTS_IN_RING = 100;
        private const float HEALTH_BAR_Y_POSITION_MULTIPLIER = 1.2f;
        private const float SHIELD_RADIUS_TO_HEALTH_BAR_WIDTH_MULTIPLIER = 1.6f;
        private const float HEALTH_BAR_WIDTH_TO_HEIGHT_MULTIPLIER = 0.025f;

        public IPvPShieldStats Stats { get; private set; }
        public override PvPTargetType TargetType => PvPTargetType.Buildings;

        private Vector2 _size;
        public override Vector2 Size => _size;
        private int shieldUpdateCnt = 0;

        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);

            PvPHelper.AssertIsNotNull(visuals, circleCollider, healthBar);

            Stats = GetComponent<IPvPShieldStats>();
            Assert.IsNotNull(Stats);

            float diameter = 2 * Stats.ShieldRadiusInM;
            _size = new Vector2(diameter, diameter);

            _takeDamageSoundDebouncer = new PvPDebouncer(PvPTimeBC.Instance.TimeSinceGameStartProvider, debounceTimeInS: 0.5f);
        }

        public void Initialise(PvPFaction faction, IPvPSoundPlayer soundPlayer)
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

            float width = SHIELD_RADIUS_TO_HEALTH_BAR_WIDTH_MULTIPLIER * Stats.ShieldRadiusInM;
            float height = HEALTH_BAR_WIDTH_TO_HEIGHT_MULTIPLIER * width;
            healthBar.UpdateSize(width, height);
        }

        // PERF:  Don't need to do this every frame
        void Update()
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
                        //    _soundPlayer.PlaySoundAsync(PvPSoundKeys.Shields.FullyCharged, Position);
                        //   OnPlayFullchargedSoundClientRpc(Position);
                        OnPlaySoundClientRpc(PvPSoundKeys.Shields.FullyCharged.Type, PvPSoundKeys.Shields.FullyCharged.Name, Position);
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
            ////
        }

        private void PlayDamagedSound()
        {
            //  _soundPlayer.PlaySoundAsync(PvPSoundKeys.Shields.HitWhileActive, Position);
            //  OnPlayTakeDamageSoundClientRpc(Position);
            OnPlaySoundClientRpc(PvPSoundKeys.Shields.HitWhileActive.Type, PvPSoundKeys.Shields.HitWhileActive.Name, Position);
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
            OnPlaySoundClientRpc(PvPSoundKeys.Shields.FullyDepleted.Type, PvPSoundKeys.Shields.FullyDepleted.Name, Position);
            UpdateBuildingImmunity(false);
        }

        public override bool IsShield()
        {
            return true;
        }

        [ClientRpc]
        private void OnPlaySoundClientRpc(PvPSoundType soundType, string soundName, Vector3 position)
        {
            if (IsClient)
                PvPBattleSceneGodClient.Instance.factoryProvider.Sound.SoundPlayer.PlaySoundAsync(new PvPSoundKey(soundType, soundName), position);
        }

    }
}
