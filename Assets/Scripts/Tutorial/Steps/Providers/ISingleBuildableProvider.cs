using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Steps.ClickSteps;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public interface ISingleBuildableProvider :
        IProvider<IBuildable>,
        IHighlightablesProvider,
        IClickablesProvider
    {
    }
}
