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
        public const float PRESSED = 0.6f;
        public const float HOVER = 0.8f;
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
	}

    public static class GameObjectTags
    {
        public const string AIRCRAFT = "Aircraft";
        public const string SHIP = "Ship";
        public const string OFFENSIVE = "Offensive";
    }
}
