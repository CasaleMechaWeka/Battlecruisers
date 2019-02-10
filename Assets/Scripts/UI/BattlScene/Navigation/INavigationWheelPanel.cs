using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface INavigationWheelPanel
    {
        INavigationWheel NavigationWheel { get; }
        IPyramid PanelArea { get; }

        /// <returns>
        /// The navigation wheel y position as a proportion of the maximum
        /// height of the panel pyramid area:  0-1
        /// </returns>
        float FindYProportion();

        /// <summary>
        /// Reverse of FindYProportion.
        /// </summary>
        float FindYPosition(float yProportion);

        /// <returns>
        /// The navigation wheel x position as a proportion of valid
        /// width at the current wheel y position:  0-1
        /// </returns>
        float FindXProportion();

        /// <summary>
        /// Reverse of FindXProportion.
        /// </summary>
        float FindXPosition(float xProportion, float navigationWheelYPosition);
    }
}