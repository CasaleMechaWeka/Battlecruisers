using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStatsController : MonoBehaviour 
{
	// FELIX  Inject
	private ValueToStarsConverter _valueToStarsConverter;

	public StatsRowNumberController droneRow;
	public StatsRowNumberController buildTimeRow;
	public StatsRowStarsController healthRow;
	public StatsRowStarsController damageRow;

	private const string HEALTH_LABEL = "Health";
	private const string DAMAGE_LABEL = "Damage";
	private const string DRONES_LABEL = "Drones";
	private const string BUILD_TIME_LABEL = "BuildTime";

	private const string BUILD_TIME_SUFFIX = "s";

	void Start()
	{
		_valueToStarsConverter = new ValueToStarsConverter();
	}

	public void ShowBuildingStats(Building building)
	{
		droneRow.Initialise(DRONES_LABEL, building.numOfDronesRequired.ToString());
		buildTimeRow.Initialise(BUILD_TIME_LABEL, building.buildTimeInS.ToString() + BUILD_TIME_SUFFIX);
		healthRow.Initialise(HEALTH_LABEL, _valueToStarsConverter.HealthValueToStars(building.health));
		// FELIX
		damageRow.Initialise(DAMAGE_LABEL, _valueToStarsConverter.DamageValueToStars(25));
//		damageRow.Initialise(DAMAGE_LABEL, _valueToStarsConverter.DamageValueToStars(building.turretStats.DamangePerS));
	}
}
