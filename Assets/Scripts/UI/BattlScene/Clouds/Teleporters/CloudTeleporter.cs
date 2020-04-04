using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Teleporters
{
    public class CloudTeleporter
    {
        private readonly IUpdater _updater;
        private readonly ITeleporterHelper _teleporterHelper;
        private ICloud _leftCloud, _rightCloud;

        public CloudTeleporter(IUpdater updater, ITeleporterHelper teleporterHelper, ICloud leftCloud, ICloud rightCloud)
        {
            Helper.AssertIsNotNull(updater, teleporterHelper, leftCloud, rightCloud);
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