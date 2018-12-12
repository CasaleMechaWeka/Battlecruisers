using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps.Providers
{
    // TUTORIAL  Remove?  Hmm, used in one place?  But is it necessary???
    public interface ISlotsProvider :
        IListProvider<ISlot>,
        IListProvider<IHighlightable>,
        IListProvider<IClickableEmitter>
    {
    }
}
