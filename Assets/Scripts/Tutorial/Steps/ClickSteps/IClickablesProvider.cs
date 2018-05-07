using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    public interface IClickablesProvider
    {
        IList<IClickable> FindClickables();
    }
}
