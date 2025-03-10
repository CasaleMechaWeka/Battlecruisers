namespace BattleCruisers.UI.BattleScene.Presentables
{
    public interface IPresentableComponent : IPresentable
    {
        void AddChildPresentable(IPresentable presentableToAdd);
    }
}