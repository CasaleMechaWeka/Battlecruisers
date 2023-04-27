using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public class PvPBuildableProgressController : MonoBehaviour
    {
        private IPvPBuildable _buildable;

        public Image FillableImage { get; private set; }
        public Image OutlineImage { get; private set; }

        public void Initialise()
        {
            Logging.LogMethod(Tags.PROGRESS_BARS);

            _buildable = gameObject.GetComponentInInactiveParent<IPvPBuildable>();
            Assert.IsNotNull(_buildable);

            FillableImage = transform.FindNamedComponent<Image>("Canvas/FillableImage");
            OutlineImage = transform.FindNamedComponent<Image>("Canvas/OutlineImage");

            FillableImage.fillAmount = 0;
            gameObject.SetActive(false);

            _buildable.StartedConstruction += Buildable_StartedBuilding;
            _buildable.BuildableProgress += Buildable_BuildableProgress;
            _buildable.CompletedBuildable += Buildable_CompletedOrDestroyedBuilding;
            _buildable.Destroyed += Buildable_CompletedOrDestroyedBuilding;
        }

        private void Buildable_StartedBuilding(object sender, EventArgs e)
        {
            Logging.Log(Tags.PROGRESS_BARS, $"Show build progress for {_buildable}");
            gameObject.SetActive(true);
        }

        private void Buildable_BuildableProgress(object sender, PvPBuildProgressEventArgs e)
        {
            Logging.Verbose(Tags.PROGRESS_BARS, "e.Buildable.BuildProgress: " + e.Buildable.BuildProgress);

            Assert.IsTrue(e.Buildable.BuildProgress >= 0);
            FillableImage.fillAmount = e.Buildable.BuildProgress;
        }

        private void Buildable_CompletedOrDestroyedBuilding(object sender, EventArgs e)
        {
            Logging.Log(Tags.PROGRESS_BARS, $"Hide build progress for {_buildable}");
            gameObject.SetActive(false);
        }
    }
}
