using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers.Sprites;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashTalkData
    {
        public string EnemySpritePath;
        public CaptainExoKey EnemyExoPrefabKey;

        public string EnemyNameKey;
        public string EnemyTextKey;
        public string PlayerTextKey;
        public string AppraisalDroneTextKey;

        public bool PlayerTalksFirst;
        public TrashTalkData(int levelNumber,
                             int exoId,
                             bool playerTalksFirst,
                             string stringKeyBasePrefix)
        {
            EnemySpritePath = SpritePaths.ExoImagesPath + StaticData.GetCaptainSpriteFilename(exoId);
            EnemyExoPrefabKey = StaticPrefabKeys.CaptainExos.GetCaptainExoKey(exoId);

            string stringKeyBase = $"{stringKeyBasePrefix}{levelNumber}";

            EnemyNameKey = $"{stringKeyBase}/name";
            EnemyTextKey = $"{stringKeyBase}/EnemyText";
            PlayerTextKey = $"{stringKeyBase}/PlayerText";
            AppraisalDroneTextKey = $"{stringKeyBase}/DroneText";

            PlayerTalksFirst = playerTalksFirst;
        }
    }
}