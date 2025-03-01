using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.Utils.BattleScene.Update;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Teleporters
{
    public class PvPCloudTeleporter
    {
        private readonly IUpdater _updater;
        private readonly IPvPTeleporterHelper _teleporterHelper;
        private ICloud _leftCloud, _rightCloud;

        public PvPCloudTeleporter(IUpdater updater, IPvPTeleporterHelper teleporterHelper, ICloud leftCloud, ICloud rightCloud)
        {
            PvPHelper.AssertIsNotNull(updater, teleporterHelper, leftCloud, rightCloud);
            Assert.IsTrue(leftCloud.Position.x < rightCloud.Position.x);

            _updater = updater;
            _teleporterHelper = teleporterHelper;
            _leftCloud = leftCloud;
            _rightCloud = rightCloud;

            _updater.Updated += _updater_Updated;
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (!_teleporterHelper.ShouldTeleportCloud(_rightCloud))
            {
                return;
            }

            _rightCloud.Position = _teleporterHelper.FindTeleportTargetPosition(_leftCloud, _rightCloud);

            // Switch clouds
            ICloud newLeftCloud = _rightCloud;
            _rightCloud = _leftCloud;
            _leftCloud = newLeftCloud;
        }
    }
}