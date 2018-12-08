using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public interface ISlotProvider :
        IItemProvider<ISlot>,
        IItemProvider<IHighlightable>,
        IItemProvider<IClickableEmitter>
    {
    }
}
