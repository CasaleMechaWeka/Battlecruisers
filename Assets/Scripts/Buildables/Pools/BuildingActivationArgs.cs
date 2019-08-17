using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Buildables.ActivationArgs
{
    public class BuildingActivationArgs : BuildableActivationArgs
    {
        public ISlot ParentSlot { get; }
        public IDoubleClickHandler<IBuilding> DoubleClickHandler { get; }

        public BuildingActivationArgs(
            ICruiser parentCruiser,
            ICruiser enemyCruiser,
            ICruiserSpecificFactories cruiserSpecificFactories,
            ISlot parentSlot,
            IDoubleClickHandler<IBuilding> doubleClickHandler)
            : base(parentCruiser, enemyCruiser, cruiserSpecificFactories)
        {
            Helper.AssertIsNotNull(parentSlot, doubleClickHandler);

            ParentSlot = parentSlot;
            DoubleClickHandler = doubleClickHandler;
        }
    }
}