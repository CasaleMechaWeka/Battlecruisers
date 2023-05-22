

using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public class PvPBuildableWrapper<TPvPBuildable> : PvPPrefab, IPvPBuildableWrapper<TPvPBuildable> where TPvPBuildable : class, IPvPBuildable
    {
        public TPvPBuildable Buildable { get; private set; }

        public PvPBuildableWrapper<TPvPBuildable> UnityObject => this;

        public override void StaticInitialise(ILocTable commonStrings)
        {
            Buildable = GetComponentInChildren<TPvPBuildable>();
            Assert.IsNotNull(Buildable);

            PvPHealthBarController healthBar = GetComponentInChildren<PvPHealthBarController>();
            Assert.IsNotNull(healthBar);

            Buildable.StaticInitialise(gameObject, healthBar, commonStrings);
        }
    }
}

