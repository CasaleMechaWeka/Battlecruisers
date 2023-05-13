using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public interface IPvPBuildableSorterFactory
    {
        IPvPBuildableSorter<IPvPBuilding> CreateBuildingSorter();
        IPvPBuildableSorter<IPvPUnit> CreateUnitSorter();
    }
}
