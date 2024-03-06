using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data
{
    public interface ISideQuestData
    {
        bool PlayerTalksFirst { get; }
        IPrefabKey EnemyCaptainExo { get; }
        int UnlockRequirementLevel { get; }
        string UnlockRequirementQuestID { get; }
        // RewardKey StaticPrefabKeys { get; }
        // EnemyHull Hull { get; }
        SoundKeyPair MusicBackgroundKey { get; }
        // SkyMaterials SkyMaterial { get; }
        bool IsCompleted { get; }
        int SideLevelNum { get; }

    }
}