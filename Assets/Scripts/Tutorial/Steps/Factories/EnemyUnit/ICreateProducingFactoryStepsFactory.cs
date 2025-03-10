using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Tutorial.Steps.Factories.EnemyUnit
{
    public interface ICreateProducingFactoryStepsFactory
    {
        FactoryStepsResult CreateSteps(IPrefabKey factoryKey, IPrefabKey unitKey);
    }
}