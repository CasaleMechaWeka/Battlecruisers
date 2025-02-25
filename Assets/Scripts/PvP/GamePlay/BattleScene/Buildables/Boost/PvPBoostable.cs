using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    public class PvPBoostable : IBoostable
    {
        public float BoostMultiplier { get; set; }

        public PvPBoostable(float initialMultiplier)
        {
            BoostMultiplier = initialMultiplier;
        }
    }
}
