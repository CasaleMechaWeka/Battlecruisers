namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    public class PvPBoostable : IPvPBoostable
    {
        public float BoostMultiplier { get; set; }

        public PvPBoostable(float initialMultiplier)
        {
            BoostMultiplier = initialMultiplier;
        }
    }
}
