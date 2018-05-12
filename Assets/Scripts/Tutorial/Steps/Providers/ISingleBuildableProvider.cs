using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public interface ISingleBuildableProvider :
        IProvider<IBuildable>,
        IListProvider<IHighlightable>,
        IListProvider<IClickable>
    {
    }
}
