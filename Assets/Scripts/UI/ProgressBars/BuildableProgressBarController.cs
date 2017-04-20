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
		public void Initialise(Buildable buildable)
		{
			buildable.BuildableProgress += Buildable_BuildableProgress;
		}

		private void Buildable_BuildableProgress(object sender, BuildProgressEventArgs e)
		{
			OnProgressChanged(e.BuildProgress);
		}
	}
}
