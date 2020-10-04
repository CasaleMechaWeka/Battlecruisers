using System.Threading.Tasks;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public interface ITrashTalkProvider
    {
        Task<ITrashTalkData> GetTrashTalkAsync(int levelNum);
    }
}