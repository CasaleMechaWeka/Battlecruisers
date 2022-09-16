using System.Threading.Tasks;

namespace BattleCruisers.Utils.Localisation
{
    public interface ILocTableFactory
    {
        Task<ILocTable> LoadBattleSceneTableAsync();
        Task<ILocTable> LoadCommonTableAsync();
        Task<ILocTable> LoadScreensSceneTableAsync();
        Task<ILocTable> LoadStoryTableAsync();
        Task<ILocTable> LoadTutorialTableAsync();
        Task<ILocTable> LoadFontsTableAsync();

        Task<ILocTable> LoadAdvertisingTableAsync();

        void ReleaseBattleSceneTable();
        void ReleaseCommonTable();
        void ReleaseScreensSceneTable();
        void ReleaseStoryTable();
        void ReleaseTutorialTable();
        void ReleaseFontsTable();
        void ReleaseAdvertisingTable();
    }
}