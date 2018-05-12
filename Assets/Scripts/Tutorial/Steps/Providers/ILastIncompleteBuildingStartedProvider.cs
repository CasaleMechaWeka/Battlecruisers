using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    // FELIX  Remove this interface.  Use ISingleBuildableProvider instead :)
    public interface ILastIncompleteBuildingStartedProvider : 
        IListProvider<IHighlightable>, 
        IListProvider<IClickable>, 
        IProvider<IBuildable>
    {
    }
}
