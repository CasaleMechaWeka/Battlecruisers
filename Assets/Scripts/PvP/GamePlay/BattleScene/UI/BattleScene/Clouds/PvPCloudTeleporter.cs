using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Teleporters
{
    public class PvPCloudTeleporter
    {
        private readonly IPvPUpdater _updater;
        private readonly IPvPTeleporterHelper _teleporterHelper;
        private IPvPCloud _leftCloud, _rightCloud;

        public PvPCloudTeleporter(IPvPUpdater updater, IPvPTeleporterHelper teleporterHelper, IPvPCloud leftCloud, IPvPCloud rightCloud)
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
            IPvPCloud newLeftCloud = _rightCloud;
            _rightCloud = _leftCloud;
            _leftCloud = newLeftCloud;
        }
    }
}