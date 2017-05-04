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
	/// <summary>
	/// Note:  Targets are ranked in ascending priority.
	/// </summary>
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
			RankTargets();

			_expectedOrder = new List<ITarget>(new ITarget[] { lowValue, mediumValue, highValue });

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}

		[Test]
		public void LowValueAntiAir_TrumpsHighValue()
		{
			ITarget lowValueAnitAir = CreateMockTarget(TargetValue.Low, TargetType.Aircraft);
			ITarget highValue = CreateMockTarget(TargetValue.High);

			_rankedTargets = new List<ITarget>(new ITarget[] { lowValueAnitAir, highValue });
			RankTargets();

			_expectedOrder = new List<ITarget>(new ITarget[] { highValue, lowValueAnitAir });

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}


		[Test]
		public void LowValueAntiCruiser_TrumpsHighValue()
		{
			ITarget lowValueAnitCruiser = CreateMockTarget(TargetValue.Low, TargetType.Cruiser);
			ITarget highValue = CreateMockTarget(TargetValue.High);

			_rankedTargets = new List<ITarget>(new ITarget[] { lowValueAnitCruiser, highValue });
			RankTargets();

			_expectedOrder = new List<ITarget>(new ITarget[] { highValue, lowValueAnitCruiser });

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}

		[Test]
		public void LowValueAntiAir_TrumpsHighValueAntiCruiser()
		{
			ITarget lowValueAnitAir = CreateMockTarget(TargetValue.Low, TargetType.Aircraft);
			ITarget highValueAntiCruiser = CreateMockTarget(TargetValue.High, TargetType.Cruiser);

			_rankedTargets = new List<ITarget>(new ITarget[] { lowValueAnitAir, highValueAntiCruiser });
			RankTargets();

			_expectedOrder = new List<ITarget>(new ITarget[] { highValueAntiCruiser, lowValueAnitAir });

			Assert.AreEqual(_expectedOrder, _rankedTargets);
		}

		private ITarget CreateMockTarget(TargetValue targetValue, params TargetType[] attackCapabilities)
		{
			ITarget target = Substitute.For<ITarget>();
			target.TargetValue.Returns(targetValue);
			target.AttackCapabilities.Returns(new List<TargetType>(attackCapabilities));
			return target;
		}

		private void RankTargets()
		{
			_rankedTargets.Sort((x, y) => _targetRanker.RankTarget(x) - _targetRanker.RankTarget(y));
		}
	}
}
