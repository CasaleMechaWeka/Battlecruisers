using BattleCruisers.Buildables;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public interface IBuildableButton : IButton
    {
        IBuildable Buildable { get; }
    }
}
