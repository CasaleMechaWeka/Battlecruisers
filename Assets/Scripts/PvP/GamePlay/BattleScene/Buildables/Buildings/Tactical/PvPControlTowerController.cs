using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPControlTowerController : PvPTacticalBuilding
    {
        private IPvPBoostProvider _boostProvider;

        protected override PvPPrioritisedSoundKey ConstructionCompletedSoundKey => PvPPrioritisedSoundKeys.PvPCompleted.PvPBuildings.ControlTower;

        public float boostMultiplier;

        public override void Initialise(IPvPFactoryProvider factoryProvider)
        {
            // base.Initialise(uiManager, factoryProvider);
            _boostProvider = _factoryProvider.BoostFactory.CreateBoostProvider(boostMultiplier);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            _cruiserSpecificFactories.GlobalBoostProviders.AircraftBoostProviders.Add(_boostProvider);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            _cruiserSpecificFactories.GlobalBoostProviders.AircraftBoostProviders.Remove(_boostProvider);
        }
    }
}
