namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public abstract class PvPSatelliteController : PvPAircraftController
    {
        public override void StartConstruction()
        {
            base.StartConstruction();

            // Satellites insta-complete
            OnBuildableCompleted();
        }
    }
}
