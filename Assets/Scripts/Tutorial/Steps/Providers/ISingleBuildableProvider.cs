using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public interface ISingleBuildableProvider :
        IItemProvider<IBuildable>,
        IListProvider<IHighlightable>,
        IListProvider<IClickableEmitter>
    {
    }
}
