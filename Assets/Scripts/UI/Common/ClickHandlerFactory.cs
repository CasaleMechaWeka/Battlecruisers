using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common
{
    public class ClickHandlerFactory : IClickHandlerFactory
    {
        public IClickHandler CreateClickHandler(float doubleClickThresholdInS = Constants.DEFAULT_DOUBLE_CLICK_THRESHOLD_IN_S)
        {
            return new ClickHandler(doubleClickThresholdInS);
        }
    }
}