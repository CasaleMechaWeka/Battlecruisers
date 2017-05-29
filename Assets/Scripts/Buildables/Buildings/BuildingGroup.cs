using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings
{
	public class BuildingGroup
	{
		public IList<BuildingWrapper> Buildings { get; private set; }
		public BuildingCategory BuildingCategory { get; private set; }
		public string BuildingGroupName { get; private set; }
		public string Description { get; private set; }

		private int MIN_NUM_OF_BUILDINGS = 1;
		private int MAX_NUM_OF_BUILDINGS = 5;

		public BuildingGroup(
			IList<BuildingWrapper> buildings,
			string groupName,
			string description)
		{
			if (buildings.Count < MIN_NUM_OF_BUILDINGS || buildings.Count > MAX_NUM_OF_BUILDINGS)
			{
				throw new ArgumentException("Invalid building count: " + buildings.Count);
			}

			// Check building category matches this group's category
			if (buildings.Count > 0)
			{
				BuildingCategory = buildings[0].Building.category;

				for (int i = 1; i < buildings.Count; ++i)
				{
					if (buildings[i].Building.category != BuildingCategory)
					{
						throw new ArgumentException();
					}
				}
			}

			Buildings = buildings;
			BuildingGroupName = groupName;
			Description = description;
		}
	}
}
