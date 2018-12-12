using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public interface ISlotProvider :
        IItemProvider<ISlot>,
        IItemProvider<IMaskHighlightable>,
        IItemProvider<IClickableEmitter>
    {
    }
}
