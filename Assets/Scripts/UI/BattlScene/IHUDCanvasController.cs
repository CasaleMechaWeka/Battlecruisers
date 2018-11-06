using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Common.BuildableDetails;

namespace BattleCruisers.UI.BattleScene
{
    public interface IHUDCanvasController : IInformatorPanel
    {
        ICruiserInfo PlayerCruiserInfo { get; }
        INavigationButtonsWrapper NavigationButtonsWrapper { get; }
        IGameSpeedWrapper GameSpeedWrapper { get; }
    }
}
