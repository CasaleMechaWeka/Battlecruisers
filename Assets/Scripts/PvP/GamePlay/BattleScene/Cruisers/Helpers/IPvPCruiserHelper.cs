using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildable.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers
{
    public interface ICruiserHelper
    {
        void FocusCameraOnCruiser();
        void OnBuildingConstructionStarted(IPvPBuilding buildingStarted, IPvPSlotAccessor slotAccessor, IPvPSlotHighlighter slotHighlighter);
    }
}

