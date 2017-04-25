using BattleCruisers.Cruisers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TargetFinders
{
	public class TargetFinderFactory
	{
		private Cruiser _enemyCruiser;

		private BomberTargetFinder _bomberTargetFinder;
		public BomberTargetFinder BomberTargetFinder
		{
			get
			{
				if (_bomberTargetFinder == null)
				{
					_bomberTargetFinder = new BomberTargetFinder(_enemyCruiser);
				}
				return _bomberTargetFinder;
			}
		}

		public TargetFinderFactory(Cruiser enemyCruiser)
		{
			_enemyCruiser = enemyCruiser;
		}
	}
}
