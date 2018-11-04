using BattleCruisers.Data.Serialization;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Data
{
    /// <summary>
    /// Static class that allows scenes to communicate with each other.
    /// </summary>
    public static class ApplicationModelProvider
    {
        public static IApplicationModel ApplicationModel { get; private set; }

        static ApplicationModelProvider()
		{
            IDataProvider dataProvider 
                = new DataProvider(
                    new StaticData(),
                    new Serializer(new ModelFilePathProvider()),
                    new SettingsManager());

            ApplicationModel = new ApplicationModel(dataProvider);
        }
    }
}
