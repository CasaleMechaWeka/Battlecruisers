namespace BattleCruisers.UI.BattleScene.Presentables
{
    public interface IPresentableComponent : IPresentable
    {
        bool IsPresented { get; }

        void AddChildPresentable(IPresentable presentableToAdd);
    }
}