using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface ICreateProducingFactoryStepsFactory
    {
        FactoryStepsResult CreateSteps(IPrefabKey factoryKey, IPrefabKey unitKey);
    }
}