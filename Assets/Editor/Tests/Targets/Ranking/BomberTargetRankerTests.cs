using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets.Ranking
{
	public class BomberTargetRankerTests 
	{
		private ITargetRanker _targetRanker;

		private List<ITarget> _rankedTargets;
		private IList<ITarget> _expectedOrder;

		[SetUp]
		public void SetuUp()
		{
			_targetRanker = new BomberTargetRanker();
		}

		[Test]
		public void NoAttackCapabilities_RankedByValue()
		{
			ITarget lowValue = CreateMockTarget(TargetValue.Low);
			ITarget mediumValue = CreateMockTarget(TargetValue.Medium);
			ITarget highValue = CreateMockTarget(TargetValue.High);

			_rankedTargets = new List<ITarget>(new ITarget[] { mediumValue, highValue, lowValue });
			_rankedTargets.Sort((x, y) => _targetRanker.RankTarget(x) - _targetRanker.RankTarget(y));

			_expectedOrder = new List<ITarget>(new ITarget[] { lowValue, mediumValue, highValue });

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}

		private ITarget CreateMockTarget(TargetValue targetValue, IList<TargetType> attackCapabilities = null)
		{
			if (attackCapabilities == null)
			{
				attackCapabilities = new List<TargetType>();
			}

			ITarget target = Substitute.For<ITarget>();
			target.TargetValue.Returns(targetValue);
			target.AttackCapabilities.Returns(attackCapabilities);
			return target;
		}
	}
}
