using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuildingCategory
{
	Factory, Turret, Support, Tactical, AntiCruiser, Experimental
}

public class Building : MonoBehaviour
{
	private Renderer _renderer;

	public string buildingName;
	public string description;
	public int numOfDronesRequired;
	public int buildTimeInS;
	public BuildingCategory category;
	public SlotType slotType;
	public float health;
	// FELIX  Load from file?
	public ITurretStats turretStats;

	public virtual Vector3 Size 
	{ 
		get 
		{ 
			return _renderer.bounds.size; 
		} 
	}

	protected Sprite _buidlingSprite;
	public virtual Sprite BuildingSprite
	{
		get
		{
			if (_buidlingSprite == null)
			{
				_buidlingSprite = GetComponent<SpriteRenderer>().sprite;
			}
			return _buidlingSprite;
		}
	}

	void Awake()
	{
		Debug.Log("Building.Awake()");
		_renderer = GetComponent<Renderer>();
	}
}
