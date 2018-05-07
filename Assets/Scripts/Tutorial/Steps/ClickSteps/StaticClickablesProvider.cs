using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    // FELIX  Test
    public class StaticClickablesProvider : IClickablesProvider
    {
        private readonly IList<IClickable> _clickables;

        public StaticClickablesProvider(params IClickable[] clickables)
        {
            Assert.IsNotNull(clickables);
            Assert.IsTrue(clickables.Length > 0);

            _clickables = clickables;
        }

        public IList<IClickable> FindClickables()
        {
            return _clickables;
        }
    }
}
