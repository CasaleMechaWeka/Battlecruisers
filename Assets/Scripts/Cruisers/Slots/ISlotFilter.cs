namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotFilter
    {
        bool IsMatch(ISlot slot);
    }
}
