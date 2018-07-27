using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common
{
    public interface IClickHandlerFactory
    {
        IClickHandler CreateClickHandler(float doubleClickThresholdInS = Constants.DEFAULT_DOUBLE_CLICK_THRESHOLD_IN_S);
    }
}