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

        // FELIX  Check how much memory these take.  If decent, release (the non common ones at least :P)
        void ReleaseBattleSceneTable();
        void ReleaseCommonTable();
        void ReleaseScreensSceneTable();
        void ReleaseStoryTable();
        void ReleaseTutorialTable();
    }
}