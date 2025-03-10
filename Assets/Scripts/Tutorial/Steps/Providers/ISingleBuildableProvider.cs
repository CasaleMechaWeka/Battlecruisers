using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.UI;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public interface ISingleBuildableProvider :
        IItemProvider<IBuildable>,
        IItemProvider<IHighlightable>,
        IItemProvider<IClickableEmitter>
    {
    }
}
