namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    public interface IPvPBoostFactory
    {
        IPvPBoostConsumer CreateBoostConsumer();
        IPvPBoostProvider CreateBoostProvider(float boostMultiplier);
        IPvPBoostableGroup CreateBoostableGroup();
        IPvPBoostable CreateBoostable();
    }
}