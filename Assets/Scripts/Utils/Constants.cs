namespace BattleCruisers.Utils
{
	public static class Constants
	{
		public const float GRAVITY = 9.81f;  // m/s^2  => Match unity physics setting!
		public const float PI = 3.14f;
		public const int MS_PER_S = 1000;

		public const float DISABLED_UI_ALPHA = 0.5f;
		public const float ENABLED_UI_ALPHA = 1.0f;

        public const float BUILD_SPEED_BALANCING_MULTIPLIER = 2;
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
