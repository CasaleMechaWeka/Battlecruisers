using System.Threading.Tasks;

namespace BattleCruisers.Utils.Localisation
{
    public interface ILocTableFactory
    {
        Task<ILocTable> LoadBattleSceneTable();
        Task<ILocTable> LoadCommonTable();
        Task<ILocTable> LoadScreensSceneTable();

        void ReleaseBattleSceneTable();
        void ReleaseCommonTable();
        void ReleaseScreensSceneTable();
    }
}