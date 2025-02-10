using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks
{
    public interface IPvPTaskFactory
    {
        IPvPPrioritisedTask CreateConstructBuildingTask(PvPTaskPriority taskPriority, IPrefabKey buildingKey);
    }
}