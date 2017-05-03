using BattleCruisers.Buildables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ProgressBars
{
	public class BuildableProgressBarController : BaseProgressBarController
	{
		private Buildable _buildable;

		public void Initialise(Buildable buildable)
		{
			_buildable = buildable;
			_buildable.BuildableProgress += Buildable_BuildableProgress;
		}

		private void Buildable_BuildableProgress(object sender, BuildProgressEventArgs e)
		{
			OnProgressChanged(e.Buildable.BuildProgress);
		}

		public void Cleanup()
		{
			_buildable.BuildableProgress -= Buildable_BuildableProgress;
		}
	}
}
