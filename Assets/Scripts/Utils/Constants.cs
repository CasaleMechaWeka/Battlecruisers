namespace BattleCruisers.Utils
{
	public static class Constants
	{
		public const float GRAVITY = 9.81f;  // m/s^2  => Match unity physics setting!
		public const float PI = 3.14f;
		public const int MS_PER_S = 1000;

		public const float DISABLED_UI_ALPHA = 0.5f;
		public const float ENABLED_UI_ALPHA = 1.0f;
    }

    public static class BuildSpeedMultipliers
    {
        // FELIX  Rename to avoid repeating suffix :P
        public const float NORMAL_BUILD_SPEED_MULTIPLIER = 2;
		public const float DEFAULT_TUTORIAL_BUILD_SPEED_MULTIPLIER = 8;
        public const float FAST_BUILD_SPEED_MULTIPLIER = 50;
		public const float INSANE_BUILD_SPEED_MULTIPLIER = 100;
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
    }
}
