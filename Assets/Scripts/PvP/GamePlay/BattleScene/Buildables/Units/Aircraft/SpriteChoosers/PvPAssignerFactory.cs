using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public class PvPAssignerFactory : IPvPAssignerFactory
    {
        public IAssigner CreateAssigner(int numOfOptions)
        {
            return new PvPLinearProportionAssigner(numOfOptions);
        }
    }
}
