using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    public interface ISlotsProvider :
        IListProvider<ISlot>,
        IListProvider<IHighlightable>,
        IListProvider<IClickableEmitter>
    {
    }
}
