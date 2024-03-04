using UnityEngine;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Sound;
using BattleCruisers.AI;

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
        BackgroundImageStats BackgroundImage { get; }
        IArtificialIntelligence AIStrategy { get; }
        bool IsCompleted { get; }
        int SideLevelNum { get; }

    }
}