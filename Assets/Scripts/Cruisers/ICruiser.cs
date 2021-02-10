using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.UI;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
    public interface ICruiser : ICruiserController, ITarget, IComparableItem, IClickableEmitter
    {
        IBuildableWrapper<IBuilding> SelectedBuildingPrefab { get; set; }
        IDroneConsumerProvider DroneConsumerProvider { get; }
        Direction Direction { get; }
        float YAdjustmentInM { get; }
        Vector2 TrashTalkScreenPosition { get; }
        IGameObject Fog { get; }
        IRepairManager RepairManager { get; }
        int NumOfDrones { get; }
        IBuildProgressCalculator BuildProgressCalculator { get; }
        IFactoryProvider FactoryProvider { get; }
        ICruiserSpecificFactories CruiserSpecificFactories { get; }
        bool IsPlayerCruiser { get; }
        CruiserDeathExplosion DeathPrefab { get; }

        IBuilding ConstructSelectedBuilding(ISlot slot);
        void MakeInvincible();
    }
}
