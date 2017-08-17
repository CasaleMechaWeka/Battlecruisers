using System.Collections.Generic;
using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.FactoryManagers
{
	public class UnitChooserTests
	{
		[SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;
		}

		[Test]
		public void Constructor_NullUnitsThrows()
		{
            Assert.Throws<UnityAsserts.AssertionException>(() => new UnitChooser(units: null));
		}

		[Test]
		public void Constructor_EmptyUnitsThrows()
		{
            IList<IBuildableWrapper<IUnit>> units = new List<IBuildableWrapper<IUnit>>();
			Assert.Throws<UnityAsserts.AssertionException>(() => new UnitChooser(units));
		}
	}
}
