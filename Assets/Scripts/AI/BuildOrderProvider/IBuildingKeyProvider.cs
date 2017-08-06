using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public interface IBuildingKeyProvider
    {
        IPrefabKey Next();
    }
}