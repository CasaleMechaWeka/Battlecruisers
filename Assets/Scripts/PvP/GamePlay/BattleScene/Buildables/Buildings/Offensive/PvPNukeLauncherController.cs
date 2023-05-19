using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Offensive
{
    public class PvPNukeLauncherController : PvPBuilding
    {
        private PvPNukeSpinner _spinner;
        private IPvPNukeStats _nukeStats;
        private PvPNukeController _launchedNuke;

        public PvPSiloHalfController leftSiloHalf, rightSiloHalf;
        public PvPNukeController nukeMissilePrefab;

        private IPvPAudioClipWrapper _nukeImpactSound;
        public AudioClip nukeImpactSound;

        private const float SILO_HALVES_ROTATE_SPEED_IN_M_PER_S = 15;
        private const float SILO_TARGET_ANGLE_IN_DEGREES = 45;
        private static Vector3 NUKE_SPAWN_POSITION_ADJUSTMENT = new Vector3(0, -0.3f, 0);

        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.Ultra;
        public override PvPTargetValue TargetValue => PvPTargetValue.High;

        protected override void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IPvPBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            PvPHelper.AssertIsNotNull(leftSiloHalf, rightSiloHalf, nukeMissilePrefab);

            leftSiloHalf.StaticInitialise();
            rightSiloHalf.StaticInitialise();

            _spinner = gameObject.GetComponentInChildren<PvPNukeSpinner>();
            Assert.IsNotNull(_spinner);
            _spinner.StaticInitialise();

            _nukeStats = GetComponent<PvPNukeProjectileStats>();
            Assert.IsNotNull(_nukeStats);
            AddAttackCapability(PvPTargetType.Cruiser);
            AddDamageStats(new PvPDamageCapability(_nukeStats.Damage, AttackCapabilities));

            Assert.IsNotNull(nukeImpactSound);
            _nukeImpactSound = new PvPAudioClipWrapper(nukeImpactSound);
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(factoryProvider);

            leftSiloHalf.Initialise(_movementControllerFactory, SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);
            rightSiloHalf.Initialise(_movementControllerFactory, SILO_HALVES_ROTATE_SPEED_IN_M_PER_S, SILO_TARGET_ANGLE_IN_DEGREES);

            _spinner.Initialise(_movementControllerFactory);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            _spinner.StartRotating();
            _spinner.StopRotating();
            _spinner.Renderer.enabled = false;

            CreateNuke();

            leftSiloHalf.ReachedDesiredAngle += SiloHalf_ReachedDesiredAngle;

            leftSiloHalf.StartRotating();
            rightSiloHalf.StartRotating();
        }

        private void CreateNuke()
        {
            _launchedNuke = Instantiate(nukeMissilePrefab);

            IPvPTargetFilter targetFilter = _factoryProvider.Targets.FilterFactory.CreateExactMatchTargetFilter(EnemyCruiser);
            _launchedNuke.Initialise(_commonStrings, _factoryProvider);
            _launchedNuke.Activate(
                new PvPTargetProviderActivationArgs<IPvPNukeStats>(
                    transform.position + NUKE_SPAWN_POSITION_ADJUSTMENT,
                    _nukeStats,
                    Vector2.zero,
                    targetFilter,
                    this,
                    _nukeImpactSound,
                    EnemyCruiser));

            // Make nuke face upwards (rotation is set in Initialise() above)
            _launchedNuke.transform.eulerAngles = new Vector3(0, 0, 90);
        }

        private void SiloHalf_ReachedDesiredAngle(object sender, EventArgs e)
        {
            leftSiloHalf.ReachedDesiredAngle -= SiloHalf_ReachedDesiredAngle;
            _launchedNuke.Launch();
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            return new List<SpriteRenderer>()
            {
                transform.FindNamedComponent<SpriteRenderer>("Base"),
                _spinner.Renderer,
                leftSiloHalf.Renderer,
                rightSiloHalf.Renderer
            };
        }
    }
}
