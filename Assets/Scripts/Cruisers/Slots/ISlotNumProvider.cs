namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotNumProvider
    {
        int GetSlotCount(SlotType type);
    }

    public class CompositeSlotNumProvider : ISlotNumProvider
    {
        private readonly ISlotNumProvider[] _providers;

        public CompositeSlotNumProvider(ISlotNumProvider[] providers)
        {
            _providers = providers;
        }

        public int GetSlotCount(SlotType type)
        {
            int total = 0;
            foreach (var provider in _providers)
            {
                total += provider.GetSlotCount(type);
            }
            return total;
        }
    }
}
