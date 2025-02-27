using BattleCruisers.AI.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks
{
    public class PvPDelayProvider : IDelayProvider
    {
        public float DelayInS { get; set; }

        public PvPDelayProvider(float initialDelayInS)
        {
            DelayInS = initialDelayInS;
        }
    }
}