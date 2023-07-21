using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Localisation;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings
{
    public abstract class PvPSatelliteLauncherController : PvPBuilding
    {
        private IPvPUnit _satellite;

        public PvPUnitWrapper satellitePrefab;

        protected abstract Vector3 SpawnPositionAdjustment { get; }

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);
            Assert.IsNotNull(satellitePrefab);

            // sava added
            satellitePrefab.UnityObject.StaticInitialise(commonStrings);
        }

        protected override async void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            _satellite = await _factoryProvider.PrefabFactory.CreateUnit(satellitePrefab, /*_uiManager, */_factoryProvider);
            _satellite.Position = transform.position + SpawnPositionAdjustment;
            // pvp
            _satellite.GameObject.GetComponent<NetworkTransform>().Teleport(transform.position + SpawnPositionAdjustment, transform.rotation, transform.localScale);

            _satellite.Activate(
                new PvPBuildableActivationArgs(
                    ParentCruiser,
                    EnemyCruiser,
                    _cruiserSpecificFactories));

            _satellite.StartConstruction();
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            if (BuildableState == PvPBuildableState.Completed)
            {
                _satellite.Destroy();
            }
        }
    }
}
