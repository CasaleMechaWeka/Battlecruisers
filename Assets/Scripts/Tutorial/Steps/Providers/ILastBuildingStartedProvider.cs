using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Highlighting.Providers;
using BattleCruisers.Tutorial.Steps.ClickSteps;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public interface ILastBuildingStartedProvider : 
        IHighlightablesProvider, 
        IClickablesProvider, 
        IProvider<IBuildable>
    {
    }
}
