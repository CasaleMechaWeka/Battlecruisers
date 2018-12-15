using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface IConstructBuildingStepsFactory
    {
        IList<ITutorialStep> CreateSteps(
            BuildingCategory buildingCategory,
            BuildableInfo buildingToConstruct,
            SlotSpecification slotSpecification,
            string constructBuildingInstruction,
            bool waitForBuildingToComplete = true);
    }
}