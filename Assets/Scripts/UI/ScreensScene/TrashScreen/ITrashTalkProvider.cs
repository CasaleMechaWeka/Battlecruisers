using System.Threading.Tasks;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public interface ITrashTalkProvider
    {
        Task<TrashTalkData> GetTrashTalkAsync(int levelNum, bool isSideQuest = false);
    }
}