namespace BattleCruisers.UI.BattleScene.Navigation
{
    public interface INavigationWheelPanel
    {
        // FELIX  Rename both methods :)

        /// <returns>0-1</returns>
        float FindNavigationWheelYPositionAsProportionOfMaxHeight();

        /// <returns>0-1</returns>
        float FindNavigationWheelXPositionAsProportionOfValidWidth();
    }
}