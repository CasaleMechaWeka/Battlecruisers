namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    public interface IPvPBoostProvider
    {
        // < 1 to reduce performance, > 1 to improve performance, 1 by default
        float BoostMultiplier { get; }

        void AddBoostConsumer(IPvPBoostConsumer boostConsumer);
        void RemoveBoostConsumer(IPvPBoostConsumer boostConsumer);
    }
}
