using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Buildables.Pools
{
    public class BuildingActivationArgs : BuildableActivationArgs
    {
        public Slot ParentSlot { get; }
        public IDoubleClickHandler<IBuilding> DoubleClickHandler { get; }

        public BuildingActivationArgs(
            ICruiser parentCruiser,
            ICruiser enemyCruiser,
            CruiserSpecificFactories cruiserSpecificFactories,
            Slot parentSlot,
            IDoubleClickHandler<IBuilding> doubleClickHandler)
            : base(parentCruiser, enemyCruiser, cruiserSpecificFactories)
        {
            Helper.AssertIsNotNull(parentSlot, doubleClickHandler);

            ParentSlot = parentSlot;
            DoubleClickHandler = doubleClickHandler;
        }
    }
}