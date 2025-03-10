namespace BattleCruisers.UI.Common.Click
{
    public interface IDoubleClickHandler<TClickTarget>
    {
        void OnDoubleClick(TClickTarget clickTarget);
    }
}