namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface IPvPAssignerFactory
    {
        IPvPAssigner CreateAssigner(int numOfOptions);
    }
}
