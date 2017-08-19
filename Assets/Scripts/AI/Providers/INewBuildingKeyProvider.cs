using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    // FELIX  Rename
    // FELIX  Delete :P  Unused :(
    public interface INewBuildingKeyProvider
    {
        bool HasNext { get; }
        IPrefabKey Next { get; }
    }
}
