using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationButtonsWrapper : UIElement, INavigationButtonsWrapper
    {
        public IButton PlayerCruiserButton { get; private set; }
        public IButton MidLeftButton { get; private set; }
        public IButton OverviewButton { get; private set; }
        public IButton MidRightButton { get; private set; }
        public IButton AICruiserButton { get; private set; }

        public void Initialise(ICameraController cameraController, IActivenessDecider navigationActivenessDecider)
        {
            base.Initialise();

            Helper.AssertIsNotNull(cameraController, navigationActivenessDecider);

            NavigationButtonController playerCruiserButton = transform.FindNamedComponent<NavigationButtonController>("PlayerCruiserButton");
            playerCruiserButton.Initialise(cameraController.FocusOnPlayerCruiser, navigationActivenessDecider);
            PlayerCruiserButton = playerCruiserButton;

            NavigationButtonController midLeftButton = transform.FindNamedComponent<NavigationButtonController>("MidLeftButton");
            midLeftButton.Initialise(cameraController.ShowMidLeft, navigationActivenessDecider);
            MidLeftButton = midLeftButton;

            NavigationButtonController overviewButton = transform.FindNamedComponent<NavigationButtonController>("OverviewButton");
            overviewButton.Initialise(cameraController.ShowFullMapView, navigationActivenessDecider);
            OverviewButton = overviewButton;

            NavigationButtonController midRightButton = transform.FindNamedComponent<NavigationButtonController>("MidRightButton");
            midRightButton.Initialise(cameraController.ShowMidRight, navigationActivenessDecider);
            MidRightButton = midRightButton;

            NavigationButtonController aiCruiserButton = transform.FindNamedComponent<NavigationButtonController>("AiCruiserButton");
            aiCruiserButton.Initialise(cameraController.FocusOnAiCruiser, navigationActivenessDecider);
            AICruiserButton = aiCruiserButton;
        }
    }
}
