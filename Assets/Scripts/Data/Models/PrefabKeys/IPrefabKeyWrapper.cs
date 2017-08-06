using BattleCruisers.AI.Providers;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public interface IPrefabKeyWrapper
    {
        bool HasKey { get; }
        IPrefabKey Key { get; }

        void Initialise(IBuildOrders buildOrders);
    }
}