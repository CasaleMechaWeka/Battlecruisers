using BattleCruisers.AI.Providers;

namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public interface IPrefabKeyWrapper
    {
        bool HasKey { get; }
        IPrefabKey Key { get; }

        void Initialise(IBuildOrders buildOrders);
    }
}
