using System.Threading.Tasks;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public interface ITrashTalkProvider
    {
        Task<ITrashTalkData> GetLevelTrashTalkAsync(int levelNum);
        Task<ITrashTalkData> GetSideQuestTrashTalkAsync(int sideQuestID);
    }
}