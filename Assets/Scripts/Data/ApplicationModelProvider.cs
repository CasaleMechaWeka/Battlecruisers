using BattleCruisers.Data.Serialization;

namespace BattleCruisers.Data
{
    /// <summary>
    /// Static class that allows scenes to communicate with each other.
    /// </summary>
    public static class ApplicationModelProvider
    {
        public static IApplicationModel ApplicationModel { get; }

        static ApplicationModelProvider()
        {
            DataProvider.Initialise();

            ApplicationModel = new ApplicationModel();
        }
    }
}
