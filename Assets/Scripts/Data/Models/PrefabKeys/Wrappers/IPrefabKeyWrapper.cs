using BattleCruisers.AI.BuildOrders;

namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public interface IPrefabKeyWrapper
    {
        bool HasKey { get; }
        BuildingKey Key { get; }

        void Initialise(IBuildOrders buildOrders);
    }
}
