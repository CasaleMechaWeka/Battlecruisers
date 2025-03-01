using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface IPvPAssignerFactory
    {
        IAssigner CreateAssigner(int numOfOptions);
    }
}
