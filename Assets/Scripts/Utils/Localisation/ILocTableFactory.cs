using System.Threading.Tasks;

namespace BattleCruisers.Utils.Localisation
{
    public interface ILocTableFactory
    {
        // FELIX  Add async suffix :D
        Task<ILocTable> LoadBattleSceneTable();
        Task<ILocTable> LoadCommonTable();
        Task<ILocTable> LoadScreensSceneTable();
        Task<ILocTable> LoadStoryTable();
        Task<ILocTable> LoadTutorialTable();

        // FELIX  Check how much memory these take.  If decent, release (the non common ones at least :P)
        void ReleaseBattleSceneTable();
        void ReleaseCommonTable();
        void ReleaseScreensSceneTable();
        void ReleaseStoryTable();
        void ReleaseTutorialTable();
    }
}