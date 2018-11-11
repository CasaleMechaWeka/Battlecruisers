using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public class BuildableIsInitialisedFilter : IFilter<IBuildable>
    {
        public bool IsMatch(IBuildable element)
        {
            return element.IsInitialised;
        }
    }
}