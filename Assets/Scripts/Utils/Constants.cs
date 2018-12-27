namespace BattleCruisers.Utils
{
	public static class Constants
	{
		public const float GRAVITY = 9.81f;  // m/s^2  => Match unity physics setting!
		public const float PI = 3.14f;
		public const int MS_PER_S = 1000;

		public const float DISABLED_UI_ALPHA = 0.5f;
        public const float ENABLED_UI_ALPHA = 1.0f;
		
        public const float MAX_ACCURACY = 1;

        public const float DEFAULT_DOUBLE_CLICK_THRESHOLD_IN_S = 0.5f;
    }

    public static class BuildSpeedMultipliers
    {
        public const float POINT_3_DEFAULT = 0.6f;
        public const float POINT_7_DEFAULT = 1.4f;
        public const float DEFAULT = 2;
        public const float ONE_AND_A_HALF_DEFAULT = 3;
        public const float DEFAULT_TUTORIAL = 8;
        public const float FAST = 50;
		public const float VERY_FAST = 500;
    }

	public static class SceneNames
	{
		public const string BATTLE_SCENE = "BattleScene";
		public const string SCREENS_SCENE = "ScreensScene";
	}

    public static class GameObjectTags
    {
        public const string AIRCRAFT = "Aircraft";
        public const string SHIP = "Ship";
        public const string OFFENSIVE = "Offensive";
    }
}
