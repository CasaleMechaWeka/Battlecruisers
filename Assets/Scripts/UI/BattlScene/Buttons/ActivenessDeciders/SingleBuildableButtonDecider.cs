using System;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders
{
    // FELIX  Delete
    public class SingleBuildableButtonDecider : IActivenessDecider
    {
        private readonly IBuildable _buildable;
        private readonly IActivenessDecider<IBuildable> _generalDecider;

        public bool ShouldBeEnabled { get { return _generalDecider.ShouldBeEnabled(_buildable); } }

        public event EventHandler PotentialActivenessChange
        {
            add { _generalDecider.PotentialActivenessChange += value; }
            remove { _generalDecider.PotentialActivenessChange -= value; }
        }

        public SingleBuildableButtonDecider(IBuildable buildable, IActivenessDecider<IBuildable> generalDecider)
        {
            Helper.AssertIsNotNull(buildable, generalDecider);

            _buildable = buildable;
            _generalDecider = generalDecider;
        }
    }
}
