using BattleCruisers.UI.BattleScene.Presentables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables
{
    public interface IPvPPresentableComponent : IPresentable
    {
        void AddChildPresentable(IPresentable presentableToAdd);
    }
}