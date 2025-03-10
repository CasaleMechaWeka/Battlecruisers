namespace BattleCruisers.Utils
{
    public static class Constants
    {
        public const float GRAVITY = 9.81f;  // m/s^2  => Match unity physics setting!
        public const float PI = 3.14f;
        public const int MS_PER_S = 1000;
        public const float MAX_ACCURACY = 1;
        public const float DEFAULT_DOUBLE_CLICK_THRESHOLD_IN_S = 0.5f;
        public const int POPULATION_LIMIT = 30;
        public const float WATER_LINE = -1.4f;
    }

    public static class ZoomScale
    {
        public const float SCROLL_WHEEL = 1200;
        public const float SWIPE = 60;
    }

    public static class Alpha
    {
        public const float DISABLED = 0.4f;
        public const float PRESSED = 1f;
        public const float HOVER = 1f;
        public const float ENABLED = 1.0f;
    }

    public static class BuildSpeedMultipliers
    {
        public const float HALF_DEFAULT = 1;
        public const float POINT_65_DEFAULT = 1.3f;
        public const float POINT_7_DEFAULT = 1.4f;
        public const float POINT_93_DEFAULT = 1.86f;
        public const float DEFAULT = 2;
        public const float ONE_AND_A_QUARTER_DEFAULT = 2.5f;
        public const float ONE_AND_A_HALF_DEFAULT = 3;
        public const float DEFAULT_TUTORIAL = 3;
        public const float FAST = 50;
        public const float VERY_FAST = 500;
    }

    public static class SceneNames
    {
        public const string LANDING_SCENE = "LandingScene";
        public const string LOADING_SCENE = "LoadingScene";
        public const string BATTLE_SCENE = "BattleScene";
        public const string SCREENS_SCENE = "ScreensScene";
        public const string CREDITS_SCENE = "LonelyCreditsScene";
        public const string CUTSCENE_SCENE = "HuntressCutscene";
        public const string DESTRUCTION_SCENE = "DestructionScene";
        public const string STAGE_INTERSTITIAL_SCENE = "StageInterstitialScene";
        public const string PvP_BATTLE_SCENE = "PvPBattleScene";
        public const string PvP_DESTRUCTION_SCENE = "PvPDestructionScene";
        public const string PvP_BOOT_SCENE = "PvPBootScene";
        public const string PvP_INITIALIZE_SCENE = "PvPInitializeScene";
    }

    public static class GameObjectTags
    {
        public const string AIRCRAFT = "Aircraft";
        public const string SHIP = "Ship";
        public const string OFFENSIVE = "Offensive";
    }

    //these should be the same as specified in the LevelSets in screens scene, but are unlinked right now, so if changes to the level sets occur these must be updated.
    public static class LevelStages
    {
        public const int STAGE_1 = 0;
        public const int STAGE_2 = 3;
        public const int STAGE_3 = 7;
        public const int STAGE_4 = 10;
        public const int STAGE_5 = 15;
        public const int STAGE_6 = 18;
        public const int STAGE_7 = 22;
        public const int STAGE_8 = 26;
        public static int[] STAGE_STARTS = { STAGE_1, STAGE_2, STAGE_3, STAGE_4, STAGE_5, STAGE_6, STAGE_7, STAGE_8 };
    }
}
