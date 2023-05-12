namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Presentables
{
    public interface IPvPPresentableComponent : IPvPPresentable
    {
        void AddChildPresentable(IPvPPresentable presentableToAdd);
    }
}