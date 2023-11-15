namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks
{
    public class PvPDelayProvider : IPvPDelayProvider
    {
        public float DelayInS { get; set; }

        public PvPDelayProvider(float initialDelayInS)
        {
            DelayInS = initialDelayInS;
        }
    }
}