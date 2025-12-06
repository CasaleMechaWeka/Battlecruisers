using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Tactical
{
    public class PvPControlTowerController : PvPTacticalBuilding
    {
        private IBoostProvider _boostProvider;

        public float boostMultiplier;

        public override void Initialise()
        {
            base.Initialise();
            _boostProvider = new BoostProvider(boostMultiplier);
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                _cruiserSpecificFactories.GlobalBoostProviders.AircraftBoostProviders.Add(_boostProvider);
                OnBuildableCompletedClientRpc();
            }
            else
                OnBuildableCompleted_PvPClient();
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            _cruiserSpecificFactories.GlobalBoostProviders.AircraftBoostProviders.Remove(_boostProvider);
        }
    }
}
