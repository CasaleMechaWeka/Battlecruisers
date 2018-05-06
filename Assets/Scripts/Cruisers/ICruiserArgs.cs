using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserArgs
    {
        Faction Faction { get; }
        ICruiser EnemyCruiser { get; }
        IUIManager UiManager { get; }
        IDroneManager DroneManager { get; }
        IDroneConsumerProvider DroneConsumerProvider { get; }
        IFactoryProvider FactoryProvider { get; }
        Direction FacingDirection { get; }
        RepairManager RepairManager { get; }
        bool ShouldShowFog { get; }
        ICruiserHelper Helper { get; }
        ISlotFilter HighlightableFilter { get; }
    }
}
