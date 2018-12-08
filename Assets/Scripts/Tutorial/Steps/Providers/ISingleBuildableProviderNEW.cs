using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public interface ISingleBuildableProviderNEW :
        IItemProvider<IBuildable>,
        IItemProvider<IMaskHighlightable>,
        IItemProvider<IClickableEmitter>
    {
    }
}
