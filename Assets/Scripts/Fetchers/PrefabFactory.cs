using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Fetchers
{
	public interface IPrefabFactory
	{
		BuildingWrapper GetBuildingWrapperPrefab(BuildingKey buildingKey);
		Building CreateBuilding(BuildingWrapper buildingWrapperPrefab);

		UnitWrapper GetUnitWrapperPrefab(UnitKey unitKey);
		Unit CreateUnit(UnitWrapper unitWrapperPrefab);

		Cruiser GetCruiserPrefab(HullKey hullKey);
		Cruiser CreateCruiser(Cruiser cruiserPrefab);
	}

	// FELIX  Surely I can use polymorphism for this...
	public class PrefabFactory : MonoBehaviour, IPrefabFactory
	{
		private PrefabFetcher _prefabFetcher;

		public void Initialise(PrefabFetcher prefabFetcher)
		{
			_prefabFetcher = prefabFetcher;
		}

		public BuildingWrapper GetBuildingWrapperPrefab(BuildingKey buildingKey)
		{
			BuildingWrapper buildingWrapperPrefab = _prefabFetcher.GetBuildingPrefab(buildingKey);

			// Awake() is synonymous to the prefabs constructor.  When the prefab is loaded,
			// Awake is called.  Because this prefab will never be loaded (only copies of it
			// made, and those copies will be loaded), need to explicitly call Awake().
//			buildingWrapperPrefab.Awake();
//			buildingWrapperPrefab.Building.Awake();

//			buildingWrapperPrefab.gameObject.SetActive(true);
//			buildingWrapperPrefab = Instantiate(buildingWrapperPrefab);
//			buildingWrapperPrefab.gameObject.transform.position = new Vector3(1000, 1000);
//			buildingWrapperPrefab.gameObject.SetActive(false);

			FakeAwake(buildingWrapperPrefab);

			return buildingWrapperPrefab;
		}

		public Building CreateBuilding(BuildingWrapper buildingWrapperPrefab)
		{
			return Instantiate(buildingWrapperPrefab).Building;
		}

		public UnitWrapper GetUnitWrapperPrefab(UnitKey unitKey)
		{
			UnitWrapper unitWrapperPrefab = _prefabFetcher.GetUnitPrefab(unitKey);

			// Awake() is synonymous to the prefabs constructor.  When the prefab is loaded,
			// Awake is called.  Because this prefab will never be loaded (only copies of it
			// made, and those copies will be loaded), need to explicitly call Awake().

//			unitWrapperPrefab.Awake();
//			unitWrapperPrefab.Unit.Awake();

//			unitWrapperPrefab.gameObject.SetActive(true);
//			unitWrapperPrefab = Instantiate(unitWrapperPrefab);
//			unitWrapperPrefab.gameObject.transform.position = new Vector3(1000, 1000);
//			unitWrapperPrefab.gameObject.SetActive(false);

			FakeAwake(unitWrapperPrefab);

			return unitWrapperPrefab;
		}

		public Unit CreateUnit(UnitWrapper unitWrapperPrefab)
		{
			return Instantiate(unitWrapperPrefab).Unit;
		}

		public Cruiser GetCruiserPrefab(HullKey hullKey)
		{
			return _prefabFetcher.GetCruiserPrefab(hullKey);
		}

		public Cruiser CreateCruiser(Cruiser cruiserPrefab)
		{
			return Instantiate(cruiserPrefab);
		}

		private void FakeAwake(BuildableWrapper buildableWrapper)
		{
			IAwakable[] awakables = buildableWrapper.GetComponentsInChildren<IAwakable>();
			foreach (IAwakable awakable in awakables)
			{
				awakable.Awake();
			}
		}
	}

	// FELIX  own file
	public interface IAwakable
	{
		void Awake();
	}
}
