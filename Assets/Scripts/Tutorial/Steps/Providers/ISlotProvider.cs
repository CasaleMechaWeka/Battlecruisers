using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.UI;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public interface ISlotProvider :
        IItemProvider<ISlot>,
        IItemProvider<IMaskHighlightable>,
        IItemProvider<IClickableEmitter>
    {
    }
}
