namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public class PvPAssignerFactory : IPvPAssignerFactory
    {
        public IPvPAssigner CreateAssigner(int numOfOptions)
        {
            return new PvPLinearProportionAssigner(numOfOptions);
        }
    }
}
