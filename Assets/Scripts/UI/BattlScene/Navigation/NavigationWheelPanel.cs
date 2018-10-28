using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheelPanel : INavigationWheelPanel
    {
        private readonly IPyramid _panelArea;
        private readonly ITransform _navigationWheelTransform;

        private Vector2 NavigationWheelPosition { get { return _navigationWheelTransform.Position; } }

        public NavigationWheelPanel(IPyramid panelArea, ITransform navigationWheelTransform)
        {
            Helper.AssertIsNotNull(panelArea, navigationWheelTransform);

            _panelArea = panelArea;
            _navigationWheelTransform = navigationWheelTransform;
        }

        public float FindNavigationWheelYPositionAsProportionOfMaxHeight()
        {
            // FELIX
            return 0;
        }

        public float FindNavigationWheelXPositionAsProportionOfValidWidth()
        {
            // FELIX
            return 0;
        }
    }
}