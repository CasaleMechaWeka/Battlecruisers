namespace BattleCruisers.Tutorial.Providers
{
    public class StaticProvider<TItem> : IProvider<TItem>
    {
        private readonly TItem _item;

        public StaticProvider(TItem item)
        {
            _item = item;
        }

        public TItem FindItem()
        {
            return _item;
        }
    }
}
