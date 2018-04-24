using BattleCruisers.Data.Serialization;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Data
{
    public static class ApplicationModel
	{
		public static int SelectedLevel { get; set; }
        public static bool ShowPostBattleScreen { get; set; }
		public static bool IsTutorial { get; set; }

		private static IDataProvider _dataProvider;
		public static IDataProvider DataProvider
		{
			get
			{
				if (_dataProvider == null)
				{
					_dataProvider = new DataProvider(
						new StaticData(),
						new Serializer(new ModelFilePathProvider()),
                        new SettingsManager());
				}
				return _dataProvider;
			}
		}

		static ApplicationModel()
		{
			SelectedLevel = -1;
            ShowPostBattleScreen = false;
            IsTutorial = false;
		}
	}
}
